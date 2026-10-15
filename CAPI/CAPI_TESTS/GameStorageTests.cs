using CAPI_CORE;
using CAPI_CORE.Rules;
using CAPI_CORE.Storage;
using Xunit;

namespace CAPI_TESTS;

/// <summary>
/// Tests of the JSON files of a game: final export (US-09), save on a coffee break (US-10)
/// and resume from the save (US-11).
/// </summary>
/// <remarks>
/// Author: Valentin Perroux
/// </remarks>
public class GameStorageTests
{
    private static readonly Player[] players = { new("Alice"), new("Bob"), new("Chloé") };

    private static readonly Feature[] features =
    {
        new("F-01", "Créer un compte", "Un visiteur crée un compte."),
        new("F-02", "Se connecter"),
        new("F-03", "Rechercher un produit"),
        new("F-04", "Ajouter au panier"),
        new("F-05", "Payer la commande")
    };

    // Every player plays the same card, then the round is concluded.
    private static void PlayRound(GameSession session, Card card)
    {
        foreach (Player player in session.Players)
        {
            session.Vote(player, card);
        }

        session.ConcludeRound();
    }

    // Median game where F-01 and F-02 are validated, then every player plays the coffee card on F-03.
    private static GameSession CreateGameInterruptedOnThirdFeature()
    {
        GameSession session = new(players, features, new MedianRule());
        PlayRound(session, new Card("3", 3));
        PlayRound(session, new Card("5", 5));
        PlayRound(session, new Card(Card.CoffeeLabel));
        return session;
    }

    [Fact]
    public void CA_11_1_SaveThenLoad_RestoresSameGame()
    {
        // Arrange
        GameSession session = CreateGameInterruptedOnThirdFeature();

        // Act
        string json = GameStorage.SerializeSave(session);
        GameSession resumed = GameStorage.DeserializeSave(json);

        // Assert
        Assert.Equal(session.Players, resumed.Players);
        Assert.Equal(ValidationRules.MedianRuleName, resumed.Rule.Name);
        Assert.Equal(session.Features, resumed.Features);
        Assert.Equal(2, resumed.Estimates.Count);
        Assert.Equal(new Card("3", 3), resumed.Estimates[features[0]]);
        Assert.Equal(new Card("5", 5), resumed.Estimates[features[1]]);
        Assert.Equal("F-03", resumed.CurrentFeature.Id);
        Assert.Equal(1, resumed.RoundNumber);
    }

    [Fact]
    public void CA_11_1_ResumedGame_ContinuesWithChosenRule()
    {
        // Why it matters: the rule is stored in the save, so the median still applies after the resume.
        // Arrange
        GameSession resumed = GameStorage.DeserializeSave(GameStorage.SerializeSave(CreateGameInterruptedOnThirdFeature()));

        // Act
        resumed.Vote(players[0], new Card("3", 3));
        resumed.Vote(players[1], new Card("5", 5));
        resumed.Vote(players[2], new Card("8", 8));
        resumed.ConcludeRound();
        resumed.Vote(players[0], new Card("3", 3));
        resumed.Vote(players[1], new Card("5", 5));
        resumed.Vote(players[2], new Card("8", 8));
        resumed.ConcludeRound();

        // Assert
        Assert.Equal(new Card("5", 5), resumed.Estimates[features[2]]);
        Assert.Equal("F-04", resumed.CurrentFeature.Id);
    }

    [Fact]
    public void CA_10_1_CoffeeBreak_SaveContainsRulePlayersEstimatesAndCurrentFeature()
    {
        // Arrange
        GameSession session = CreateGameInterruptedOnThirdFeature();

        // Act
        string json = GameStorage.SerializeSave(session);

        // Assert
        Assert.Contains("\"regle\": \"mediane\"", json);
        Assert.Contains("\"Alice\"", json);
        Assert.Contains("\"Bob\"", json);
        Assert.Contains("\"Chloé\"", json);
        Assert.Contains("\"F-05\"", json);
        Assert.Contains("\"difficulte\": 5", json);
        Assert.Contains("\"fonctionnaliteCourante\": \"F-03\"", json);
    }

    [Fact]
    public void CA_09_1_EstimatedBacklog_ExportContainsRuleAndEachDifficulty()
    {
        // Arrange
        GameSession session = new(players, features.Take(3), new StrictRule());
        PlayRound(session, new Card("2", 2));
        PlayRound(session, new Card("8", 8));
        PlayRound(session, new Card("13", 13));

        // Act
        string json = GameStorage.SerializeExport(session);

        // Assert
        Assert.Contains("\"regle\": \"strict\"", json);
        Assert.Contains("\"difficulte\": 2", json);
        Assert.Contains("\"difficulte\": 8", json);
        Assert.Contains("\"difficulte\": 13", json);
    }

    [Fact]
    public void Export_BacklogNotEstimated_IsRefused()
    {
        // Arrange
        GameSession session = CreateGameInterruptedOnThirdFeature();

        // Act and Assert
        Assert.Throws<InvalidOperationException>(() => GameStorage.SerializeExport(session));
    }

    [Fact]
    public void Load_InvalidJson_IsRefused()
    {
        // Act and Assert
        Assert.Throws<InvalidSaveFileException>(() => GameStorage.DeserializeSave("{pas du json"));
    }

    [Fact]
    public void Load_UnknownRule_IsRefused()
    {
        // Arrange
        string json = GameStorage.SerializeSave(CreateGameInterruptedOnThirdFeature())
            .Replace("\"mediane\"", "\"moyenne\"");

        // Act and Assert
        Assert.Throws<InvalidSaveFileException>(() => GameStorage.DeserializeSave(json));
    }

    [Fact]
    public void Load_CurrentFeatureNotFirstUnestimated_IsRefused()
    {
        // Why it matters: features are estimated in order, so resuming on F-05 would skip F-03 and F-04.
        // Arrange
        string json = GameStorage.SerializeSave(CreateGameInterruptedOnThirdFeature())
            .Replace("\"fonctionnaliteCourante\": \"F-03\"", "\"fonctionnaliteCourante\": \"F-05\"");

        // Act and Assert
        Assert.Throws<InvalidSaveFileException>(() => GameStorage.DeserializeSave(json));
    }
}

using System.IO;
using CAPI_APP.Services;
using CAPI_CORE;
using CAPI_CORE.Rules;
using CAPI_CORE.Storage;
using CommunityToolkit.Mvvm.Input;

namespace CAPI_APP.ViewModels;

/// <summary>
/// Shows the card played by each player once the votes of the round are revealed,
/// and the decision taken by the rule engine for the round.
/// </summary>
/// <remarks>
/// Author: Corentin Legrand
/// </remarks>
public partial class RevealViewModel : InGameViewModel
{
    private readonly GameSession session;

    /// <summary>
    /// Gets the feature voted on during the revealed round.
    /// </summary>
    public Feature Feature { get; }

    /// <summary>
    /// Gets the card played by each player during the revealed round.
    /// </summary>
    public IReadOnlyDictionary<Player, Card> Votes { get; }

    /// <summary>
    /// Gets the decision taken by the rule engine for the revealed round.
    /// </summary>
    public VoteResult Result { get; }

    /// <summary>
    /// Gets a value indicating whether the game is over, because the backlog is estimated or the players asked for a coffee break.
    /// </summary>
    public bool HasGameEnded { get; }

    /// <summary>
    /// Gets a value indicating whether every player played the coffee card, so the game can be saved.
    /// </summary>
    /// <remarks>
    /// Author: Valentin Perroux
    /// </remarks>
    public bool IsCoffeeBreak { get; }

    /// <summary>
    /// Gets a value indicating whether every feature of the backlog is estimated, so the estimates can be exported.
    /// </summary>
    /// <remarks>
    /// Author: Valentin Perroux
    /// </remarks>
    public bool IsBacklogEstimated { get; }

    /// <summary>
    /// Gets the card retained for each feature validated so far.
    /// </summary>
    public IReadOnlyDictionary<Feature, Card> Estimates { get; }

    /// <summary>
    /// Gets a value indicating whether the team revotes because at least one player played the "?" card.
    /// </summary>
    public bool IsRevoteForQuestionMark { get; }

    /// <summary>
    /// Gets the names of the players who played the "?" card, separated by commas.
    /// </summary>
    public string QuestionMarkPlayerNames { get; }

    /// <summary>
    /// Gets a value indicating whether the team revotes because only some players played the coffee card.
    /// </summary>
    public bool IsRevoteForCoffee { get; }

    /// <summary>
    /// Gets a value indicating whether the team revotes only because the rule did not allow to validate the votes.
    /// </summary>
    public bool IsRevoteForDisagreement { get; }

    /// <summary>
    /// Occurs when the players move on to the next round.
    /// </summary>
    public event EventHandler? NextRoundRequested;

    /// <summary>
    /// Initializes a new <see cref="RevealViewModel"/> revealing and concluding the current round of the given game.
    /// </summary>
    /// <param name="session">The game whose current round has been voted by every player.</param>
    public RevealViewModel(GameSession session)
    {
        this.session = session;
        Feature = session.CurrentFeature;
        Votes = session.RevealVote();
        Result = session.ConcludeRound();
        IsCoffeeBreak = Result.Outcome == VoteOutcome.CoffeeBreak;
        IsBacklogEstimated = session.IsBacklogEstimated;
        HasGameEnded = IsCoffeeBreak || IsBacklogEstimated;
        Estimates = session.Estimates;

        bool isRevote = Result.Outcome == VoteOutcome.Revote;
        QuestionMarkPlayerNames = string.Join(", ", GetPlayerNamesWhoPlayed(Card.QuestionMarkLabel));
        IsRevoteForQuestionMark = isRevote && QuestionMarkPlayerNames.Length > 0;
        IsRevoteForCoffee = isRevote && GetPlayerNamesWhoPlayed(Card.CoffeeLabel).Any();
        IsRevoteForDisagreement = isRevote && !IsRevoteForQuestionMark && !IsRevoteForCoffee;
    }

    private IEnumerable<string> GetPlayerNamesWhoPlayed(string label)
    {
        return Votes.Where(vote => vote.Value.Label == label).Select(vote => vote.Key.Name);
    }

    [RelayCommand]
    private void StartNextRound()
    {
        NextRoundRequested?.Invoke(this, EventArgs.Empty);
    }

    /// <remarks>
    /// Author: Valentin Perroux
    /// </remarks>
    [RelayCommand]
    private void SaveGame()
    {
        string? path = DialogService.AskPathToSave("Sauvegarder la partie", "sauvegarde-capi.json");
        if (path is null)
        {
            return;
        }

        try
        {
            GameStorage.SaveGame(session, path);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            DialogService.ShowError($"La partie n'a pas pu être sauvegardée : {exception.Message}");
            return;
        }

        DialogService.ShowInformation($"Partie sauvegardée dans {path}. Vous pourrez la reprendre depuis le menu.");
        ReturnToSetup();
    }

    /// <remarks>
    /// Author: Valentin Perroux
    /// </remarks>
    [RelayCommand]
    private void ExportEstimates()
    {
        string? path = DialogService.AskPathToSave("Exporter les estimations", "estimations-capi.json");
        if (path is null)
        {
            return;
        }

        try
        {
            GameStorage.ExportEstimates(session, path);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            DialogService.ShowError($"Les estimations n'ont pas pu être exportées : {exception.Message}");
            return;
        }

        DialogService.ShowInformation($"Estimations exportées dans {path}.");
    }
}

using CAPI_CORE;
using CAPI_CORE.Rules;
using Xunit;

namespace CAPI_TESTS;

/// <summary>
/// Tests of the rule engine, one per line of the decision table (R2).
/// Tests named after a CA reuse its identifier; the others reuse the line number of the table.
/// </summary>
/// <remarks>
/// Author: Valentin Perroux
/// </remarks>
public class RuleEngineTests
{
    // Ligne 1 de la table
    [Fact]
    public void CA_06_2_Strict_Round1_UnanimousVotes_ValidatedWithFive()
    {
        // Arrange
        RuleEngine engine = new(new StrictRule());
        Card[] votes = { new("5", 5), new("5", 5), new("5", 5) };

        // Act
        VoteResult result = engine.Evaluate(votes, 1);

        // Assert
        Assert.Equal(VoteOutcome.Validated, result.Outcome);
        Assert.Equal(new Card("5", 5), result.RetainedCard);
    }

    // Ligne 2 de la table
    [Fact]
    public void CA_06_1_Strict_Round1_DifferentVotes_Revote()
    {
        // Arrange
        RuleEngine engine = new(new StrictRule());
        Card[] votes = { new("5", 5), new("5", 5), new("8", 8) };

        // Act
        VoteResult result = engine.Evaluate(votes, 1);

        // Assert
        Assert.Equal(VoteOutcome.Revote, result.Outcome);
    }

    // Ligne 3 de la table
    [Fact]
    public void Row3_Median_Round1_DifferentVotes_Revote()
    {
        // Pourquoi ce cas compte : au tour 1, la médiane doit exiger l'unanimité comme strict (consigne du sujet).
        // Arrange
        RuleEngine engine = new(new MedianRule());
        Card[] votes = { new("8", 8), new("8", 8), new("5", 5) };

        // Act
        VoteResult result = engine.Evaluate(votes, 1);

        // Assert
        Assert.Equal(VoteOutcome.Revote, result.Outcome);
    }

    // Ligne 4 de la table
    [Fact]
    public void CA_10_1_Strict_Round4_AllCoffee_CoffeeBreak()
    {
        // Arrange
        RuleEngine engine = new(new StrictRule());
        Card[] votes = { new("Café"), new("Café"), new("Café") };

        // Act
        VoteResult result = engine.Evaluate(votes, 4);

        // Assert
        Assert.Equal(VoteOutcome.CoffeeBreak, result.Outcome);
    }

    // Ligne 5 de la table
    [Fact]
    public void CA_07_1_Median_Round2_OddCount_ValidatedWithMiddleCard()
    {
        // Arrange
        RuleEngine engine = new(new MedianRule());
        Card[] votes = { new("3", 3), new("5", 5), new("8", 8) };

        // Act
        VoteResult result = engine.Evaluate(votes, 2);

        // Assert
        Assert.Equal(VoteOutcome.Validated, result.Outcome);
        Assert.Equal(new Card("5", 5), result.RetainedCard);
    }

    // Ligne 6 de la table
    [Fact]
    public void Row6_Median_Round2_QuestionMark_Revote()
    {
        // Pourquoi ce cas compte : « ? » n'a pas de valeur, on ne peut pas calculer de médiane avec (décision de la table).
        // Arrange
        RuleEngine engine = new(new MedianRule());
        Card[] votes = { new("?"), new("5", 5), new("8", 8) };

        // Act
        VoteResult result = engine.Evaluate(votes, 2);

        // Assert
        Assert.Equal(VoteOutcome.Revote, result.Outcome);
    }

    // Ligne 7 de la table
    [Fact]
    public void Row7_Median_Round2_EvenCount_ValidatedWithHigherMiddleCard()
    {
        // Pourquoi ce cas compte : en nombre pair, on retient la plus grande valeur centrale pour ne pas sous-estimer (décision de la table).
        // Arrange
        RuleEngine engine = new(new MedianRule());
        Card[] votes = { new("5", 5), new("5", 5), new("8", 8), new("8", 8) };

        // Act
        VoteResult result = engine.Evaluate(votes, 2);

        // Assert
        Assert.Equal(VoteOutcome.Validated, result.Outcome);
        Assert.Equal(new Card("8", 8), result.RetainedCard);
    }

    // Ligne 8 de la table
    [Fact]
    public void CA_10_2_Median_Round2_SomeCoffee_Revote()
    {
        // Arrange
        RuleEngine engine = new(new MedianRule());
        Card[] votes = { new("5", 5), new("5", 5), new("Café") };

        // Act
        VoteResult result = engine.Evaluate(votes, 2);

        // Assert
        Assert.Equal(VoteOutcome.Revote, result.Outcome);
    }
}

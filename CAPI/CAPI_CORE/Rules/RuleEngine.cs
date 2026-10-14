namespace CAPI_CORE.Rules;

/// <summary>
/// Decides the outcome of a round from the revealed votes, the round number and the rule chosen for the game.
/// It applies what is common to every rule (first round played unanimously, "?" card, coffee card)
/// and delegates the rest to the chosen <see cref="IValidationRule"/>.
/// </summary>
/// <remarks>
/// Author: Valentin Perroux
/// </remarks>
public class RuleEngine
{
    private const string CoffeeLabel = "Café";

    private readonly IValidationRule rule;
    private readonly IValidationRule firstRoundRule = new StrictRule();

    /// <summary>
    /// Initializes a new <see cref="RuleEngine"/> applying the given rule from the second round onwards.
    /// </summary>
    /// <param name="rule">The rule chosen for the game.</param>
    public RuleEngine(IValidationRule rule)
    {
        this.rule = rule;
    }

    /// <summary>
    /// Evaluates the revealed votes of a round.
    /// </summary>
    /// <param name="votes">The cards revealed in the round, one per player.</param>
    /// <param name="roundNumber">The number of the round on the current feature, starting at 1.</param>
    /// <returns>The decision taken for the round and, when validated, the retained card.</returns>
    public VoteResult Evaluate(IReadOnlyList<Card> votes, int roundNumber)
    {
        // Checked first: All() is true on an empty list, which would otherwise trigger a coffee break.
        if (votes.Count == 0)
        {
            return new VoteResult(VoteOutcome.Revote);
        }

        if (votes.All(card => card.Label == CoffeeLabel))
        {
            return new VoteResult(VoteOutcome.CoffeeBreak);
        }

        // "?" and a coffee card played by only some players carry no value: no estimate can be computed.
        if (votes.Any(card => card.Value is null))
        {
            return new VoteResult(VoteOutcome.Revote);
        }

        return roundNumber == 1 ? firstRoundRule.Evaluate(votes) : rule.Evaluate(votes);
    }
}

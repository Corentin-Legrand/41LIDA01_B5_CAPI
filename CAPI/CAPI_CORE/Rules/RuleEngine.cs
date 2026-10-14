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
    private readonly IValidationRule rule;

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
        throw new NotImplementedException();
    }
}

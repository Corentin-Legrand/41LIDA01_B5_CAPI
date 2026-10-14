namespace CAPI_CORE.Rules;

/// <summary>
/// Strict rule: the feature is validated only when every player played the same card.
/// </summary>
/// <remarks>
/// Author: Valentin Perroux
/// </remarks>
public class StrictRule : IValidationRule
{
    /// <inheritdoc />
    public VoteResult Evaluate(IReadOnlyList<Card> votes)
    {
        Card first = votes[0];
        return votes.All(card => card == first)
            ? new VoteResult(VoteOutcome.Validated, first)
            : new VoteResult(VoteOutcome.Revote);
    }
}

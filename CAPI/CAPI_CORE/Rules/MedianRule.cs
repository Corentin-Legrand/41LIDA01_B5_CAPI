namespace CAPI_CORE.Rules;

/// <summary>
/// Median rule: the feature is validated with the middle card of the sorted votes.
/// With an even number of votes, the higher of the two middle cards is retained.
/// </summary>
/// <remarks>
/// Author: Valentin Perroux
/// </remarks>
public class MedianRule : IValidationRule
{
    /// <inheritdoc />
    public string Name
    {
        get { return ValidationRules.MedianRuleName; }
    }

    /// <inheritdoc />
    public VoteResult Evaluate(IReadOnlyList<Card> votes)
    {
        List<Card> sorted = votes.OrderBy(card => card.Value).ToList();
        // Count / 2 is the middle index for an odd count and the higher middle index for an even count.
        return new VoteResult(VoteOutcome.Validated, sorted[sorted.Count / 2]);
    }
}

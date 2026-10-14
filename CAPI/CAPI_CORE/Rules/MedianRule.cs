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
    public VoteResult Evaluate(IReadOnlyList<Card> votes)
    {
        throw new NotImplementedException();
    }
}

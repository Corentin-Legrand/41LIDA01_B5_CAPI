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
        throw new NotImplementedException();
    }
}

namespace CAPI_CORE.Rules;

/// <summary>
/// Result of a round: the decision taken and, when the feature is validated, the retained card.
/// </summary>
/// <remarks>
/// Author: Valentin Perroux
/// </remarks>
public readonly struct VoteResult
{
    /// <summary>
    /// Gets the decision taken for the round.
    /// </summary>
    public VoteOutcome Outcome { get; init; }

    /// <summary>
    /// Gets the card retained as the estimate, or <see langword="null"/> when the feature is not validated.
    /// </summary>
    public Card? RetainedCard { get; init; }

    /// <summary>
    /// Initializes a new <see cref="VoteResult"/>.
    /// </summary>
    /// <param name="outcome">The decision taken for the round.</param>
    /// <param name="retainedCard">The card retained as the estimate, only when the feature is validated.</param>
    public VoteResult(VoteOutcome outcome, Card? retainedCard = null)
    {
        Outcome = outcome;
        RetainedCard = retainedCard;
    }
}

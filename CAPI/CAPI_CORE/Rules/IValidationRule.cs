namespace CAPI_CORE.Rules;

/// <summary>
/// A way of concluding a round from the second round onwards (strict, median...).
/// Adding a new rule means adding a new implementation, without modifying the existing ones.
/// </summary>
/// <remarks>
/// Author: Valentin Perroux
/// </remarks>
public interface IValidationRule
{
    /// <summary>
    /// Gets the name identifying the rule in the JSON files (export and save), such as "strict" or "mediane".
    /// </summary>
    /// <remarks>
    /// Author: Valentin Perroux
    /// </remarks>
    string Name { get; }

    /// <summary>
    /// Decides whether the revealed votes allow the feature to be validated.
    /// </summary>
    /// <param name="votes">The cards revealed in the round, containing only cards that carry a value.</param>
    /// <returns>The decision taken for the round.</returns>
    VoteResult Evaluate(IReadOnlyList<Card> votes);
}

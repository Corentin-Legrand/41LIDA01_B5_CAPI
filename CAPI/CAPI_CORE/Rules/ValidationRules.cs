namespace CAPI_CORE.Rules;

/// <summary>
/// Lists the validation rules implemented by the application and finds a rule from its name,
/// as stored in the JSON files (export and save).
/// </summary>
/// <remarks>
/// Author: Valentin Perroux
/// </remarks>
public static class ValidationRules
{
    /// <summary>
    /// The name of the strict rule (unanimity).
    /// </summary>
    public const string StrictRuleName = "strict";

    /// <summary>
    /// The name of the median rule.
    /// </summary>
    public const string MedianRuleName = "mediane";

    /// <summary>
    /// Creates the rule carrying the given name.
    /// </summary>
    /// <param name="name">The name of the rule, such as "strict" or "mediane".</param>
    /// <returns>A new instance of the rule.</returns>
    /// <exception cref="ArgumentException">Thrown when no implemented rule carries this name.</exception>
    public static IValidationRule FromName(string name)
    {
        return name switch
        {
            StrictRuleName => new StrictRule(),
            MedianRuleName => new MedianRule(),
            _ => throw new ArgumentException($"La règle « {name} » n'existe pas.", nameof(name))
        };
    }
}

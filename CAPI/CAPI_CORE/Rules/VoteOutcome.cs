namespace CAPI_CORE.Rules;

/// <summary>
/// Decision taken by the rule engine once the votes of a round are revealed.
/// </summary>
/// <remarks>
/// Author: Valentin Perroux
/// </remarks>
public enum VoteOutcome
{
    /// <summary>The feature is estimated with the retained card.</summary>
    Validated,

    /// <summary>The feature is not estimated: the players discuss and vote again.</summary>
    Revote,

    /// <summary>Every player played the coffee card: the game is saved and paused.</summary>
    CoffeeBreak
}

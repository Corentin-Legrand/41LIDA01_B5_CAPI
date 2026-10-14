using CAPI_CORE;
using CAPI_CORE.Rules;
using CommunityToolkit.Mvvm.Input;

namespace CAPI_APP.ViewModels;

/// <summary>
/// Shows the card played by each player once the votes of the round are revealed,
/// and the decision taken by the rule engine for the round.
/// </summary>
/// <remarks>
/// Author: Corentin Legrand
/// </remarks>
public partial class RevealViewModel : InGameViewModel
{
    /// <summary>
    /// Gets the feature voted on during the revealed round.
    /// </summary>
    public Feature Feature { get; }

    /// <summary>
    /// Gets the card played by each player during the revealed round.
    /// </summary>
    public IReadOnlyDictionary<Player, Card> Votes { get; }

    /// <summary>
    /// Gets the decision taken by the rule engine for the revealed round.
    /// </summary>
    public VoteResult Result { get; }

    /// <summary>
    /// Gets a value indicating whether the game is over, because the backlog is estimated or the players asked for a coffee break.
    /// </summary>
    public bool HasGameEnded { get; }

    /// <summary>
    /// Gets the card retained for each feature validated so far.
    /// </summary>
    public IReadOnlyDictionary<Feature, Card> Estimates { get; }

    /// <summary>
    /// Gets a value indicating whether the team revotes because at least one player played the "?" card.
    /// </summary>
    public bool IsRevoteForQuestionMark { get; }

    /// <summary>
    /// Gets the names of the players who played the "?" card, separated by commas.
    /// </summary>
    public string QuestionMarkPlayerNames { get; }

    /// <summary>
    /// Gets a value indicating whether the team revotes because only some players played the coffee card.
    /// </summary>
    public bool IsRevoteForCoffee { get; }

    /// <summary>
    /// Gets a value indicating whether the team revotes only because the rule did not allow to validate the votes.
    /// </summary>
    public bool IsRevoteForDisagreement { get; }

    /// <summary>
    /// Occurs when the players move on to the next round.
    /// </summary>
    public event EventHandler? NextRoundRequested;

    /// <summary>
    /// Initializes a new <see cref="RevealViewModel"/> revealing and concluding the current round of the given game.
    /// </summary>
    /// <param name="session">The game whose current round has been voted by every player.</param>
    public RevealViewModel(GameSession session)
    {
        Feature = session.CurrentFeature;
        Votes = session.RevealVote();
        Result = session.ConcludeRound();
        HasGameEnded = Result.Outcome == VoteOutcome.CoffeeBreak || session.IsBacklogEstimated;
        Estimates = session.Estimates;

        bool isRevote = Result.Outcome == VoteOutcome.Revote;
        QuestionMarkPlayerNames = string.Join(", ", GetPlayerNamesWhoPlayed(Card.QuestionMarkLabel));
        IsRevoteForQuestionMark = isRevote && QuestionMarkPlayerNames.Length > 0;
        IsRevoteForCoffee = isRevote && GetPlayerNamesWhoPlayed(Card.CoffeeLabel).Any();
        IsRevoteForDisagreement = isRevote && !IsRevoteForQuestionMark && !IsRevoteForCoffee;
    }

    private IEnumerable<string> GetPlayerNamesWhoPlayed(string label)
    {
        return Votes.Where(vote => vote.Value.Label == label).Select(vote => vote.Key.Name);
    }

    [RelayCommand]
    private void StartNextRound()
    {
        NextRoundRequested?.Invoke(this, EventArgs.Empty);
    }
}

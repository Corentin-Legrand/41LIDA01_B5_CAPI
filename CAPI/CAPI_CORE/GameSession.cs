using CAPI_CORE.Rules;

namespace CAPI_CORE;

/// <summary>
/// Represents a running planning poker game: its players, the features to estimate,
/// the votes of the current round and the estimates already validated.
/// </summary>
/// <remarks>
/// Author: Corentin Legrand
/// </remarks>
public class GameSession
{
    private const int FirstRoundNumber = 1;

    private readonly Dictionary<Player, Card> votes = new();
    private readonly List<Player> players = new();
    private readonly List<Feature> features = new();
    private readonly Dictionary<Feature, Card> estimates = new();
    private readonly RuleEngine ruleEngine;
    private readonly IValidationRule rule;
    private int currentFeatureIndex = 0;
    // private readonly Backlog backlog = new(); // Enable once Backlog is implemented.

    /// <summary>
    /// Initializes a new <see cref="GameSession"/> for the given players, features and validation rule.
    /// </summary>
    /// <param name="players">The players taking part in the game.</param>
    /// <param name="features">The features to estimate, in the order they are voted on.</param>
    /// <param name="rule">The rule chosen to conclude the rounds from the second round onwards.</param>
    public GameSession(IEnumerable<Player> players, IEnumerable<Feature> features, IValidationRule rule)
    {
        this.players.AddRange(players);
        this.features.AddRange(features);
        this.rule = rule;
        ruleEngine = new RuleEngine(rule);
    }

    /// <summary>
    /// Initializes a <see cref="GameSession"/> resumed from a saved game: the given estimates are restored
    /// and the game starts again at the first round of the first feature not estimated yet.
    /// </summary>
    /// <param name="players">The players taking part in the game.</param>
    /// <param name="features">The features to estimate, in the order they are voted on.</param>
    /// <param name="rule">The rule chosen to conclude the rounds from the second round onwards.</param>
    /// <param name="estimates">
    /// The card retained for each feature already validated. Features are estimated in order,
    /// so these must be the first features of the backlog.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the estimated features are not the first features of the backlog, in order.
    /// </exception>
    /// <remarks>
    /// Author: Valentin Perroux
    /// </remarks>
    public GameSession(IEnumerable<Player> players, IEnumerable<Feature> features, IValidationRule rule,
        IEnumerable<KeyValuePair<Feature, Card>> estimates)
        : this(players, features, rule)
    {
        foreach (KeyValuePair<Feature, Card> estimate in estimates)
        {
            if (currentFeatureIndex >= this.features.Count || !this.features[currentFeatureIndex].Equals(estimate.Key))
            {
                throw new ArgumentException(
                    "Les fonctionnalités estimées doivent être les premières du backlog, dans l'ordre.", nameof(estimates));
            }

            this.estimates[estimate.Key] = estimate.Value;
            currentFeatureIndex++;
        }
    }

    /// <summary>
    /// Gets the features to estimate, in the order they are voted on.
    /// </summary>
    /// <remarks>
    /// Author: Valentin Perroux
    /// </remarks>
    public IReadOnlyList<Feature> Features
    {
        get { return features; }
    }

    /// <summary>
    /// Gets the rule chosen to conclude the rounds from the second round onwards.
    /// </summary>
    /// <remarks>
    /// Author: Valentin Perroux
    /// </remarks>
    public IValidationRule Rule
    {
        get { return rule; }
    }

    /// <summary>
    /// Gets the feature currently being voted on.
    /// </summary>
    public Feature CurrentFeature
    {
        get { return features[currentFeatureIndex]; }
    }

    /// <summary>
    /// Gets the number of the current round on the current feature, starting at 1.
    /// </summary>
    public int RoundNumber { get; private set; } = FirstRoundNumber;

    /// <summary>
    /// Gets a value indicating whether every feature of the backlog has been estimated.
    /// </summary>
    public bool IsBacklogEstimated
    {
        get { return currentFeatureIndex >= features.Count; }
    }

    /// <summary>
    /// Gets the card retained for each feature validated so far.
    /// </summary>
    public IReadOnlyDictionary<Feature, Card> Estimates
    {
        get { return estimates; }
    }

    /// <summary>
    /// Gets the players taking part in the game, in the order they vote.
    /// </summary>
    public IReadOnlyList<Player> Players
    {
        get { return players; }
    }

    /// <summary>
    /// Gets a value indicating whether every player of the game has voted in the current round.
    /// </summary>
    public bool HasEveryPlayerVoted
    {
        get { return players.All(votes.ContainsKey); }
    }

    /// <summary>
    /// Records the card played by the given player for the current round, replacing the card
    /// this player may have already played in this round.
    /// </summary>
    /// <param name="player">The player who plays the card.</param>
    /// <param name="card">The card played.</param>
    public void Vote(Player player, Card card)
    {
        votes[player] = card;
    }

    /// <summary>
    /// Reveals the card played by each player during the current round.
    /// </summary>
    /// <returns>The card played by each player during the current round.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when at least one player of the game has not voted yet.
    /// </exception>
    public IReadOnlyDictionary<Player, Card> RevealVote()
    {
        if (!HasEveryPlayerVoted)
        {
            throw new InvalidOperationException("Votes cannot be revealed before every player has voted.");
        }

        return new Dictionary<Player, Card>(votes);
    }

    /// <summary>
    /// Concludes the current round with the rule engine and prepares the next one: a validated feature
    /// is recorded and the game moves to the next feature, a revote starts a new round on the same feature.
    /// </summary>
    /// <returns>The decision taken for the round.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when at least one player of the game has not voted yet.
    /// </exception>
    public VoteResult ConcludeRound()
    {
        VoteResult result = ruleEngine.Evaluate(RevealVote().Values.ToList(), RoundNumber);

        if (result.Outcome == VoteOutcome.Validated)
        {
            estimates[CurrentFeature] = result.RetainedCard!.Value;
            currentFeatureIndex++;
            RoundNumber = FirstRoundNumber;
        }
        else if (result.Outcome == VoteOutcome.Revote)
        {
            RoundNumber++;
        }

        votes.Clear();
        return result;
    }
}

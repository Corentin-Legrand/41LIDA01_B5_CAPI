namespace CAPI_CORE;

/// <summary>
/// Represents a running planning poker game: its players and the votes of the current round.
/// </summary>
/// <remarks>
/// Author: Corentin Legrand
/// </remarks>
public class GameSession
{
    private const int SingleDistinctVote = 1;

    private readonly Dictionary<Player, Card> votes = new();
    private readonly List<Player> players = new();
    // private readonly Backlog backlog = new(); // Enable once Backlog is implemented.

    /// <summary>
    /// Initializes a new <see cref="GameSession"/> for the given players.
    /// </summary>
    /// <param name="players">The players taking part in the game.</param>
    public GameSession(IEnumerable<Player> players)
    {
        this.players.AddRange(players);
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
    /// Gets a value indicating whether every player of the game played the same card in the current round.
    /// </summary>
    public bool IsVoteUnanimous
    {
        get { return HasEveryPlayerVoted && votes.Values.Distinct().Count() == SingleDistinctVote; }
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
}

using CAPI_CORE;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CAPI_APP.ViewModels;

/// <summary>
/// Lets each player of the game play a card in turn, without revealing the cards already played.
/// </summary>
/// <remarks>
/// Author: Corentin Legrand
/// </remarks>
public partial class VotingViewModel : InGameViewModel
{
    private readonly GameSession session;
    private int currentPlayerIndex = 0;

    /// <summary>
    /// Gets or sets the player whose turn it is to play a card.
    /// </summary>
    [ObservableProperty]
    public partial Player CurrentPlayer { get; set; }

    /// <summary>
    /// Gets the cards of the planning poker deck the current player can choose from.
    /// </summary>
    public IReadOnlyList<Card> Cards { get; } = GetDeck();

    /// <summary>
    /// Gets the feature being voted on.
    /// </summary>
    public Feature CurrentFeature
    {
        get { return session.CurrentFeature; }
    }

    /// <summary>
    /// Gets the number of the round being played on the current feature.
    /// </summary>
    public int RoundNumber
    {
        get { return session.RoundNumber; }
    }

    /// <summary>
    /// Occurs when every player has played a card and the votes can be revealed.
    /// </summary>
    public event EventHandler? VoteRevealed;

    /// <summary>
    /// Initializes a new <see cref="VotingViewModel"/> for the given game, starting with its first player.
    /// </summary>
    /// <param name="session">The game whose current round is being voted on.</param>
    public VotingViewModel(GameSession session)
    {
        this.session = session;
        CurrentPlayer = session.Players[currentPlayerIndex];
    }

    [RelayCommand]
    private void PlayCard(Card card)
    {
        session.Vote(CurrentPlayer, card);
        AdvanceToNextPlayer();
    }

    private void AdvanceToNextPlayer()
    {
        if (session.HasEveryPlayerVoted)
        {
            VoteRevealed?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            currentPlayerIndex++;
            CurrentPlayer = session.Players[currentPlayerIndex];
        }
    }

    private static IReadOnlyList<Card> GetDeck()
    {
        return new List<Card>
        {
            new("0", 0),
            new("1", 1),
            new("2", 2),
            new("3", 3),
            new("5", 5),
            new("8", 8),
            new("13", 13),
            new("20", 20),
            new("40", 40),
            new("100", 100),
            new(Card.QuestionMarkLabel),
            new(Card.CoffeeLabel)
        };
    }
}

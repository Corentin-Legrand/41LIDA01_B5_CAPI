using CAPI_CORE;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CAPI_APP.ViewModels;

/// <summary>
/// Drives the navigation between the screens of the application.
/// </summary>
/// <remarks>
/// Author: Corentin Legrand
/// </remarks>
public partial class MainViewModel : ObservableObject
{
    /// <summary>
    /// Gets the view model of the screen currently displayed.
    /// </summary>
    [ObservableProperty]
    public partial ObservableObject CurrentViewModel { get; set; }

    /// <summary>
    /// Initializes a new <see cref="MainViewModel"/> showing the setup screen.
    /// </summary>
    public MainViewModel()
    {
        SetupViewModel setup = new();
        setup.GameStarted += OnGameStarted;
        CurrentViewModel = setup;
    }

    private void OnGameStarted(object? sender, GameSession session)
    {
        VotingViewModel voting = new(session);
        voting.VoteRevealed += OnVoteRevealed;
        CurrentViewModel = voting;
    }

    private void OnVoteRevealed(object? sender, IReadOnlyDictionary<Player, Card> votes)
    {
        CurrentViewModel = new RevealViewModel(votes);
    }
}

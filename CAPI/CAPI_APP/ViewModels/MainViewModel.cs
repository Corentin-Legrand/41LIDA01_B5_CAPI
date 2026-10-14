using CAPI_APP.Mocks;
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
    /// Initializes a new <see cref="MainViewModel"/> showing the setup screen with the mock backlog loaded.
    /// </summary>
    public MainViewModel()
    {
        CurrentViewModel = CreateSetup();
    }

    private SetupViewModel CreateSetup()
    {
        SetupViewModel setup = new(BacklogMock.GetFeatures());
        setup.GameStarted += (_, session) => ShowVoting(session);
        return setup;
    }

    private void ShowVoting(GameSession session)
    {
        VotingViewModel voting = new(session);
        voting.VoteRevealed += (_, _) => ShowReveal(session);
        voting.ReturnToSetupRequested += (_, _) => CurrentViewModel = CreateSetup();
        CurrentViewModel = voting;
    }

    private void ShowReveal(GameSession session)
    {
        RevealViewModel reveal = new(session);
        reveal.NextRoundRequested += (_, _) => ShowVoting(session);
        reveal.ReturnToSetupRequested += (_, _) => CurrentViewModel = CreateSetup();
        CurrentViewModel = reveal;
    }
}

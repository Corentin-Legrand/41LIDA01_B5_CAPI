using System.Collections.ObjectModel;
using CAPI_CORE;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CAPI_APP.ViewModels;

/// <summary>
/// Collects the players of a new game and starts it once the configuration is validated.
/// </summary>
/// <remarks>
/// Author: Corentin Legrand
/// </remarks>
public partial class SetupViewModel : ObservableObject
{
    private const int MinimumPlayerCount = 2;

    /// <summary>
    /// Gets or sets the nickname being typed for the next player to add.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddPlayerCommand))]
    public partial string NewPlayerName { get; set; }

    /// <summary>
    /// Gets the players added to the game so far.
    /// </summary>
    public ObservableCollection<Player> Players { get; } = new();

    /// <summary>
    /// Occurs when the animator validates the configuration and starts the game.
    /// </summary>
    public event EventHandler<GameSession>? GameStarted;

    /// <summary>
    /// Initializes a new <see cref="SetupViewModel"/> with an empty list of players.
    /// </summary>
    public SetupViewModel()
    {
        NewPlayerName = string.Empty;
        Players.CollectionChanged += (_, _) => StartGameCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanAddPlayer))]
    private void AddPlayer()
    {
        Players.Add(new Player(NewPlayerName.Trim()));
        NewPlayerName = string.Empty;
    }

    private bool CanAddPlayer()
    {
        return !string.IsNullOrWhiteSpace(NewPlayerName);
    }

    [RelayCommand(CanExecute = nameof(CanStartGame))]
    private void StartGame()
    {
        GameStarted?.Invoke(this, new GameSession(Players));
    }

    private bool CanStartGame()
    {
        return Players.Count >= MinimumPlayerCount;
    }
}

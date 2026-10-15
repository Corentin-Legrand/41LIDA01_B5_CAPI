using System.Collections.ObjectModel;
using CAPI_APP.Services;
using CAPI_CORE;
using CAPI_CORE.Rules;
using CAPI_CORE.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CAPI_APP.ViewModels;

/// <summary>
/// Collects the players and the validation rule of a new game and starts it once the configuration is validated.
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
    [NotifyPropertyChangedFor(nameof(IsNewPlayerNameTaken))]
    public partial string NewPlayerName { get; set; }

    /// <summary>
    /// Gets a value indicating whether the nickname being typed is already used by a player of the game.
    /// </summary>
    public bool IsNewPlayerNameTaken
    {
        get { return Players.Contains(new Player(NewPlayerName.Trim())); }
    }

    /// <summary>
    /// Gets the players added to the game so far.
    /// </summary>
    public ObservableCollection<Player> Players { get; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether the median rule is chosen instead of the strict rule.
    /// </summary>
    [ObservableProperty]
    public partial bool IsMedianRuleSelected { get; set; }

    /// <summary>
    /// Gets the features of the loaded backlog, in the order they will be voted on.
    /// </summary>
    public IReadOnlyList<Feature> Features { get; }

    /// <summary>
    /// Occurs when the animator validates the configuration and starts the game.
    /// </summary>
    public event EventHandler<GameSession>? GameStarted;

    /// <summary>
    /// Initializes a new <see cref="SetupViewModel"/> with an empty list of players and the given backlog.
    /// </summary>
    /// <param name="features">The features of the loaded backlog.</param>
    public SetupViewModel(IReadOnlyList<Feature> features)
    {
        Features = features;
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
        return !string.IsNullOrWhiteSpace(NewPlayerName) && !IsNewPlayerNameTaken;
    }

    [RelayCommand(CanExecute = nameof(CanStartGame))]
    private void StartGame()
    {
        IValidationRule rule = IsMedianRuleSelected ? new MedianRule() : new StrictRule();
        GameStarted?.Invoke(this, new GameSession(Players, Features, rule));
    }

    private bool CanStartGame()
    {
        return Players.Count >= MinimumPlayerCount;
    }

    /// <remarks>
    /// Author: Valentin Perroux
    /// </remarks>
    [RelayCommand]
    private void ResumeGame()
    {
        string? path = DialogService.AskPathToOpen("Reprendre une partie sauvegardée");
        if (path is null)
        {
            return;
        }

        GameSession session;
        try
        {
            session = GameStorage.LoadGame(path);
        }
        catch (InvalidSaveFileException exception)
        {
            DialogService.ShowError($"Cette sauvegarde ne peut pas être reprise : {exception.Message}");
            return;
        }

        GameStarted?.Invoke(this, session);
    }
}

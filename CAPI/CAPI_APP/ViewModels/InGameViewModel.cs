using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CAPI_APP.ViewModels;

/// <summary>
/// Base of the screens shown while a game is running, from which the animator can go back to the setup screen.
/// </summary>
/// <remarks>
/// Author: Corentin Legrand
/// </remarks>
public abstract partial class InGameViewModel : ObservableObject
{
    /// <summary>
    /// Occurs when the animator leaves the running game to go back to the setup screen.
    /// </summary>
    public event EventHandler? ReturnToSetupRequested;

    /// <summary>
    /// Leaves the running game and goes back to the setup screen.
    /// </summary>
    /// <remarks>
    /// Modified by: Valentin Perroux (protected, so that a screen can end the game on its own after a save)
    /// </remarks>
    [RelayCommand]
    protected void ReturnToSetup()
    {
        ReturnToSetupRequested?.Invoke(this, EventArgs.Empty);
    }
}

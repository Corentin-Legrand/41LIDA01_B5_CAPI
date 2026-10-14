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

    [RelayCommand]
    private void ReturnToSetup()
    {
        ReturnToSetupRequested?.Invoke(this, EventArgs.Empty);
    }
}

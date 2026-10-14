using System.Windows;
using System.Windows.Controls;

namespace CAPI_APP.Controls;

/// <summary>
/// A button that asks the user to confirm before executing its command.
/// </summary>
/// <remarks>
/// Author: Corentin Legrand
/// </remarks>
public class ConfirmationButton : Button
{
    /// <summary>
    /// Gets or sets the question displayed in the confirmation dialog.
    /// </summary>
    public string ConfirmationMessage { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override void OnClick()
    {
        MessageBoxResult answer = MessageBox.Show(ConfirmationMessage, "Confirmation",
            MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (answer == MessageBoxResult.Yes)
        {
            base.OnClick();
        }
    }
}

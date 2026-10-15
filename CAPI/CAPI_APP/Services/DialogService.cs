using System.Windows;
using Microsoft.Win32;

namespace CAPI_APP.Services;

/// <summary>
/// Shows the dialogs used by the view models: choosing a JSON file to write or to read, and informing the user.
/// </summary>
/// <remarks>
/// Author: Valentin Perroux
/// </remarks>
public static class DialogService
{
    private const string JsonFilter = "Fichier JSON (*.json)|*.json";

    /// <summary>
    /// Asks the user where to write a JSON file.
    /// </summary>
    /// <param name="title">The title of the dialog.</param>
    /// <param name="defaultFileName">The file name suggested to the user.</param>
    /// <returns>The chosen path, or <see langword="null"/> when the user cancels.</returns>
    public static string? AskPathToSave(string title, string defaultFileName)
    {
        SaveFileDialog dialog = new()
        {
            Title = title,
            FileName = defaultFileName,
            Filter = JsonFilter,
            DefaultExt = ".json"
        };

        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }

    /// <summary>
    /// Asks the user which JSON file to read.
    /// </summary>
    /// <param name="title">The title of the dialog.</param>
    /// <returns>The chosen path, or <see langword="null"/> when the user cancels.</returns>
    public static string? AskPathToOpen(string title)
    {
        OpenFileDialog dialog = new()
        {
            Title = title,
            Filter = JsonFilter
        };

        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }

    /// <summary>
    /// Shows an information message.
    /// </summary>
    /// <param name="message">The message to show.</param>
    public static void ShowInformation(string message)
    {
        MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <summary>
    /// Shows an error message.
    /// </summary>
    /// <param name="message">The message to show.</param>
    public static void ShowError(string message)
    {
        MessageBox.Show(message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}

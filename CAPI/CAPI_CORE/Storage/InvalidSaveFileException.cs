namespace CAPI_CORE.Storage;

/// <summary>
/// The exception thrown when a save file cannot be read or does not describe a valid game.
/// </summary>
/// <remarks>
/// Author: Valentin Perroux
/// </remarks>
public class InvalidSaveFileException : Exception
{
    /// <summary>
    /// Initializes a new <see cref="InvalidSaveFileException"/> with a message explaining why the file is refused.
    /// </summary>
    /// <param name="message">The reason why the save file is refused.</param>
    /// <param name="innerException">The exception that caused the refusal, if any.</param>
    public InvalidSaveFileException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}

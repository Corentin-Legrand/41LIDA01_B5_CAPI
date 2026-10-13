namespace CAPI_CORE;

/// <summary>
/// Represents a backlog feature to be estimated during a planning poker game.
/// </summary>
/// <remarks>
/// Author: Corentin Legrand
/// </remarks>
public readonly struct Feature
{
    /// <summary>
    /// Gets the identifier of the feature within the backlog.
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Gets the title of the feature.
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// Gets the detailed description of the feature, or <see langword="null"/> when none is provided.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Initializes a new <see cref="Feature"/> with the given identifier, title and optional description.
    /// </summary>
    /// <param name="id">The identifier of the feature within the backlog.</param>
    /// <param name="title">The title of the feature.</param>
    /// <param name="description">The detailed description of the feature, or <see langword="null"/> when none is provided.</param>
    public Feature(string id, string title, string? description = null)
    {
        Id = id;
        Title = title;
        Description = description;
    }
}

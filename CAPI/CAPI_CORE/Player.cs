namespace CAPI_CORE;

/// <summary>
/// Represents a player taking part in a planning poker game.
/// </summary>
/// <remarks>
/// Author: Corentin Legrand
/// </remarks>
public readonly struct Player : IEquatable<Player>
{
    /// <summary>
    /// Gets the nickname identifying the player.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Initializes a new <see cref="Player"/> with the given nickname.
    /// </summary>
    /// <param name="name">The nickname identifying the player.</param>
    public Player(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Determines whether this player carries the same nickname as another player,
    /// using a case-sensitive ordinal comparison.
    /// </summary>
    /// <param name="other">The player to compare with this one.</param>
    /// <returns><see langword="true"/> when both nicknames are identical; otherwise <see langword="false"/>.</returns>
    public bool Equals(Player other)
    {
        return string.Equals(Name, other.Name, StringComparison.Ordinal);
    }

    /// <summary>
    /// Determines whether the given object is a <see cref="Player"/> carrying the same nickname as this one.
    /// </summary>
    /// <param name="obj">The object to compare with this player.</param>
    /// <returns><see langword="true"/> when the object is an equal player; otherwise <see langword="false"/>.</returns>
    public override bool Equals(object? obj)
    {
        return obj is Player other && Equals(other);
    }

    /// <summary>
    /// Returns a hash code consistent with <see cref="Equals(Player)"/>.
    /// </summary>
    /// <returns>The hash code of the player.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }

    /// <summary>
    /// Determines whether two players carry the same nickname.
    /// </summary>
    /// <param name="left">The first player to compare.</param>
    /// <param name="right">The second player to compare.</param>
    /// <returns><see langword="true"/> when both players are equal; otherwise <see langword="false"/>.</returns>
    public static bool operator ==(Player left, Player right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two players carry different nicknames.
    /// </summary>
    /// <param name="left">The first player to compare.</param>
    /// <param name="right">The second player to compare.</param>
    /// <returns><see langword="true"/> when both players differ; otherwise <see langword="false"/>.</returns>
    public static bool operator !=(Player left, Player right)
    {
        return !left.Equals(right);
    }
}

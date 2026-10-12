namespace CAPI_CORE;

/// <summary>
/// Represents a card of the planning poker deck.
/// </summary>
/// <remarks>
/// Author: Corentin Legrand
/// </remarks>
public readonly struct Card : IEquatable<Card>
{
    /// <summary>
    /// Gets the label displayed on the card.
    /// </summary>
    public string Label { get; init; }

    /// <summary>
    /// Gets the difficulty carried by the card, or <see langword="null"/> when the card carries no difficulty.
    /// </summary>
    public int? Value { get; init; }

    /// <summary>
    /// Initializes a new <see cref="Card"/> with the given label and difficulty.
    /// </summary>
    /// <param name="label">The label displayed on the card.</param>
    /// <param name="value">The difficulty carried by the card, or <see langword="null"/> when the card carries no difficulty.</param>
    public Card(string label, int? value = null)
    {
        Label = label;
        Value = value;
    }

    /// <summary>
    /// Determines whether this card carries the same label and difficulty as another card,
    /// the label being compared with a case-sensitive ordinal comparison.
    /// </summary>
    /// <param name="other">The card to compare with this one.</param>
    /// <returns><see langword="true"/> when both label and difficulty are identical; otherwise <see langword="false"/>.</returns>
    public bool Equals(Card other)
    {
        return string.Equals(Label, other.Label, StringComparison.Ordinal) && Value == other.Value;
    }

    /// <summary>
    /// Determines whether the given object is a <see cref="Card"/> equal to this one.
    /// </summary>
    /// <param name="obj">The object to compare with this card.</param>
    /// <returns><see langword="true"/> when the object is an equal card; otherwise <see langword="false"/>.</returns>
    public override bool Equals(object? obj)
    {
        return obj is Card other && Equals(other);
    }

    /// <summary>
    /// Returns a hash code consistent with <see cref="Equals(Card)"/>.
    /// </summary>
    /// <returns>The hash code of the card.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Label, Value);
    }

    /// <summary>
    /// Determines whether two cards carry the same label and difficulty.
    /// </summary>
    /// <param name="left">The first card to compare.</param>
    /// <param name="right">The second card to compare.</param>
    /// <returns><see langword="true"/> when both cards are equal; otherwise <see langword="false"/>.</returns>
    public static bool operator ==(Card left, Card right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two cards differ by their label or their difficulty.
    /// </summary>
    /// <param name="left">The first card to compare.</param>
    /// <param name="right">The second card to compare.</param>
    /// <returns><see langword="true"/> when both cards differ; otherwise <see langword="false"/>.</returns>
    public static bool operator !=(Card left, Card right)
    {
        return !left.Equals(right);
    }
}

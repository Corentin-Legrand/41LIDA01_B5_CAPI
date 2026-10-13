using CAPI_CORE;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CAPI_APP.ViewModels;

/// <summary>
/// Shows the card played by each player once the votes of the round are revealed.
/// </summary>
/// <remarks>
/// Author: Corentin Legrand
/// </remarks>
public class RevealViewModel : ObservableObject
{
    /// <summary>
    /// Gets the card played by each player during the revealed round.
    /// </summary>
    public IReadOnlyDictionary<Player, Card> Votes { get; }

    /// <summary>
    /// Initializes a new <see cref="RevealViewModel"/> displaying the given votes.
    /// </summary>
    /// <param name="votes">The card played by each player during the revealed round.</param>
    public RevealViewModel(IReadOnlyDictionary<Player, Card> votes)
    {
        Votes = votes;
    }
}

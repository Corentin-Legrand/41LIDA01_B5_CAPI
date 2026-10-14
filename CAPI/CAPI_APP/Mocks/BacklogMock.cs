using CAPI_CORE;

namespace CAPI_APP.Mocks;

/// <summary>
/// Provides a fixed backlog loaded at startup, used to try the voting and the rule engine
/// until the backlog can be loaded from a JSON file.
/// </summary>
/// <remarks>
/// Author: Corentin Legrand
/// </remarks>
public static class BacklogMock
{
    /// <summary>
    /// Gets the features of the mock backlog, in the order they are voted on.
    /// </summary>
    /// <returns>The features to estimate.</returns>
    public static IReadOnlyList<Feature> GetFeatures()
    {
        return new List<Feature>
        {
            new("F-01", "Créer un compte", "Un visiteur crée un compte avec son e-mail et un mot de passe."),
            new("F-02", "Se connecter", "Un utilisateur inscrit se connecte avec ses identifiants."),
            new("F-03", "Rechercher un produit", "Un utilisateur recherche un produit par mot-clé."),
            new("F-04", "Ajouter au panier", "Un utilisateur ajoute un produit à son panier."),
            new("F-05", "Payer la commande", "Un utilisateur paie le contenu de son panier par carte bancaire.")
        };
    }
}


## I. Principes généraux

Un code propre est **lisible, simple et cohérent**.  
Il doit être **facile à comprendre, maintenir et faire évoluer**.

**Objectifs :**
- Clarté avant tout.
- Pas de redondance (principe **DRY**).
- Cohérence du style et du vocabulaire.
- Code explicite plutôt que commenté.

---

## II. Nommage

### Règles générales
- **Tout le code est en anglais.**
- Les noms doivent être **explicites et cohérents**.
- Pas d’abréviations ou de préfixes inutiles.

### Méthodes et fonctions
- Nom sous forme **verbe + complément** :
  - Procédure (modifie un état) → `doSomething()`
  - Fonction (retourne une valeur) → `getSomething()`

```csharp
public User GetUserById(int id)
public Exercise CreateExercise(Exercise exercise)
```

### Collections et booléens
```csharp
private List<User> users; // ✅ pluriel
bool isActive; bool hasErrors; bool canExecute; // is - has - can
```

### Exceptions et tests
- Exceptions : les fichiers et classes se terminent par `Exception`
- Tests : les fichiers et classes se terminent par `Test` 
exemple -> fichier : `NomDeClasseTests.cs` / classe : `NomDeClasseTests`

---

### Conventions par langage

#### TypeScript
| Élément                              | Convention                  |
| ------------------------------------ | --------------------------- |
| Variables / Fonctions                | `camelCase`                 |
| Constantes                           | `UPPER_SNAKE_CASE`          |
| Classes / Interfaces / Types / Enums | `PascalCase` *(pas de “I”)* |
| Fichiers                             | `kebab-case`                |

#### C sharp#
| Élément            | Convention          |
| ------------------ | ------------------- |
| Variables          | `camelCase`         |
| Méthodes / Classes | `PascalCase`        |
| Interfaces         | `I` + `PascalCase`  |
| Fichiers           | `PascalCase`        |

```csharp
public interface IUser {}
```

---

## III. Typage

- **Toujours typer** attributs, paramètres, variables et retours.  

---

## IV. Documentation et commentaires

- Documentation **en anglais** pour tout (pas le privé).  
- Mentionner **l’auteur** dans la doc des classes.
- Mentionner **l’auteur** dans les méthodes modifiées (sinon par défaut on supposera que l'auteur d'une méthode = auteur de la classe)
- Commenter **seulement si nécessaire** (ex. regex ou logique complexe).  
- Si le code a besoin d’un commentaire, il doit souvent être simplifié.

##### Exemple en C sharp
``` C#
/// <summary>
/// Represents a user.
/// </summary>
/// <remarks>
/// Author: John Doe
/// </remarks>
public class User
{
}

/// <summary>
/// Retrieves a user by their unique identifier.
/// </summary>
/// <param name="id">The user's identifier.</param>
/// <returns>The <see cref="User"/> instance matching the given ID.</returns>
/// <exception cref="UserNotFoundException">
/// Thrown when no user exists with the specified ID.
/// </exception>
/// <remarks>
/// Author: Jane Smith
/// </remarks>
public static User GetUserById(int id)
{
    if (id <= 0)
        throw new UserNotFoundException($"User with ID {id} not found.");

    return someone;
}
```
##### Exemple en TS
``` TS
/**
 * Represents a user in the system.
 * @author John Doe
 */
export class User {}

/**
 * Retrieves a user by their unique identifier.
 * @param {number} id - The user's identifier.
 * @returns {User} The user instance matching the given ID.
 * @throws {UserNotFoundError} If no user exists with the specified ID.
 * @author Jane Smith
 */
static getUserById(id: number): User {
  if (id <= 0) {
    throw new UserNotFoundError(`User with ID ${id} not found.`);
  }
  return someone;
}
```
---

## V. Structure et style

### Organisation
- Classe ≤ **200 lignes** de code logique (hors documentation), méthode ≤ **30 lignes**.
- Méthodes avec peu d’arguments, faible imbrication (< 3 niveaux).
- Supprimer le **code mort**.
- **Initialiser** les variables à la déclaration.

### Style
```csharp
if (a)
{
    b;
}
```
- Une seule instruction `return` par méthode.
- Éviter les indirections multiples :  
  `var tmp = a.b; tmp.c = 3;`
- Ne pas comparer des flottants avec `==`.

---

## VI. Conception orientée objet

### Fonctions vs procédures
- **Fonction** → obtient un état.  
- **Procédure** → modifie un état.

### Objets vs structures
| Objet                       | Structure (DTO ou struct)  |
| --------------------------- | -------------------------- |
| Fournit un **comportement** | Contient des **données**   |
| Cache son implémentation    | Peut exposer ses attributs |
| Attributs privés            | Attributs publics tolérés  |
| Pas d’accesseurs directs    | Propriétés possibles       |

### Encapsulation
- Les objets **ne révèlent pas leur implémentation**.
- Les attributs sont **privés**.
- Les propriétés publiques sont **limitées**.

---

## VII. Gestion des erreurs

- Le code d’erreur doit être **isolé** du métier.  
- Ne pas retourner `null` : préférer **exceptions** ou **valeurs par défaut**.
- Les `try/catch` doivent être **courts et clairs**, idéalement dans la **couche IHM**.
- Ne pas utiliser les exceptions comme flux logique.

---

## VIII. Tests

### Règles FIRST
| Principe            | Signification       |
| ------------------- | ------------------- |
| **Fast**            | Rapide              |
| **Independent**     | Indépendant         |
| **Repeatable**      | Reproductible       |
| **Self-validating** | Résultat binaire    |
| **Timely**          | Écrit avant le code |

### Bonnes pratiques
- Un test = une vérification.
- Noms descriptifs et explicites.
- Tests automatisés, reproductibles, sans dépendances.

---

## IX. Constantes et valeurs magiques

- Ne pas utiliser de **valeurs “magiques”**.  
- Utiliser des **constantes** ou **énumérations**.

```csharp
public class Example
{
    public const int MaxRetryCount = 10;
}
```

---

## ✅ Résumé rapide

| Thème | Règle clé |
|--------|------------|
| **Lisibilité** | Code clair et cohérent |
| **Nommage** | En anglais, explicite |
| **Typage** | Toujours explicite |
| **Structure** | Petites classes et méthodes |
| **Erreurs** | Isolées, jamais silencieuses |
| **Objets** | Cachent leur implémentation |
| **Tests** | FIRST : Fast, Independent, Repeatable, Self-validating, Timely |

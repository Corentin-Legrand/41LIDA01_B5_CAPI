![CI](https://github.com/Corentin-Legrand/41LIDA01_B5_CAPI/actions/workflows/ci.yml/badge.svg)

# CAPI

Application de planning poker (C# .NET 10 / WPF).

## Prérequis

- SDK .NET 10 (`dotnet --list-sdks` doit afficher une version 10.0.x)
- Windows pour lancer l'application (WPF)

## Compiler et lancer l'application

Depuis la racine du dépôt :

```sh
dotnet run --project CAPI/CAPI_APP/CAPI_APP.csproj
```

## Lancer les tests

Depuis la racine du dépôt :

```sh
dotnet test CAPI/CAPI_TESTS/CAPI_TESTS.csproj
```
## Fichiers JSON (US-09, US-10, US-11)

Des exemples de chaque fichier se trouvent dans [`exemples/`](exemples/). La règle de jeu est stockée dans l'export et dans la sauvegarde, sous la clé `regle` : `strict` (unanimité) ou `mediane` (choisie dans le menu, appliquée à partir du deuxième tour).

**Export final** ([`exemples/export-estimations.json`](exemples/export-estimations.json)) : quand toutes les fonctionnalités sont estimées, le bouton « Exporter les estimations (JSON) » enregistre la règle et, pour chaque fonctionnalité, son `id`, son `titre`, sa `description` (facultative) et sa `difficulte`.

**Sauvegarde café** ([`exemples/sauvegarde-cafe.json`](exemples/sauvegarde-cafe.json)) : quand tous les joueurs jouent la carte café, le bouton « Sauvegarder la partie (JSON) » enregistre :

| Clé | Contenu |
|---|---|
| `regle` | la règle de la partie |
| `joueurs` | les pseudos, dans l'ordre de vote |
| `fonctionnalites` | tout le backlog (`id`, `titre`, `description`) |
| `estimations` | les fonctionnalités déjà validées (`id`, `difficulte`), dans l'ordre du backlog |
| `fonctionnaliteCourante` | l'`id` de la fonctionnalité en cours, la première non estimée |

**Reprise** : dans le menu, « Reprendre une partie sauvegardée » recharge ce fichier. La partie reprend au premier tour de la fonctionnalité courante, avec les mêmes joueurs et la même règle. Un fichier invalide (JSON mal formé, règle inconnue, moins de deux joueurs, fonctionnalité courante incohérente) est refusé avec un message, sans planter l'application.

## Tests du moteur de règles (US-06, US-07)

Les tests de `CAPI/CAPI_TESTS/RuleEngineTests.cs` correspondent chacun à une ligne de la table de décision du moteur de règles (voir le rendu R2 dans `docs/rendus/`). Les tests nommés d'après un critère d'acceptation (CA) reprennent son identifiant ; les autres reprennent le numéro de la ligne.

| Ligne | Test | Règle | Tour | Cas | Résultat attendu |
|---|---|---|---|---|---|
| 1 | `CA_06_2_Strict_Round1_UnanimousVotes_ValidatedWithFive` | Strict | 1 | Votes unanimes (5, 5, 5) | Validé avec la carte 5 |
| 2 | `CA_06_1_Strict_Round1_DifferentVotes_Revote` | Strict | 1 | Votes différents (5, 5, 8) | Nouveau vote |
| 3 | `Row3_Median_Round1_DifferentVotes_Revote` | Médiane | 1 | Votes différents (8, 8, 5) | Nouveau vote |
| 4 | `CA_10_1_Strict_Round4_AllCoffee_CoffeeBreak` | Strict | 4 | Tous les votes « Café » | Pause café |
| 5 | `CA_07_1_Median_Round2_OddCount_ValidatedWithMiddleCard` | Médiane | 2 | Nombre impair de votes (3, 5, 8) | Validé avec la carte centrale (5) |
| 6 | `Row6_Median_Round2_QuestionMark_Revote` | Médiane | 2 | Un vote « ? » (?, 5, 8) | Nouveau vote |
| 7 | `Row7_Median_Round2_EvenCount_ValidatedWithHigherMiddleCard` | Médiane | 2 | Nombre pair de votes (5, 5, 8, 8) | Validé avec la plus grande carte centrale (8) |
| 8 | `CA_10_2_Median_Round2_SomeCoffee_Revote` | Médiane | 2 | Au moins un vote « Café » (5, 5, Café) | Nouveau vote |

### Justification des cas

- **Ligne 3** : au tour 1, la médiane doit exiger l'unanimité comme la règle stricte (consigne du sujet).
- **Ligne 6** : « ? » n'a pas de valeur, on ne peut donc pas calculer de médiane avec (décision de la table).
- **Ligne 7** : en nombre pair, on retient la plus grande valeur centrale pour ne pas sous-estimer (décision de la table).

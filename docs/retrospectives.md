# Rétrospectives : binôme B05

## Partie A : rétrospective de sprint (sprint 1, séances 1 et 2)

### Faits

Estimations reprises du PV de planning poker de R1 (US-05 = 2 points sert de référence).

| US prévues | US terminées | Points prévus | Points réalisés (vélocité) |
|---|---|---|---|
| US-01 (5), US-03 (8), US-05 (2) | US-01, US-05 | 15 | 7 |

- US-03 (chargement du backlog JSON) n'est pas terminée : l'application utilise une liste de fonctionnalités écrite en dur (`CAPI_APP/Mocks/BacklogMock.cs`).
- US-02 (choix de la règle, 1 point) n'était pas prévue au sprint 1 mais a été livrée avec l'écran de configuration (PR #1).
- Écart assumé dans la DoD : les US du sprint 1 n'ont pas de tests xUnit (conditions 1 et 3), voir `docs/definition-of-done.md`.
- En séance 2, US-06 (1) et US-07 (3) ont été livrées avec 8 tests écrits avant le code (commit rouge 7e9aa0d, PR #2, PR #15).
- La PR #2 a été fusionnée alors que la CI était rouge (run CI #1), ce que notre DoD interdit.

### Start / Stop / Continue

Chacun écrit **seul 1 idée par colonne**, puis mise en commun.

| | Start (commencer à faire) | Stop (arrêter de faire) | Continue (continuer à faire) |
|---|---|---|---|
| A | Fusionner une PR seulement quand la CI est verte (la PR #2 a été fusionnée au rouge, run CI #1) | Laisser une US en données factices dans `main` (US-03 : `BacklogMock`) | Écrire les tests avant le code, comme pour US-06 et US-07 (commit rouge 7e9aa0d) |
| B | Noter chaque rotation pilote-copilote dans `docs/journal-binome.md` (le fichier est vide) | Remplir `AI_LOG.md` en fin de séance avec des prompts bruts, sans dire ce qui a été vérifié | Une branche et une PR par US, relue par le copilote avant fusion (PR #15) |

Mise en commun : les deux colonnes Start parlent du même problème, le travail est fait mais pas tracé (CI ignorée, journal vide). Nous retenons donc deux actions de traçabilité.

### Actions pour le sprint suivant (2 maximum)

| # | Action concrète et vérifiable | Comment saura-t-on qu'elle a été faite ? |
|---|---|---|
| 1 | Activer sur GitHub la protection de la branche `main` : fusion impossible tant que le job `test` de la CI n'est pas vert | Dans Settings > Branches, la règle existe ; une PR rouge affiche « Merging is blocked » |
| 2 | Remplacer `BacklogMock` par le chargement du backlog JSON (US-03) avant la fin du sprint 2 | L'issue US-03 est fermée par une PR fusionnée et `CAPI_APP/Mocks/BacklogMock.cs` n'existe plus |

---

## Partie B : rétrospective de fin de projet (3 questions)

### 1. Estimation et réalité (3 US)

| US | Estimation (points) | Temps réel passé | Commentaire |
|---|---|---|---|
| | | | |

Conclusion sur l'estimation en difficulté (2 lignes) :

### 2. Une pratique

Parmi la programmation par paires, les tests écrits avant le code, l'intégration continue et la revue croisée : **laquelle vous a été la plus utile, laquelle vous a le plus gênés ?** Pourquoi, avec un exemple précis de votre projet (3 lignes) :

### 3. L'IA

- Tâches où elle a fait gagner du temps, et tâches où elle en a fait perdre (d'après `AI_LOG.md`) :
- Nombre de propositions rejetées ou corrigées :
- Ce que nous ferions différemment :

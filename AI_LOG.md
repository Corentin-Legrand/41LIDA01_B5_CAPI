# AI_LOG.md : journal d'usage de l'IA

> À placer à la racine du dépôt. À remplir **au fil de l'eau**, pas en fin de projet. Une entrée par tâche (pas par message), en 5 lignes.
> Qui a fait la demande, les fichiers touchés et les commits se retrouvent dans l'historique Git : ne les recopiez pas.

## Outils utilisés (à remplir une fois)

| Outil | Modèle (si connu) | Mode (autocomplétion, chat, agent) |
|---|---|---|
| Claude Code | Claude Sonnet 5.5 | agent, avec validation humaine de chaque écriture de fichier et de chaque commande |
| Claude (claude.ai) | Claude Opus 5.5 | agent |

Fichier d'instructions pour agent versionné (`CLAUDE.md`, `AGENTS.md`...) : non. Nos conventions de code (`CAPI/Conventions de code et bonnes pratiques.md`) sont données à l'agent dans la demande.

## Entrées

### 2026-10-11 : Squelette de l'application, première version (US-01, US-03, US-05)

- **Demande (résumé fidèle)** : produire un squelette qui saisit les joueurs, charge un backlog JSON, fait voter en secret puis révèle les votes, à partir du sujet et de nos user stories.
- **Résultat** : rejeté
- **Ce que nous avons vérifié** : le langage et l'organisation proposés, comparés à nos choix.
- **Ce que nous avons changé ou rejeté, et pourquoi** : la première proposition était en Python alors que notre langage est C# ; la seconde était une application console en C#, alors que nous avions choisi WPF avec le modèle MVVM. Les deux ont été rejetées ; le squelette a été refait par l'entrée suivante.

### 2026-10-12 : Walking skeleton en C# / WPF : joueurs, vote et révélation (US-01, US-05)

- **Demande (résumé fidèle)** : dans nos deux projets (CAPI_APP pour la vue et les view models, CAPI_CORE pour les modèles et la logique), en suivant nos conventions de code, créer les structs Player, Feature et Card, la classe GameSession (Vote, RevealVote) et une fenêtre WPF pour saisir les joueurs puis révéler les cartes, sans prendre d'initiative non demandée.
- **Résultat** : modifié
- **Ce que nous avons vérifié** : relecture des classes par le copilote avant fusion (PR #1) ; application lancée sous Windows : saisie des joueurs, vote, révélation.
- **Ce que nous avons changé ou rejeté, et pourquoi** : nous avons fait renommer CAPI_VIEW en CAPI_APP et CAPI_MODEL en CAPI_CORE et supprimer les fichiers des modèles de projet ; nous avons demandé `Equals` sur Player et Card pour préparer la détection des pseudos en double et la comparaison des votes.

### 2026-10-13 : Projet de tests xUnit (configuration d'outil, US-06, US-07)

- **Demande (résumé fidèle)** : créer le projet CAPI_TESTS (xUnit, .NET 10) qui référence seulement CAPI_CORE, pour tester le moteur sans lancer l'interface.
- **Résultat** : modifié
- **Ce que nous avons vérifié** : `dotnet test` fonctionne sur nos machines.
- **Ce que nous avons changé ou rejeté, et pourquoi** : la commande échouait (erreur NETSDK1045) car une machine n'avait que le SDK .NET 9 ; nous avons installé le SDK .NET 10 au lieu de baisser la version du projet.

### 2026-10-13 : Fichier de CI GitHub Actions (US-06, US-07)

- **Demande (résumé fidèle)** : écrire un workflow qui lance les tests xUnit sur push et sur pull request vers main, avec des droits en lecture seule et des actions épinglées.
- **Résultat** : modifié
- **Ce que nous avons vérifié** : le premier run (CI #1) est rouge avec les 8 tests en échec, puis la CI passe au vert après l'implémentation du moteur (PR #15) ; chaque bloc du YAML est commenté.
- **Ce que nous avons changé ou rejeté, et pourquoi** : la CI tourne sous Linux et ne teste que CAPI_TESTS, car l'application WPF ne se compile que sous Windows ; nous avons retouché les commentaires du YAML pour qu'ils soient écrits avec nos mots.

### 2026-10-14 : Implémentation du moteur de règles (US-06, US-07)

- **Demande (résumé fidèle)** : implémenter les méthodes du moteur de règles (qui levaient toutes une exception) pour que nos tests passent, sans modifier les tests.
- **Résultat** : modifié
- **Ce que nous avons vérifié** : les 8 tests passent en local et dans la CI (PR #15) ; relecture du code par le copilote avant fusion.
- **Ce que nous avons changé ou rejeté, et pourquoi** : avec une liste de votes vide, `All()` renvoie vrai et le moteur aurait déclenché une pause café ; nous avons imposé un revote dans ce cas. La méthode `IsVoteUnanimous` de GameSession, devenue inutile car le moteur s'en charge, a été supprimée.

### 2026-10-14 : Brancher le moteur sur l'interface (US-01, US-05, US-06, US-07)

- **Demande (résumé fidèle)** : relier le moteur au view model et à la vue en gardant le view model le plus léger possible, avec un backlog factice chargé au lancement ; puis un bouton de retour au menu avec confirmation, des messages explicites quand on revote à cause de « ? » ou du café, et le refus d'un pseudo déjà utilisé.
- **Résultat** : accepté tel quel
- **Ce que nous avons vérifié** : partie jouée à la main sous Windows avec les règles stricte et médiane, cartes « ? » et café, pseudo en double.
- **Ce que nous avons changé ou rejeté, et pourquoi** : rien à changer. Le backlog factice (`BacklogMock`) reste temporaire : il sera remplacé par le chargement du fichier JSON (US-03).

### 2026-10-15 : Code de production du sprint 2 : export JSON, carte café, sauvegarde et reprise (US-09, US-10, US-11)

- **Demande (résumé fidèle)** : écrire l'export JSON final (difficulté de chaque fonctionnalité et règle choisie), la sauvegarde de la partie quand tous les joueurs jouent café (joueurs, règle, backlog, estimations, fonctionnalité courante) et la reprise depuis le menu.
- **Résultat** : modifié
- **Ce que nous avons vérifié** : `dotnet test` (16 tests verts) ; essai à la main sous Windows : F-01 validée à 3, café unanime sur F-02, sauvegarde, reprise sur F-02 au tour 1 avec les mêmes joueurs, puis export final où F-01 vaut toujours 3.
- **Ce que nous avons changé ou rejeté, et pourquoi** : l'application ne compilait pas (erreur CS0103 sur `IOException`), car un projet WPF n'importe pas `System.IO` automatiquement ; nous avons ajouté `using System.IO;` dans `RevealViewModel.cs`. Nous avons décidé qu'une partie reprise recommence au tour 1 de la fonctionnalité courante (décision D-05).

## Bilan (à compléter en séance 4, 3 lignes)

- Nombre d'entrées et de propositions rejetées ou corrigées :
- Tâches où l'IA a fait gagner du temps :
- Tâches où l'IA a fait perdre du temps :

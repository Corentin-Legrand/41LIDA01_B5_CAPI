# Definition of Done : binôme B05

Une user story est **terminée** quand **toutes** les conditions suivantes sont vraies. Chaque condition doit pouvoir être vérifiée par quelqu'un d'extérieur au binôme.

| # | Condition | Comment on la vérifie |
|---|---|---|
| 1 | Les critères d'acceptation de l'US sont satisfaits | Un test xUnit par critère, nommé avec son identifiant (`CA_05_2_...`) : `dotnet test` les exécute tous et réussit |
| 2 | Le code est arrivé sur `main` par une PR relue par le copilote | La PR `US-xx : ...` porte un commentaire ou une approbation du copilote, et ferme l'issue de l'US (`Closes #n`), qui passe dans « Terminé » |
| 3 | Les tests passent | La commande de test du README réussit ; la CI est verte (à partir de la séance 2) |
| 4 | Le README est à jour si la façon de lancer ou de tester a changé | Les commandes du README fonctionnent après un `git clone` |
| 5 | Si l'IA a servi, une entrée existe dans `AI_LOG.md` | L'entrée cite l'US |

## Conditions adaptées par rapport au modèle

| Condition | Modèle fourni | Notre version | Pourquoi |
|---|---|---|---|
| 1 | Test par critère « à partir de la séance 2 ; sinon essai à la main » | Test xUnit par critère, sans essai à la main | Un essai à la main ne laisse pas de trace : quelqu'un d'extérieur ne peut pas vérifier qu'il a été fait |
| 2 | « Le code a été relu par le copilote » | Relu **dans une PR qui ferme l'issue** | Nous travaillons avec une branche et une PR par US : la PR qui ferme l'issue tient aussi le tableau à jour |

## Écart assumé au sprint 1

Au sprint 1, les US du walking skeleton (US-01, US-03, US-05) ne respectent pas les conditions 1 et 3 : aucun projet de tests xUnit n'a été créé. Le temps de la séance devait couvrir trois fonctionnalités de bout en bout (configuration, chargement du backlog, vote secret), et nous avons donné la priorité à une version qui se lance. Les autres conditions s'appliquent normalement.

Le projet de tests xUnit sera créé en séance 2, avec les tests du moteur de règles écrits avant le code. La DoD s'appliquera alors en entier.

## Historique des modifications

| Date | Modification | Raison |
|---|---|---|
| 2026-10-09 | Adaptation des conditions 1 et 2 du modèle | Séance 1, bloc B |
| 2026-10-12 | Écart assumé sur les conditions 1 et 3 pour le sprint 1 | Volume du walking skeleton, projet de tests reporté en séance 2 |

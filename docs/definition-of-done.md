# Definition of Done : binôme B05

Une user story est **terminée** quand **toutes** les conditions suivantes sont vraies. Chaque condition doit pouvoir être vérifiée par quelqu'un d'extérieur au binôme.

| # | Condition | Comment on la vérifie |
|---|---|---|
| 1 | Les critères d'acceptation de l'US sont satisfaits | Un test xUnit par critère, nommé avec son identifiant (`CA_05_2_...`) : `dotnet test` les exécute tous et réussit |
| 2 | Le code est arrivé sur `main` par une PR relue par le copilote | La PR `US-xx : ...` porte un commentaire ou une approbation du copilote, et ferme l'issue de l'US (`Closes #n`), qui passe dans « Terminé » |
| 3 | Les tests passent | La commande de test du README réussit ; la CI est verte (à partir de la séance 2) |
| 4 | Le README est à jour si la façon de lancer ou de tester a changé | Les commandes du README fonctionnent après un `git clone` |
| 5 | Si l'IA a servi, une entrée existe dans `AI_LOG.md` | L'entrée cite l'US |

## Ce que nous avons adapté (séance 1)

| Condition | Modèle fourni | Notre version | Pourquoi |
|---|---|---|---|
| 1 | Test par critère « à partir de la séance 2 ; sinon essai à la main » | Test xUnit par critère **dès le sprint 1**, plus d'essai à la main | Nos US du walking skeleton ont déjà leurs tests nommés par CA : un essai à la main ne se vérifie pas de l'extérieur |
| 2 | « Le code a été relu par le copilote » | Relu **dans une PR qui ferme l'issue** | Nous travaillons avec une branche et une PR par US : la PR fermant l'issue tient aussi le tableau à jour sans effort supplémentaire |

## Historique des modifications

| Date | Modification | Raison |
|---|---|---|
| 2026-10-09 | Adaptation des conditions 1 et 2 du modèle | Séance 1, bloc B |

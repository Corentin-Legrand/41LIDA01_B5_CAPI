Liste des prompt a Claude Code (Sonnet 5.5 en mode agent avec validation humaine pour les write et commande shell) :
Ok tu vas m'aider à implémenter une application de planning poker en C# .NET 10 et WPF pour le front. Le projet contient deux projet le projet APP contient le front (Vue) et le VM (Vue Modele) car cela est l'arhtiecture

&#x20; favorisée par .NET et WPF les modèles et la logique de décision métier seront eu dans le projet CORE. Tu trouveras toutes les instruction sur les convention de code dans le fichier "Convention de code et bonnes pratiques



Tu implémentaras uniquement ce que je te dirais de faire et quand je te dirais de la faire je veux garder la main ne prend pas d'initative sans me le demander j'ai accepter le projet de test et le community toolkit car ce

&#x20; sont des choix d'archiutecture que j'avais fais et que je t'aurais demander dans tout les cas. Pour le moment la première tache quie je te donne est de créer les classe modele suivante (elle n'auront pas de comportement

&#x20; donc des strcuts devrait suffire car plus légère) Player (un joueur avec un nom) Feature avec un id un titre et une optionnelle description et Card avec un label (string) et une value (int) nullable.



oui fais le renommage CAPI\_VIEW → CAPI\_APP et CAPI\_MODEL → CAPI\_CORE, supprime les fichiers des templates, et effectivement nous feront plus tard de la gestion d'erreurs sur les pseudos identiques et la valeurs des votes

&#x20; via les cartes intègre leurs donc l'opérateur .Equals



 pour connaitre si le vote est unanime et une méthode RevealVote qui renvoie le dictionnaire des votes si tout les player de la liste sont dans le dictionnaire des votes. Pour le moment c'est tout pour cette classe

&#x20; ensuite tu fera un simple fenêtre WPF qui permet de lancer la partie en demandant de saisir les noms des joueurs jusque a validation et avoir le reveal des cartes. Fait donc les vue associées pour avoir ce simple

&#x20; comportement pour le moment

#PR1
Je veux que tu déplace les commentaires en français du fichier de test unitaire pour les placer dans le readme a la racine du repo, tu peux indiquer pour chaque cas de test à quoi il correspond dans la table de décision    

&#x20; (non disponible dans ce repo pour le moment)


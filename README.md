**TP5 - Paramètres du drone**


Environnement:

Collisions sur le Layer 'Industrial' pour les gros obstacles

Utilisation de SphereCast pour détections d'obstacles fins


Entité suivie:

Distance cible: au plus proche du offset (-0.6, 2.2, 2.5) par rapport au pieds du joueur

Zone d'exlusion de la tête: Interdiction d'approcher par l'avant à courte distance pour ne pas traverser le champ de vision



Paramètres:

32 directions candidates : Bon compromis entre une bonne couverture angulaire et le coût du process

60 degrés du coneAngle : Restreint la recherche aux directions possible derrière et sur les coté du joueur

2.0m + 0.5 x vitesse lookAhead : Anticipation dynamique se projettant plus loin mais pas trop si le joueur avance

0.3m clearanceRadius pour éviter les obstacles fins sans s'éloigner excessivement

5.0m/s maxSpeed pour rattraper le joueur qui à une vitesse max de 3.5m/s

8.0m/s2 maxAccel pour rester fluide tout en offrant une correction rapide

180 degré/s maxTurnRate pour éviter les rotations instantannés

0.40 wSafe : Priorité à la sécurité/collision dans un décor dense

0.30 wFollow : Seconde priorité de suivre le joueur

0.20 wLoS : On veut une bonne visibilité

0.10 wDyn : Pour lisser/amortir les mouvements


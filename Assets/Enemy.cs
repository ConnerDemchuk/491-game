using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity {

    public Enemy(int startingHP, int cardsPerTurn, int energyPerTurn, List<Card> cards) : base(startingHP, cardsPerTurn, energyPerTurn, cards) {
    }
}
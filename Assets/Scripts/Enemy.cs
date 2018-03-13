using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity {

    public Enemy(int startingHP, int cardsPerTurn, int energyPerTurn, List<ICard> cards) : base(startingHP, cardsPerTurn, energyPerTurn, cards) {
    }

    public abstract ICard ChooseCard();
    public abstract void ChooseTarget();

    public override string ToString() {
        return "enemy";
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Enemy {

    private static int startingHP = 16;
    private static int cardsPerTurn = 5;
    private static int energyPerTurn = 3;

    public Pawn() : base(startingHP, cardsPerTurn, energyPerTurn, GetStartingCards()) {
    }

    private static List<ICard> GetStartingCards() {
        return new List<ICard> {
            new Attack(),
            new Attack(),
            new Attack(),
            new Attack(),
            new Attack(),
            new Attack(),
            new Attack(),
            new Attack(),
            new Attack(),
            new Attack()
        };
    }

    public override void TakeTurn() {
        GameState.UnityOutput("The pawn attacks! It's not very effective.");
        GameState.UnityOutput("\n");
    }
}
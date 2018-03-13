using System;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Enemy {

    private static int startingHP = 16;
    private static int cardsPerTurn = 5;
    private static int energyPerTurn = 3;

    private static System.Random rng = new System.Random();

    public Pawn() : base(startingHP, cardsPerTurn, energyPerTurn, GetStartingCards()) {
    }

    private static List<ICard> GetStartingCards() {
        return new List<ICard> {
            new PWN(),
            new Attack(),
            new Attack(),
            new Attack(),
            new Attack()
        };
    }

    public override void ChooseTarget() {
        GameState state = GetState();
        List<Player> players = state.GetPlayers();
        state.SetTarget(players[rng.Next(0, players.Count)]);
    }

    public override ICard ChooseCard() {
        List<ICard> hand = GetHand();
        return hand[rng.Next(0, hand.Count)];
    }
}
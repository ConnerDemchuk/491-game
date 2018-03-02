using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class Player : Entity {

    private int maxHP;
    private int hp;

    private int currentEnergy;

    private List<ICard> classCards;

    public Player(int startingHP, int cardsPerTurn, int energyPerTurn, List<ICard> classCards, List<ICard> startingCards) : base(startingHP, cardsPerTurn, energyPerTurn, startingCards) {
        this.classCards = classCards;
        maxHP = startingHP;
        hp = startingHP;
        currentEnergy = energyPerTurn;
    }
}

public class Wizard : Player {

    private static int startingHP = 100;
    private static int cardsPerTurn = 5;
    private static int energyPerTurn = 3;

    public Wizard() : base(startingHP, cardsPerTurn, energyPerTurn, GetClassCards(), GetStartingCards()) {
    }

    private static List<ICard> GetClassCards() {
        return new List<ICard> {
                new Attack()
            };
    }

    private static List<ICard> GetStartingCards() {
        return new List<ICard> {
                new Attack(),
                new Attack(),
                new Attack(),
                new Attack(),
                new Attack(),
            };
    }
}

/*
override
public void TurnLogic() {
    if (state.GetCurrentEntity() == state.getEntity(0)) {
        Console.WriteLine("Player 1:");
    } else {
        Console.WriteLine("Player 2:");
    }
    List<Card> hand = GetHand();
    string s = "";
    while (hand.Count > 0 && s != " ") {
        int cardNum = 1;
        foreach (Card card in hand) {
            Console.WriteLine(cardNum + ") " + card);
            cardNum++;
        }
        Console.Write("Enter the number of a card to play, or press space to end turn: ");
        String validInput = "^[0-" + GetHand().Count + " ]$";
        Regex r = new Regex(validInput);
        Match m;
        do {
            ConsoleKeyInfo input = Console.ReadKey();
            s = "" + input.KeyChar;
            m = r.Match(s);
        } while (!m.Success);
        Console.WriteLine();
        if (s != " ") {
            hand[Int32.Parse(s)- 1].Run(state);
        }
        Console.WriteLine();
    }
    if (hand.Count == 0) {
        Console.WriteLine("Player hand is empty.");
    }
    Console.WriteLine();
    }
}
*/

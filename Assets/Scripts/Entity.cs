using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class Entity {

    private GameState state;

    private int maxHP;
    private int hp;
    private int cardsPerTurn;
    private int energyPerTurn;
    // If this > 10, update the player class regex
    private int handLimit = 5;
    private int currentEnergy;

    private List<ICard> allCards;
    private List<ICard> deck;
    private List<ICard> hand;
    private List<ICard> discards;
    private List<ICard> trash;

    private bool alive = true;

    public Entity(int startingHP, int cardsPerTurn, int energyPerTurn, List<ICard> startingCards) {
        maxHP = startingHP;
        hp = startingHP;
        this.cardsPerTurn = cardsPerTurn;
        this.energyPerTurn = energyPerTurn;
        currentEnergy = energyPerTurn;
        allCards = new List<ICard>(startingCards);
        deck = new List<ICard>();
        hand = new List<ICard>();
        discards = new List<ICard>();
        trash = new List<ICard>();
    }

    public void SetState(GameState state) {
        this.state = state;
    }

    public GameState GetState() {
        return state;
    }

    public int GetMaxHP() {
        return maxHP;
    }

    public int GetEnergyPerTurn()
    {
        return energyPerTurn;
    }
    public int GetEnergy() {
        return currentEnergy;
    }

    public void LoseEnergy(int n) {
        currentEnergy -= n;
    }

    public int GetHP() {
        return hp;
    }

    public void DealDamage(Entity e, int amount) {
        e.Damage(amount);
    }

    public void Damage(int amount) {
        if (alive) {
            SetHP(hp - amount);
        }
    }

    public void Heal(int amount) {
        if (alive) {
            SetHP(hp + amount);
        }
    }

    public void SetHP(int amount) {
        hp = amount;
        if (hp > maxHP) {
            hp = maxHP;
        }
        if (hp <= 0) {
            hp = 0;
            alive = false;
            state.Kill(this);
        }
    }

    public List<ICard> GetAllCards() {
        return allCards;
    }

    public List<ICard> GetDeck() {
        return deck;
    }

    public List<ICard> GetHand() {
        return hand;
    }

    public List<ICard> GetDiscards() {
        return discards;
    }

    public List<ICard> GetTrash() {
        return trash;
    }

    public bool Discard(ICard card) {
        if (hand.Remove(card)) {
            discards.Add(card);
            return true;
        }
        return false;
    }

    public void DisplayHand() {
        for (int i = 0; i < hand.Count; i++) {
            GameState.UnityOutput(i + 1 + ": " + hand[i].ToString());
        }
    }

    public bool Trash(ICard card) {
        if (hand.Remove(card) || deck.Remove(card)) {
            trash.Add(card);
            return true;
        }
        return false;
    }

    public bool Destroy(ICard card) {
        deck.Remove(card);
        hand.Remove(card);
        discards.Remove(card);
        trash.Remove(card);
        return allCards.Remove(card);
    }

    public bool BeginTurn() {
        currentEnergy = energyPerTurn;
        for (int i = 0; i < cardsPerTurn; i++) {
            if (!DrawCard()) {
                return false;
            }
        }
        return true;
    }

    public void EndTurn() {
        discards.AddRange(hand);
        hand.Clear();
    }

    public void BeginFight() {
        deck.Clear();
        discards.Clear();
        hand.Clear();
        trash.Clear();
        foreach (ICard card in allCards) {
            deck.Add(card);
        }
        Shuffle(deck);
    }

    public bool DrawCard() {
        if (deck.Count == 0) {
            if (discards.Count == 0) {
                return false;
            }
            foreach (SimpleCard card in discards) {
                deck.Add(card);
            }
            discards.Clear();
            Shuffle(deck);
        }
        ICard drawCard = deck[deck.Count - 1];
        deck.RemoveAt(deck.Count - 1);
        if (hand.Count == handLimit) {
            discards.Add(drawCard);
            return false;
        }
        hand.Add(drawCard);
        return true;
    }

    public ICard GetCard(int i) {
        return hand[i];
    }

    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(IList<T> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
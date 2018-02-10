﻿using System;
using System.Collections.Generic;

namespace ConsoleApp1 {

    public abstract class IEntity {

        private int maxHP;
        private int hp;
        private int cardsPerTurn;
        private int energyPerTurn;
        private int handLimit = 10;

        private List<Card> allCards;
        private List<Card> deck;
        private List<Card> hand;
        private List<Card> discards;
        private List<Card> trash;

        public IEntity(int startingHP, int cardsPerTurn, int energyPerTurn, List<Card> startingCards) {
            allCards = new List<Card>(startingCards);
            deck = new List<Card>();
            hand = new List<Card>();
            discards = new List<Card>();
            trash = new List<Card>();
        }

        public int getMaxHP() {
            return maxHP;
        }
        public int GetHP() {
            return hp;
        }
        public void SetHP(int amount) {
            hp = amount;
            if (hp > maxHP) {
                hp = maxHP;
            }
            if (hp <= 0) {
                hp = 0;
            }
        }

        public List<Card> GetAllCards() {
            return new List<Card>(allCards);
        }
        public List<Card> GetDeck() {
            return new List<Card>(deck);
        }
        public List<Card> GetHand() {
            return new List<Card>(hand);
        }
        public List<Card> GetDiscards() {
            return new List<Card>(discards);
        }
        public List<Card> GetTrash() {
            return new List<Card>(trash);
        }

        public bool Discard(Card card) {
            if (hand.Remove(card)) {
                discards.Add(card);
                return true;
            }
            return false;
        }
        public bool Trash(Card card) {
            if (hand.Remove(card) || deck.Remove(card)) {
                trash.Add(card);
                return true;
            }
            return false;
        }
        public bool Destroy(Card card) {
            deck.Remove(card);
            hand.Remove(card);
            discards.Remove(card);
            trash.Remove(card);
            return allCards.Remove(card);
        }
        public bool DrawHand() {
            for (int i = 0; i < cardsPerTurn; i++) {
                if (!DrawCard()) {
                    return false;
                }
            }
            return true;
        }
        public bool DrawCard() {
            if (deck.Count == 0) {
                if (discards.Count == 0) {
                    return false;
                }
                foreach (Card card in discards) {
                    deck.Add(card);
                }
                discards.Clear();
                Shuffle(deck);
            }
            Card drawCard = deck[deck.Count - 1];
            deck.RemoveAt(deck.Count - 1);
            if (hand.Count == handLimit) {
                discards.Add(drawCard);
                return false;
            }
            hand.Add(drawCard);
            return true;
        }

        private static Random rng = new Random();  

        public static void Shuffle<T>(IList<T> list)  
        {  
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
}
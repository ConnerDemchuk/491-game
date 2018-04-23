using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity {

    public List<GameObject> hand;

    public List<CardState> deck;
    public List<CardState> discard;
    public List<CardState> trash;

    public int cardsPerTurn = 5;
    public int handLimit = 5;
    public int energyPerTurn = 4;
    public int currentEnergy = 4;
    public int numOfCards = 5;

    public float r = 4f;
    
    void Awake() {
        allCards = new List<CardState>();

        hand = new List<GameObject>();

        deck = new List<CardState>();
        discard = new List<CardState>();
        trash = new List<CardState>();
        effects = new List<Effect>();


        energyPerTurn = 4;
        currentEnergy = energyPerTurn;

        Task ex = new Damage(10);
        List<Task> li = new List<Task> {
            ex
        };

        for (int i = 0; i < 25; i++)
        {
            deck.Add(new StrikeState());
            deck.Add(new HealState());
            deck.Add(new BetrayState());
            deck.Add(new PoisonState());
        }

        //For testing new cards
        //for (int i = 0; i < 25; i++)
        //{
        //    //deck.Add(new DumpState());
        //    //deck.Add(new BetrayState());
        //    deck.Add(new PoisonState());
        //}
    }

    
    override public void BeginTurn() {
        numOfCards = cardsPerTurn;
        currentEnergy = energyPerTurn;
        for(int i = 0; i < numOfCards; i++) {
          hand.Add(Instantiate(Resources.Load("Card"), new Vector2((float) (r*Math.Sin((Math.PI/180)*i*90/(numOfCards - 1) - (Math.PI/180)*45)), 
          (float) (-6f + r*Math.Cos((Math.PI/180)*i*90/(numOfCards - 1) - (Math.PI/180)*45))), Quaternion.identity) as GameObject);
		  hand[i].GetComponent<Card>().SetState(deck[0]);
          deck.Remove(deck[0]);
		  Debug.Log("Made Card: " + i);
        }
        gameObject.tag = "CurrentEntity";
        ApplyEffects();
    }

    public void OrganizeCards(){
        numOfCards = hand.Count;
        for(int i = 0; i < numOfCards; i++){
         //   hand[i].transform.position = new Vector3((float) (r*Math.Sin((Math.PI/180)*i*(numOfCards*14))/(numOfCards - 1) - (Math.PI/180)*45),
       //     (float) (-6f + r*Math.Cos((Math.PI/180)*i*(numOfCadrs*14))/(numOfCards - 1) - (Math.PI/180)*45)), 0;
        //     hand[i].transform.position = new Vector3((float) (r*Math.Sin((Math.PI/180)*i*(numOfCards*14))/(numOfCards - 1) - (Math.PI/180)*45)),
        //   (float) (-6f + r*Math.Cos((Math.PI/180)*i*(numOfCadrs*14))/(numOfCards - 1) - (Math.PI/180)*45)), 0);
        }
        //r =  4 - (4 - numOfCards)*.35f;
    }


    override public void EndTurn() {
        for(int i = 0; i < hand.Count; i++){
            discard.Add(hand[i].GetComponent<Card>().GetState());
            Destroy(hand[i]);
            hand.Remove(hand[i]);
        }
        gameObject.tag = "Player";
    }

    public void ActivateCard(int i) {
        hand[i].GetComponent<Card>().ActivateCard();
    }

    public void HighlightCard(int i) {
        hand[i].GetComponent<Card>().Highlight();
    }

    public void UnHighlightCard(int i) {
        hand[i].GetComponent<Card>().UnHighlight();
    }

    public bool NoCardHighlighted() {
        for(int i = 0; i < hand.Count; i++){
            if(hand[i].GetComponent<Card>().IsHighlighted()){
                return false;
            }
        }
        return true;
    }

    public void UnSelectAll(){
        for(int i = 0; i < hand.Count; i++) {
            hand[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/FistStatic");
        }
    }

    public void HighlightCardSelect(int i) {
        hand[i].GetComponent<Card>().HighlightSelect();
    }

    public GameObject GetCard(int i){
        return hand[i];
    }

    public List<CardState> GetDiscard(){
        return discard;
    }

    public void LoseEnergy(int n) {
        currentEnergy -= n;
    }

    public bool CardIsHighlighted(int i) {
        return hand[i].GetComponent<Card>().IsHighlighted();
    }

    public bool HasCards() {
        return hand.Count != 0;
    }

    public int GetHandSize() {
        return hand.Count;
    }

    public override List<CardState> GetDeck()
    {
        return deck;
    }

    public override void SetDeck(List<CardState> deck)
    {
        this.deck = deck;
    }


}
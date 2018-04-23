using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Enemy : Entity {

    private System.Random r;
    private List<GameObject> players;
    private List<CardState> hand;
    private int count;

    void Awake(){
        r = new System.Random();
        hand = new List<CardState>();
        effects = new List<Effect>();


        Task ex = new Damage(10);
        List<Task> li = new List<Task>();

        li.Add(ex);

        for(int i = 0; i < 3; i++){
            hand.Add(new StrikeState());
        }
    }

    override public void BeginTurn(){
        ApplyEffects();
        gameObject.tag = "CurrentEntity";
    }

    public override List<CardState> GetDeck()
    {
        return hand;
    }

    public override void SetDeck(List<CardState> deck)
    {
        hand = deck;
    }

    public void ActivateEffect(){

        List<GameObject> entities = GameController.GetGameController().entities;
        players = new List<GameObject>();
        foreach(GameObject o in entities){
            if(o.GetComponent<Entity>() is Player) players.Add(o);
        }

        CardState c = hand[r.Next(hand.Count)];
        foreach (Task task in c.getTasks()) {
            task.Run(players[r.Next(players.Count)]);
        }

        gameObject.tag = "Enemy";
    }

}
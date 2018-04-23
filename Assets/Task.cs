using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task {

    public enum Input {
        Card,
        Entity,
        Enemy,
        Player,
        DiscardCard,
        Null
    };

    public Input type;
    public virtual void Run(GameObject input) {}
}


public class Damage : Task {

    int damage;

    public Damage(int damage) {
        this.damage = damage;
        this.type = Input.Entity;
    }

    override public void Run(GameObject input){
        Entity p = input.GetComponent<Entity>();
        p.Damage(damage);
    }
}

public class Poison : Task
{

    int damage;
    int turnNum;

    public Poison(int damage, int turnNum)
    {
        this.damage = damage;
        this.turnNum = turnNum;
        this.type = Input.Entity;
    }

    override public void Run(GameObject input)
    {
        Debug.Log("we ran");
        Entity p = input.GetComponent<Entity>();
        p.AddEffect(new Effect(damage, turnNum, false));
    }
}

public class DebugTask : Task {
    private int i;
    override public void Run(GameObject input){
        Debug.Log("Task " + i + " Run!");
    }

    public DebugTask(int i){
        this.type = Input.Null;
        this.i = i;
    }

}

public class Example : Task {

    public Example() {
        this.type = Input.Null;
    }

    override public void Run(GameObject input) {
        Debug.Log("Task Run!");
    }
}

public class Heal : Task
{
    int damage = 0;

    public Heal(int damage) {
        this.damage = damage;
        this.type = Input.Entity;
    }

    override public void Run(GameObject input) {
        Entity p = input.GetComponent<Entity>();
        p.Heal(damage);
    }
}

public class InsertCard : Task
{
    CardState card;

    public InsertCard(CardState card)
    {
        this.type = Input.Entity;
        this.card = card;
    }

    override public void Run(GameObject input)
    {
        Entity p = input.GetComponent<Entity>();
        List<CardState> newDeck = p.GetDeck();
        newDeck.Add(card);
        p.SetDeck(newDeck);
    }
}

public class Betray : Task
{
    int damage = 0;
    int heal = 0;

    public Betray(int damage, int heal)
    {
        this.damage = damage;
        this.type = Input.Entity;
    }

    override public void Run(GameObject input)
    {
        Entity p = input.GetComponent<Entity>();
        GameController gc = GameController.GetGameController();
        Entity currentPlayer = gc.currentPlayer;

        //Used to check for p1 and p2, then creates energy for the gui
        if (currentPlayer.Equals(gc.entities[0].GetComponent<Entity>()))
        {
            //is p1
            gc.entities[0].GetComponent<Entity>().Heal(30);
            gc.entities[1].GetComponent<Entity>().Damage(15);
        }
        else
        {
            //is p2
            gc.entities[0].GetComponent<Entity>().Damage(10);
            gc.entities[1].GetComponent<Entity>().Heal(30);
        }
    }
}
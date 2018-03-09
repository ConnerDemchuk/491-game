using System;
using System.Collections.Generic;
using UnityEngine;

public class GameState {

    private GameScript script;

    private int numPlayers = 0;
    private List<Entity> entities = new List<Entity>();
    private List<Player> players = new List<Player>();
    private List<Entity> enemies = new List<Entity>();

    private Entity currentEntity;
    private Entity currentTarget;

    private Stack<ICard> cardStack = new Stack<ICard>();

    public GameState(GameScript script) {
        this.script = script;
    }

    public void SetState(GameScript.BattleState state) {
        script.SetState(state);
    }

    public void AddEntity(Entity e) {
        entities.Add(e);
        if (e is Player) {
            players.Add((Player)e);
        } else {
            enemies.Add(e);
        }
        e.SetState(this);
    }

    private static int msgNumber = 0;

    public static void UnityOutput(string tag, string msg) {
        Debug.Log(tag + ": " + msg);
        msgNumber++;
    }

    public static void UnityOutput(object msg) {
        UnityOutput(String.Format("{0}", msgNumber), msg.ToString());
    }


    public void BeginFight() {
        foreach (Entity entity in entities) {
            entity.BeginFight();
        }
        NextTurn();
    }

    public void BeginTurn() {
        currentEntity.BeginTurn();
    }

    public void SetTarget(Entity target) {
        currentTarget = target;
    }

    public Entity GetTarget() {
        return currentTarget;
    }

    public void CardStart(ICard card) {
        cardStack.Push(card);
    }

    public void CardEnd() {
        cardStack.Pop();
    }

    public ICard GetCurrentCard() {
        if (cardStack.Count == 0) {
            return null;
        }
        return cardStack.Peek();
    }

    public void EndTurn() {
        currentEntity.EndTurn();
    }

    public int NumPlayers() {
        return numPlayers;
    }

    public List<Player> GetPlayers() {
        return players;
    }

    public List<Entity> GetEntities() {
        return entities;
    }
    
    public List<Entity> GetEnemies() {
        return enemies;
    }

    public Entity GetEntity(int i) {
        return entities[i];
    }

    public Entity GetCurrentEntity() {
        return currentEntity;
    }

    public void SetCurrentEntity(Entity i) {
        currentEntity = i;
    }

    public void DisplayEntities() {
        for (int i = 0; i < entities.Count; i++) {
            UnityOutput(i + 1 + ": " + entities[i]);
        }
    }

    public void DisplayEntitiesHP() {
        for (int i = 0; i < entities.Count; i++) {
            UnityOutput(entities[i] + ": " + entities[i].GetHP());
        }
    }

    public void Kill(Entity i) {
        if (currentEntity == i) {
            NextTurn();
        }
        entities.Remove(i);
    }

    public void Heal(Entity e, int amount) {
        e.SetHP(e.GetHP() + amount);
    }

    public void Damage(Entity e, int amount) {
        Heal(e, -amount);
    }

    public Entity NextTurn() {
        if (currentEntity == null) {
            currentEntity = entities[0];
        } else {
            int turnIndex = entities.IndexOf(currentEntity) + 1;
            if (turnIndex >= entities.Count) {
                turnIndex = 0;
            }
            currentEntity = entities[turnIndex];
        }
        return currentEntity;
    }
}
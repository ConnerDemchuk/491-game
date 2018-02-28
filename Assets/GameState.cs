using System;
using System.Collections.Generic;
using UnityEngine;

public class GameState {

    private int numPlayers = 0;
    List<Entity> entities = new List<Entity>();
    List<Player> players = new List<Player>();
    List<Entity> enemies = new List<Entity>();
    Entity currentEntity;

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

    public static void UnityOutput(string msg) {
        UnityOutput(String.Format("{0}", msgNumber), msg);
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

    public void EndTurn() {
        currentEntity.EndTurn();
    }

    public int NumPlayers() {
        return numPlayers;
    }

    public List<Player> GetPlayers() {
        return new List<Player>(players);
    }

    public List<Entity> GetEntities() {
        return new List<Entity>(entities);
    }
    
    public List<Entity> GetEnemies() {
        return new List<Entity>(enemies);
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

    public void TestEffect() {
        UnityOutput("You attack!  It's not very effective.");
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
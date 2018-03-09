using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICard {
    /*
    public enum Rarity {
        Common = 0,
        Uncommon = 1,
        Rare = 2
    }
    */
    int GetCost();
    bool Run(GameState state);
    string ToString();
}

public abstract class SimpleCard : ICard {

    private string name;
    private int cost;

    private List<ITask> tasks;

    public SimpleCard(string name, int cost, List<ITask> tasks) {
        this.name = name;
        this.cost = cost;
        this.tasks = tasks;
    }

    public int GetCost() {
        return cost;
    }

    int currentTaskNum = 0;
    public bool Run(GameState state) {
        if (currentTaskNum == 0) {
            // Begin Card
            state.GetCurrentEntity().LoseEnergy(cost);
            state.CardStart(this);
        }
        bool pauseExecution = false;
        while (currentTaskNum < tasks.Count && !pauseExecution) {
            // If the task returns false, pause execution for input
            if (tasks[currentTaskNum].Run(state)) {
                pauseExecution = true;
            }
            currentTaskNum++;
        }
        if (currentTaskNum < tasks.Count) {
            // The card needs to continue execution later
            return false;
        } else {
            // End card
            currentTaskNum = 0;
            state.GetCurrentEntity().Discard(this);
            state.CardEnd();
            return true;
        }
    }

    override
    public string ToString() {
        return name;
    }
}

public class Attack : SimpleCard {
    private static string name = "Attack";
    private static int cost = 1;
    private static List<ITask> taskList = new List<ITask> {
            //new ExampleTask()
            new SelectEnemy(),
            new DamageSelectedEntity(10)
        };

    public Attack() : base(name, cost, taskList) {
    }
}

public class PWN : SimpleCard {
    private static string name = "PWN";
    private static int cost = 1;
    private static List<ITask> taskList = new List<ITask> {
            //new ExampleTask()
            new SelectEnemy(),
            new DamageSelectedEntity(100)
        };

    public PWN() : base(name, cost, taskList) {
    }
}

public class EndTurn : SimpleCard {
    private static string name = "Skip Turn";
    private static int cost = 0;
    private static List<ITask> taskList = new List<ITask> {
            //new ExampleTask()
            new LoseAllEnergy()
        };

    public EndTurn() : base(name, cost, taskList) {
    }
}

/*
public void QueueEnemySelect() {
    script.SetState(GameScript.BattleState.chooseEnemy);
}

public void QueueTargetSelect() {
    script.SetState(GameScript.BattleState.chooseTarget);
}

public void QueueNonSelfTargetSelect() {
    script.SetState(GameScript.BattleState.chooseNonSelfTarget);
}

public void QueueAllySelect() {
    script.SetState(GameScript.BattleState.chooseAlly);
}
*/

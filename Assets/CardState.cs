using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardState {

    private string name;
    private int cost;
    private List<Task> tasks;
    private AudioClip soundEffect;
    public Sprite icon;
    private string iconString;

    public CardState(string name, int cost, List<Task> tasks, string audioPath, string spritePath){
        this.name = name;
        this.cost = cost;
        this.tasks = tasks;
        soundEffect = Resources.Load<AudioClip>(audioPath);
        iconString = spritePath;
        icon = Resources.Load<Sprite>(spritePath);
    }

    public List<Task> getTasks() {
        return tasks;
    }

    public int GetCost() {
        return cost;
    }

    public void RunTask(GameObject input, int i){
        tasks[i].Run(input);
    }

    override
    public string ToString() {
        return name;
    }

    public AudioClip GetSoundEffect()
    {
        return soundEffect;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public string GetIconString()
    {
        return "Sprites/FistStatic";
    }

    public virtual IEnumerator Coroutine(GameObject g){
        yield return null;
    }
}

public class StrikeState : CardState {
    private static string NAME = "Strike";
    private static int COST = 1;

    public StrikeState() : base(NAME, COST, GetTasks(), "Sounds/PunchSound", "Sprites/FistStatic") {
    }

    private static List<Task> GetTasks() {
        return new List<Task> {
            new Damage(10)
        };
    }
}

public class HealState : CardState
{
    private static string NAME = "Heal";
    private static int COST = 1;

    public HealState() : base(NAME, COST, GetTasks(), "Sounds/PunchSound", "Sprites/Shield")
    {
    }

    private static List<Task> GetTasks()
    {
        return new List<Task> {
            new Heal(10)
        };
    }
}

public class TrashState : CardState
{
    private static string NAME = "Trash";
    private static int COST = 1;

    public TrashState() : base(NAME, COST, GetTasks(), "Sounds/PunchSound", "Sprites/Skull")
    {
    }

    private static List<Task> GetTasks()
    {
        return new List<Task> {
            new Example()
        };
    }
}

public class DumpState : CardState
{
    private static string NAME = "Trash Dump";
    private static int COST = 3;

    public DumpState() : base(NAME, COST, GetTasks(), "Sounds/PunchSound", "Sprites/Heart")
    {
    }

    private static List<Task> GetTasks()
    {
        return new List<Task> {
            new InsertCard(new StrikeState())
        };
    }
}

public class BetrayState : CardState
{
    private static string NAME = "Betrayal";
    private static int COST = 2;

    public BetrayState() : base(NAME, COST, GetTasks(), "Sounds/PunchSound", "Sprites/Heart")
    {
    }

    private static List<Task> GetTasks()
    {
        return new List<Task> {
           new Betray(15, 30)
        };
    }
}

public class PoisonState : CardState
{
    private static string NAME = "Betrayal";
    private static int COST = 3;

    public PoisonState() : base(NAME, COST, GetTasks(), "Sounds/PunchSound", "Sprites/Skull")
    {
    }

    private static List<Task> GetTasks()
    {
        return new List<Task> {
           new Poison(5, 3)
        };
    }
}
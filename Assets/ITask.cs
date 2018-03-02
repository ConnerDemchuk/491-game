using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITask {
    bool Run(GameState state, ICard cardPlayed);
}

public class ExampleTask : ITask {
    public bool Run(GameState state, ICard cardPlayed) {
        state.TestEffect();
        // Don't pause execution after returning
        return false;
    }
}

public class SelectEnemy : ITask {
    public bool Run(GameState state, ICard cardPlayed) {
        state.SetState(GameScript.BattleState.chooseEnemy);
        // Pause execution after returning
        return true;
    }
}
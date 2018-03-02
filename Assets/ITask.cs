using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITask {
    bool Run(GameState state, ICard cardPlayed);
}

public class ExampleTask : ITask {
    public bool Run(GameState state, ICard cardPlayed) {
        state.TestEffect();
        return true;
    }
}

public class SelectEnemy : ITask {
    public bool Run(GameState state, ICard cardPlayed) {
        state.SetState(GameScript.BattleState.chooseEnemy);
        return false;
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITask {
    // true = pause
    // false = no pause
    bool Run(GameState state);
}

public class SelectEnemy : ITask {
    public bool Run(GameState state) {
        state.SetState(GameScript.BattleState.chooseTarget);
        return true;
    }
}

public class SaySelectedEntity : ITask {
    public bool Run(GameState state) {
        GameState.UnityOutput(state.GetTarget());
        return false;
    }
}

public class DamageSelectedEntity : ITask {
    private int n;

    public DamageSelectedEntity(int amount) {
        n = amount;
    }

    public bool Run(GameState state) {
        state.GetCurrentEntity().DealDamage(state.GetTarget(), n);
        return false;
    }
}

public class LoseAllEnergy : ITask {

    public bool Run(GameState state) {
        Entity e = state.GetCurrentEntity();
        e.LoseEnergy(e.GetEnergy());
        return false;
    }
}
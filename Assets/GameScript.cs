using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {

    public enum BattleState {
        beginTurn,
        choosePlayCard,
        chooseTarget,
        chooseNonSelfTarget,
        chooseEnemy,
        chooseAlly,
        endTurn,
        endFight
    };
    BattleState curState;
    Entity curTurn;
    ICard curCard;

    GameState state;
    public void SetState(BattleState state) {
        curState = state;
    }

    // Use this for initialization
    void Start() {

        state = new GameState(this);

        Player p1 = new Wizard();
        Player p2 = new Wizard();
        Entity enemy1 = new Pawn();
        
        state.AddEntity(p1);
        state.AddEntity(p2);
        state.AddEntity(enemy1);

        state.BeginFight();

        curTurn = state.GetCurrentEntity();
        curState = BattleState.beginTurn;
    }

    private static int getKeyNum() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) return 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) return 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) return 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) return 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) return 4;
        return -1;
    }

    // Update is called once per frame
    void Update() {
        int keyNum;
        switch (curState) {
            case BattleState.beginTurn:
                GameState.UnityOutput("Turn Begins");
                state.BeginTurn();
                curTurn.DisplayHand();
                curState = BattleState.choosePlayCard;
                break;
            case BattleState.choosePlayCard:
                // if a card a picked, do use effect (discard, trash, ect.)
                // activate the card / pick its target
                // then stay in the same state
                if (curTurn is Player) {
                    keyNum = getKeyNum();
                    if (keyNum != -1) {
                        curCard = curTurn.GetCard(keyNum);
                        curCard.Run(state);
                        GameState.UnityOutput("You now have " + curTurn.GetEnergy() + " energy!");
                    }
                    if (curTurn.GetEnergy() <= 0) {
                        curState = BattleState.endTurn;
                    }
                } else if (curTurn is Enemy) {
                    ((Enemy)curTurn).TakeTurn();
                    curState = BattleState.endTurn;
                }
                break;
            case BattleState.endTurn:
                curTurn.EndTurn();
                GameState.UnityOutput("Your turn is done!");
                curTurn = state.NextTurn();
                curState = BattleState.beginTurn;
                break;
            case BattleState.chooseTarget:
                state.DisplayEntities();
                keyNum = getKeyNum();
                if (keyNum != -1) {
                    state.SetTarget(state.GetEntity(keyNum));
                    curState = BattleState.choosePlayCard;
                }
                curCard.Run(state);
                break;
        }
    }
}

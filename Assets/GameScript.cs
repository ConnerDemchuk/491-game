using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {

    public enum BattleState {
        beginTurn,
        choosePlayCard,
        chooseTarget,
        chooseTarget2,
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

    private static int GetKeyNum() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) return 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) return 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) return 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) return 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) return 4;
        return -1;
    }

    int currentHandSize;
    int lastHandSize;

    // Update is called once per frame
    void Update() {
        int keyNum;
        string turnType;
        switch (curState) {
            case BattleState.beginTurn:
                if (curTurn is Player) {
                    turnType = "Player";
                } else {
                    turnType = "Enemy";
                }
                GameState.UnityOutput("----------------------------");
                GameState.UnityOutput(turnType + " turn begins");
                state.BeginTurn();
                curState = BattleState.choosePlayCard;
                currentHandSize = -1;
                lastHandSize = -2;
                break;
            case BattleState.choosePlayCard:
                // if a card a picked, do use effect (discard, trash, ect.)
                // activate the card / pick its target
                // then stay in the same state
                if (curTurn.GetEnergy() <= 0) {
                    curState = BattleState.endTurn;
                } else {
                    if (curTurn is Player) {
                        currentHandSize = ((Player)curTurn).GetHand().Count;
                        if (currentHandSize != lastHandSize) {
                            GameState.UnityOutput("----------------------------");
                            state.DisplayEntitiesHP();
                            GameState.UnityOutput("----------------------------");
                            curTurn.DisplayHand();
                            GameState.UnityOutput("----------------------------");
                            lastHandSize = currentHandSize;
                        }
                        keyNum = GetKeyNum();
                        if (keyNum != -1) {
                            curCard = curTurn.GetCard(keyNum);
                            curCard.Run(state);
                            GameState.UnityOutput("You now have " + curTurn.GetEnergy() + " energy!");
                        }
                    } else if (curTurn is Enemy) {
                        curTurn.DisplayHand();
                        curCard = ((Enemy)curTurn).ChooseCard();
                        GameState.UnityOutput("Taking enemy turn...");
                        curCard.Run(state);
                    }
                }
                break;
            case BattleState.endTurn:
                curTurn.EndTurn();
                if (curTurn is Player) {
                    turnType = "Player";
                } else {
                    turnType = "Enemy";
                }
                GameState.UnityOutput("----------------------------");
                GameState.UnityOutput(turnType + " turn ends");
                curTurn = state.NextTurn();
                curState = BattleState.beginTurn;
                break;
            case BattleState.chooseTarget:
                if (curTurn is Enemy) {
                    ((Enemy)curTurn).ChooseTarget();
                    GameState.UnityOutput("Enemy using " + curCard + " on " + state.GetTarget());
                    curState = BattleState.choosePlayCard;
                    curCard.Run(state);
                } else {
                    state.DisplayEntities();
                    curState = BattleState.chooseTarget2;
                }
                break;
            case BattleState.chooseTarget2:
                keyNum = GetKeyNum();
                if (keyNum != -1) {
                    state.SetTarget(state.GetEntity(keyNum));
                    curState = BattleState.choosePlayCard;
                    curCard.Run(state);
                }
                break;
        }
    }
}

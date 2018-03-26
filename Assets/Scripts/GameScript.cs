using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //variables used for text and graphics
    public Text enemyHealthText;
    public Text player1InfoText;
    public Text player2InfoText;
    public Text targetSelectPrompt;
    public ArrayList prefabNames;
    public ArrayList gameObjects;
    public ArrayList cards1;
    public ArrayList cards2;
    public List<Text> cardtexts1;
    public List<Text> cardtexts2;
    public GameObject magicianPrefab;
    public GameObject snakePrefab;
    public GameObject cardPrefab;
    //texts used for card text
    public Text card1_0;
    public Text card1_1;
    public Text card1_2;
    public Text card1_3;
    public Text card1_4;
    public Text card2_0;
    public Text card2_1;
    public Text card2_2;
    public Text card2_3;
    public Text card2_4;

    GameState state;
    public void SetState(BattleState state) {
        curState = state;
    }

    //Used to create all cards for each player respectively
    public void CreateCards1(List<ICard> effects)
    {
        for (int k = 0; k < effects.Count; k++)
        {
            cards1.Add(Instantiate(cardPrefab, new Vector2(-7 + (k * 2), 3), Quaternion.identity));
            GameState.UnityOutput(effects.Count);
            GameState.UnityOutput(effects[k].ToString());
            cardtexts1[k].text = "Name: " + effects[k].ToString() + " Cost: " + effects[k].GetCost();
        }
    }
    public void CreateCards2(List<ICard> effects)
    {
        for (int k = 0; k < effects.Count; k++)
        {
            cards2.Add(Instantiate(cardPrefab, new Vector2(-7 + (k * 2), 0), Quaternion.identity));
            cardtexts2[k].text = "Name: " + effects[k].ToString() + " Cost: " + effects[k].GetCost();
        }
    }

    public void DeleteAllCards()
    {
        for (int k = 0; k < cards1.Count; k++)
        {
            Destroy((GameObject)cards1[k]);
        }
        cards1.Clear();
        for (int k = 0; k < cards2.Count; k++)
        {
            Destroy((GameObject)cards2[k]);
        }
        cards2.Clear();
        for (int k = 0; k < 5; k++)
        {
            cardtexts1[k].text = " ";
        }
        for (int k = 0; k < 5; k++)
        {
            cardtexts2[k].text = " ";
        }

    }

    public void DeleteCardGraphic1(int i, List<ICard> effects)
    {
        //Update Card prefab
        int newLength = cards1.Count - 1;
        for (int k = 0; k < cards1.Count; k++)
        {
            Destroy((GameObject)cards1[k]);
        }
        cards1.Clear();
        for (int k = 0; k < newLength; k++)
        {
            cards1.Add(Instantiate(cardPrefab, new Vector2(-7 + (k * 2), 3), Quaternion.identity));
        }

        //Update card text
        for (int k = 0; k < 5; k++)
        {
            cardtexts1[k].text = " ";
        }
        List<ICard> temp = new List<ICard>();
        foreach (ICard card in effects)
        {
            temp.Add(card);
        }
        temp.RemoveAt(i);
        for (int k = 0; k < temp.Count; k++)
        {
            cardtexts1[k].text = "Name: " + temp[k].ToString() + " Cost: " + temp[k].GetCost();
        }
    }

    public void DeleteCardGraphic2(int i, List<ICard> effects)
    {
        int newLength = cards2.Count - 1;
        for (int k = 0; k < cards2.Count; k++)
        {
            Destroy((GameObject)cards2[k]);
        }
        cards2.Clear();
        for (int k = 0; k < newLength; k++)
        {
            cards2.Add(Instantiate(cardPrefab, new Vector2(-7 + (k * 2), 0), Quaternion.identity));
        }
        //Update card text
        for (int k = 0; k < 5; k++)
        {
            cardtexts2[k].text = " ";
        }
        List<ICard> temp = new List<ICard>();
        foreach (ICard card in effects)
        {
            temp.Add(card);
        }
        temp.RemoveAt(i);
        for (int k = 0; k < temp.Count; k++)
        {
            cardtexts2[k].text = "Name: " + temp[k].ToString() + " Cost: " + temp[k].GetCost();
        }

    }

    //public void AddCardText(List<ICard> cardEffects)
    //{
    //    cardtexts[k].text = cardEffects;
    //}

    // Use this for initialization
    void Start() {

        state = new GameState(this);

        Player p1 = new Wizard();
        Player p2 = new Wizard();
        Entity enemy1 = new Pawn();

        //TEST CODE BLOCK STARTS HERE ________________________________________
        //Use two arraylists to keep track of which gameobject needs to be used for the current enemy
        prefabNames = new ArrayList();
        gameObjects = new ArrayList();
        cards1 = new ArrayList();
        cards2 = new ArrayList();
        prefabNames.Add(enemy1.ToString());
        gameObjects.Add(magicianPrefab);
        prefabNames.Add("snake");
        gameObjects.Add(snakePrefab);

        int i = prefabNames.IndexOf(enemy1.ToString());
        //int i = getIndexOfGameObject("snake");
        GameObject pawn = Instantiate((GameObject)gameObjects[i], new Vector2(6, -1), Quaternion.identity);

        //create a list of card text objects for easier manipulation later
        cardtexts1 = new List<Text>();
        cardtexts1.Add(card1_0);
        cardtexts1.Add(card1_1);
        cardtexts1.Add(card1_2);
        cardtexts1.Add(card1_3);
        cardtexts1.Add(card1_4);

        cardtexts2 = new List<Text>();
        cardtexts2.Add(card2_0);
        cardtexts2.Add(card2_1);
        cardtexts2.Add(card2_2);
        cardtexts2.Add(card2_3);
        cardtexts2.Add(card2_4);

        //TEST CODE BLOCK ENDS HERE __________________________________________


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

        //Update Information on health and energy
        //Reset both players' energy to full at the start of p1's turn
        if (state.GetCurrentEntity().Equals(state.GetEntity(0)))
        {
            enemyHealthText.text = "Enemy Health: " + state.GetEntity(2).GetHP();
            player1InfoText.text = "Player 1 Health: " + state.GetEntity(0).GetHP() + " Energy: " + state.GetEntity(0).GetEnergy();
            player2InfoText.text = "Player 2 Health: " + state.GetEntity(1).GetHP() + " Energy: " + state.GetEntity(1).GetEnergyPerTurn();
        }
        else
        {

            enemyHealthText.text = "Enemy Health: " + state.GetEntity(2).GetHP();
            player1InfoText.text = "Player 1 Health: " + state.GetEntity(0).GetHP() + " Energy: " + state.GetEntity(0).GetEnergy();
            player2InfoText.text = "Player 2 Health: " + state.GetEntity(1).GetHP() + " Energy: " + state.GetEntity(1).GetEnergy();
        }
        switch (curState) {
            case BattleState.beginTurn:
                if (curTurn is Player) {
                    turnType = "Player";
                }
                else {
                    turnType = "Enemy";
                }
                GameState.UnityOutput("----------------------------");
                GameState.UnityOutput(turnType + " turn begins");
                state.BeginTurn();
                //generate player cards
                DeleteAllCards();
                if (state.GetCurrentEntity().Equals(state.GetEntity(0)))
                {
                    CreateCards1(state.GetCurrentEntity().GetHand());
                }
                else if(state.GetCurrentEntity().Equals(state.GetEntity(1)))
                {
                    CreateCards2(state.GetCurrentEntity().GetHand());
                }
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
                        targetSelectPrompt.text = " ";
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
                            //CODE TO UPDATE CARDS ON-SCREEN ------------------------
                            //Update how many cards are on screen based on the player currently active
                            if (state.GetCurrentEntity().Equals(state.GetEntity(0)))
                            {
                                DeleteCardGraphic1(keyNum, state.GetCurrentEntity().GetHand());
                            }
                            else
                            {
                                DeleteCardGraphic2(keyNum, state.GetCurrentEntity().GetHand());
                            }
                            //END CODE TO GENERATE CARDS ON-SCREEN --------------------
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
                    targetSelectPrompt.text = "Select a Target:\n 1: Player 1\n 2: Player 2\n 3: Enemy";
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

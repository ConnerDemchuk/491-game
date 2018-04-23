using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public Entity currentEntity;
	public Player currentPlayer;
	public Enemy currentEnemy;

	public List<GameObject> entities;
	public GameObject input;

    public CardPile cardPile;

    //used for health and energy graphics
    public Slider p1HealthBar;
    public Slider p2HealthBar;
    public Slider e1HealthBar;
    public Slider e2HealthBar;
    public List<Slider> healthBars;

    public Text p1HealthText;
    public Text p2HealthText;
    public Text e1HealthText;
    public Text e2HealthText;
    public List<Text> healthTexts;

    public GameObject energy;
    public List<GameObject> p1Energy;
    public List<GameObject> p2Energy;

    public bool canCheckForEndOfTurn = true;
    private int numCardsHighlighted = 0;

	int entityIndex = 0;

	int numObjects;
	int numIndex;

    private const KeyCode unusedKey = KeyCode.A;
	public KeyCode prevCode = unusedKey;

	public enum SelectionType {
		SelectCardToPlay,
		SelectCard,
        CardPileSelect,
		SelectEntity
	};

	public SelectionType selectionType;

	void Awake () {

        selectionType = SelectionType.SelectCardToPlay;
        entities = new List<GameObject> {
            Instantiate(Resources.Load("Player"), new Vector2(-3, 0), Quaternion.identity) as GameObject
        };
        entities[0].tag = "CurrentEntity";

		entities.Add(Instantiate(Resources.Load("Player"), new Vector2(-5, 0), Quaternion.identity) as GameObject);
		entities.Add(Instantiate(Resources.Load("EnemyMagician"), new Vector2(5, 0), Quaternion.identity) as GameObject);
		entities.Add(Instantiate(Resources.Load("EnemyMagician"), new Vector2(3, 0), Quaternion.identity) as GameObject);

	}

	// Use this for initialization
	void Start () {
        healthBars = new List<Slider> {
            p1HealthBar,
            p2HealthBar,
            e1HealthBar,
            e2HealthBar
        };

        healthTexts = new List<Text> {
            p1HealthText,
            p2HealthText,
            e1HealthText,
            e2HealthText
        };

        entityIndex = 0;
		currentEntity = entities[entityIndex].GetComponent<Entity>();
		currentEntity.BeginTurn();
	}

	// Update is called once per frame
	void Update () {
        //update every entity's health bar

        int numPlayers = 0;
        int numEnemies = 0;
        for (int i = 0; i < 4; i++) {
            string message;
            Entity e = entities[i].GetComponent<Entity>();
            if (e != null) {
                if (e is Player) {
                    numPlayers++;
                    message = "Player " + numPlayers + ": ";
                } else {
                    numEnemies++;
                    message = "Enemy " + numEnemies + ": ";
                }
                message += e.GetHP() + "/" + e.GetMaxHP();
                healthTexts[i].text = message;
                healthBars[i].value = (float)e.GetHP() / e.GetMaxHP();
            }
        }

        DeleteAllEnergy();

        if (currentEntity is Player) {
			currentPlayer = (Player) currentEntity;

            //Used to check for p1 and p2, then creates energy for the gui
            if(currentEntity.Equals(entities[0].GetComponent<Entity>()))
            {
                //is p1
                DeleteEnergy1();
                CreateEnergy1();
            } else
            {
                //is p2
                DeleteEnergy2();
                CreateEnergy2();
            }

			if(selectionType == SelectionType.SelectCardToPlay && currentPlayer.HasCards()){
				//Debug.Log("The Highlighted Card is highlighted: " + currentPlayer.CardIsHighlighted(numIndex));
				//Debug.Log("Selected Card: " + numIndex);

				numObjects = currentPlayer.GetHandSize();
                //Debug.Log("Number of Cards: " + currentPlayer.GetHandSize());
                if (numCardsHighlighted == 0 && currentPlayer.HasCards()) {
                    currentPlayer.HighlightCard(0);
                    numCardsHighlighted++;
                    numIndex = 0;
                }

                KeyCode key = GetKey();
                if (prevCode == unusedKey) {
                    switch (key) {
                        case KeyCode.LeftArrow:
                            currentPlayer.UnHighlightCard(numIndex);
                            numIndex = (numIndex - 1 + numObjects) % numObjects;
                            currentPlayer.HighlightCard(numIndex);
                            break;

                        case KeyCode.RightArrow:
                            currentPlayer.UnHighlightCard(numIndex);
                            numIndex = (numIndex + 1) % numObjects;
                            currentPlayer.HighlightCard(numIndex);
                            break;

                        case KeyCode.Return:
                            CardState c = currentPlayer.GetCard(numIndex).GetComponent<Card>().GetState();
                            if(c.GetCost() <= currentPlayer.currentEnergy)
                            {
                                currentPlayer.ActivateCard(numIndex);
                                numCardsHighlighted--;
                            }
                            break;
                    }
                }
                prevCode = key;

				if(currentPlayer.currentEnergy <= 0 || !currentPlayer.HasCards()){
					if(canCheckForEndOfTurn) {
						currentPlayer.EndTurn();

						entityIndex = (entityIndex + 1) % entities.Count;
						currentEntity = entities[entityIndex].GetComponent<Entity>();
						currentEntity.BeginTurn();

					}
				}

			
			} else if(selectionType == SelectionType.SelectEntity){
				numObjects = entities.Count;

                if (NoEntitiesHighlighted()) {
                    Debug.Log("No Entities Highlighted Now!");
                    HighlightEntity(0);
                    numIndex = 0;
                }

                KeyCode key = GetKey();
                if (prevCode == unusedKey) {
                    switch (key) {
                        case KeyCode.LeftArrow:
                            UnHighlightEntity(numIndex);
                            numIndex = (numIndex - 1 + numObjects) % numObjects;
                            HighlightEntity(numIndex);
                            break;

                        case KeyCode.RightArrow:
                            UnHighlightEntity(numIndex);
                            numIndex = (numIndex + 1) % numObjects;
                            HighlightEntity(numIndex);
                            break;

                        case KeyCode.Return:
                            input = entities[numIndex];
                            UnHighlightEntity(numIndex);
                            Entity.UnHighlightAll();
                            numIndex = 0;
                            break;
                    }
                }
                prevCode = key;
			} else if(selectionType == SelectionType.SelectCard && currentPlayer.HasCards()){
                Debug.Log("The Highlighted Card is highlighted: " + currentPlayer.CardIsHighlighted(numIndex));
				Debug.Log("Selected Card: " + numIndex);

				numObjects = currentPlayer.GetHandSize();
				Debug.Log("Number of Cards: " + currentPlayer.GetHandSize());
                if (numCardsHighlighted == 0 && currentPlayer.HasCards()) {
                    currentPlayer.HighlightCard(0);
                    numCardsHighlighted++;
                    numIndex = 0;
                }

                KeyCode key = GetKey();
                if (prevCode == unusedKey) {
                    switch (key) {
                        case KeyCode.LeftArrow:
                            currentPlayer.UnHighlightCard(numIndex);
                            numIndex = (numIndex - 1 + numObjects) % numObjects;
                            currentPlayer.HighlightCardSelect(numIndex);
                            break;

                        case KeyCode.RightArrow:
                            currentPlayer.UnHighlightCard(numIndex);
                            numIndex = (numIndex + 1) % numObjects;
                            currentPlayer.HighlightCardSelect(numIndex);
                            break;

                        case KeyCode.Return:
                            input = currentPlayer.GetCard(numIndex);
                            currentPlayer.UnSelectAll();
                            break;
                    }
                }
                prevCode = key;

            } else if(selectionType == SelectionType.CardPileSelect){
                cardPile = GameObject.FindWithTag("CardPile").GetComponent<CardPile>();

                KeyCode key = GetKey();
                if (prevCode == unusedKey) {
                    switch (key) {
                        case KeyCode.LeftArrow:
                            cardPile.Left();
                            break;

                        case KeyCode.RightArrow:
                            cardPile.Right();
                            break;

                        case KeyCode.Return:
                            input = cardPile.GetSelected();
                            break;
                    }
                }
                prevCode = key;

            }
		} else if(currentEntity is Enemy){
			currentEnemy = (Enemy) currentEntity;
			currentEnemy.ActivateEffect();

			entities[entityIndex].tag = "Enemy";
			entityIndex = (entityIndex + 1) % entities.Count;
			currentEntity = entities[entityIndex].GetComponent<Entity>();
			entities[entityIndex].tag = "CurrentEntity";
			currentEntity.BeginTurn();
		}
	}

    private void HighlightEntity(int n) {
        entities[n].GetComponent<Entity>().Highlight();
    }

    private void UnHighlightEntity(int n) {
        entities[n].GetComponent<Entity>().UnHighlight();
    }

	private bool NoEntitiesHighlighted(){
		for(int i = 0; i < entities.Count; i++){
			if(entities[i].GetComponent<Entity>().IsHighlighted()) return false;
		}
		return true;
	}

	public void SetSelectionType(SelectionType s){
		selectionType = s;
	}

	public void SetInputToNull() {
		input = null;
	}

	public void SetBoolCheckEndOfTurn(bool b){
		canCheckForEndOfTurn = true;
	}

    public static GameController GetGameController() {
        return GameObject.FindWithTag("MainCamera").GetComponent<GameController>();
    }

    public static KeyCode GetKey() {
        if (Input.GetKey(KeyCode.Return)) return KeyCode.Return;
        if (Input.GetKey(KeyCode.LeftArrow)) return KeyCode.LeftArrow;
        if (Input.GetKey(KeyCode.RightArrow)) return KeyCode.RightArrow;
        return unusedKey;
    }

    //Used to create energy tokens for each player respectively
    public void CreateEnergy1()
    {
        for (int k = 0; k < currentPlayer.currentEnergy; k++) {
            p1Energy.Add(Instantiate(energy, new Vector2(-6.66f + (k * 0.75f), -1.86f), Quaternion.identity));
        }
    }

    public void CreateEnergy2()
    {
        for (int k = 0; k < currentPlayer.currentEnergy; k++) {
            p2Energy.Add(Instantiate(energy, new Vector2(-6.66f + (k * 0.75f), -3.14f), Quaternion.identity));
        }
    }

    //remove all energy tokens for a given player
    public void DeleteEnergy1()
    {
        for (int k = 0; k < p1Energy.Count; k++) {
            Destroy(p1Energy[k]);
        }
        p1Energy.Clear();
    }

    public void DeleteEnergy2()
    {
        for (int k = 0; k < p2Energy.Count; k++) {
            Destroy(p2Energy[k]);
        }
        p2Energy.Clear();
    }

    public void DeleteAllEnergy()
    {
        for (int k = 0; k < p1Energy.Count; k++) {
            Destroy(p1Energy[k]);
        }
        p1Energy.Clear();

        for (int k = 0; k < p2Energy.Count; k++) {
            Destroy(p2Energy[k]);
        }
        p2Energy.Clear();
    }

    public Entity GetHighlightedEntity(){
        for(int i = 0; i < entities.Count; i++){
            if(entities[i].GetComponent<Entity>().IsHighlighted()) return entities[i].GetComponent<Entity>();
        }
        return null;
    }
}

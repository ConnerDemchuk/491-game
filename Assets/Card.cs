using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    private CardState cardState;
    //private AudioClip HighlightSound;

    void Awake () {
        //SetIconSprite();
        //this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = cardState.GetIcon();
        //this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/FistStatic");
    }

    public string Test()
    {
        return cardState.GetIconString();
    }

    private SpriteRenderer GetRenderer() {
        return gameObject.GetComponent<SpriteRenderer>();
    }
    
    private void SetSprite(SpriteRenderer r, string filePath) {
        r.sprite = Resources.Load<Sprite>(filePath);
    }


    private void SetIconSprite()
    {
        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/FistStatic");
    }

    public void Highlight(){
        this.tag = "SelectedCard";
        SpriteRenderer r = GetRenderer();
        SetSprite(r, "Sprites/CardFrontHighlighted");
        r.sortingOrder = 2;

        GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>().PlayOneShot((AudioClip) Resources.Load("Sounds/SelectionSound"), 1);

        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 3;
    }

    public void UnHighlight() {
        this.tag = "Untagged";
        SpriteRenderer r = GetRenderer();
        SetSprite(r, "Sprites/CardFront");
        r.sortingOrder = 0;
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public bool IsHighlighted() {
        SpriteRenderer r = GetRenderer();
        return r.sprite == Resources.Load<Sprite>("Sprites/CardFrontHighlighted");
    }

    public void HighlightSelect(){
        SpriteRenderer r = GetRenderer();
        SetSprite(r, "Sprites/CardFrontSelected");
        r.sortingOrder = 2;
    }

    public void SetState(CardState c) {
        cardState = c;
        //Debug.Log(cardState.GetIconString());
        //this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(cardState.GetIconString());
        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = cardState.GetIcon();
    }

    public void AddState(CardState c){
        cardState = c;
        //this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(cardState.GetIconString());
        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = cardState.GetIcon();
    }

    public CardState GetState() {
        return cardState;
    }

	public void ActivateCard () {
        if(GameController.GetGameController().currentEntity is Player) {
            Renderer[] r = gameObject.GetComponents<Renderer>();
            for(int i = 0; i < r.Length; i++){
                Debug.Log("Fam I ran");
                r[i].enabled = false;
            }

            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            //Get input needed, 
            //give it to function in cardState
            //repeat until finished

            StartCoroutine("DoTasks");
        }
	}

    override
    public string ToString(){
        return cardState.ToString();
    }

    private IEnumerator DoTasks() {

        GameController controller = GameController.GetGameController();
        controller.SetBoolCheckEndOfTurn(false);
        List<Task> tasks = cardState.getTasks();
        bool flag = false;

        int i = 0;
        foreach (Task task in tasks) {

            //set the appropriate task
            switch (tasks[i].type) {

                case Task.Input.Null:
                    tasks[i].Run(null);
                    break;

                case Task.Input.Enemy:
                    controller.SetSelectionType(GameController.SelectionType.SelectEntity);
                    while (!(controller.input.GetComponent<Entity>() is Enemy)) { yield return null; }
                    task.Run(controller.input);
                    break;

                case Task.Input.Player:
                    controller.SetSelectionType(GameController.SelectionType.SelectEntity);
                    while (!(controller.input.GetComponent<Entity>() is Player)) {
                        yield return null;
                    }
                    task.Run(controller.input);
                    break;

                case Task.Input.Entity:
                    controller.SetSelectionType(GameController.SelectionType.SelectEntity);
                    flag = true;
                    while (controller.input == null) {
                        yield return null;
                    }
                    task.Run(controller.input);
                    StartCoroutine(flash(controller.input));
                    controller.SetInputToNull();

                    break;
                case Task.Input.Card:
                    controller.SetSelectionType(GameController.SelectionType.SelectCard);
                    while(controller.input == null){
                        yield return null;
                    }
                    cardState.getTasks()[i].Run(controller.input);
                    break;
                case Task.Input.DiscardCard:
                    GameObject g = Instantiate(Resources.Load("Card_Select"), new Vector3(0,0), Quaternion.identity) as GameObject;
                    g.tag = "CardPile";
                    CardPile c = g.GetComponent<CardPile>();
                    c.AddList(controller.currentPlayer.GetDiscard());
                    c.Initiate(5);

                    controller.SetSelectionType(GameController.SelectionType.CardPileSelect);
                    while(controller.input == null){
                        yield return null;
                    }
                    cardState.getTasks()[i].Run(controller.input);
                    c.DestroyAll();
                    break;

            }
            i++;
        }

        GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>().PlayOneShot(cardState.GetSoundEffect(), 1);


        //Add the card state to the current 
        //players discard pile
        GameObject j = controller.currentEntity.gameObject;
        Entity e = j.GetComponent<Entity>();
        Player p = (Player) e;
        p.LoseEnergy(cardState.GetCost());

        p.hand.Remove(gameObject);
        p.discard.Add(cardState);

        if(!flag) Destroy(gameObject);
        GameController.GetGameController().SetBoolCheckEndOfTurn(true);
        

        GameController.GetGameController().SetInputToNull();
        //We want to repeat using the same input when we can, but only for the same card, so we 
        //have to set it to null in between
        Debug.Log("Started from the beginning now we here");
        GameController.GetGameController().SetSelectionType(GameController.SelectionType.SelectCardToPlay);

        //set the gameController back to the normal card selection
    }

    public IEnumerator flash(GameObject g) {
        int i = 0;
        while(i < 10){
            i++;
            Debug.Log("flash");

                Renderer[] r = g.GetComponents<Renderer>();
                for(int k = 0; k < r.Length; k++){
                    r[k].enabled = !r[k].enabled;
                }
    
            yield return new WaitForSeconds(.08f);
        }
        Destroy(gameObject);
    }

}

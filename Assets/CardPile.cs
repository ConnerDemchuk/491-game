using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardPile : MonoBehaviour {
    public List<GameObject> displayed;
    public List<CardState> deck;
    private Sprite rightArrow;
    private Sprite leftArrow;

    public static List<Vector3> cardPositions;


    int indexFive = 0;
    int currentIndex = 0;

    void Awake() {
        displayed = new List<GameObject>();
        deck = new List<CardState>();
        cardPositions = new List<Vector3>();
        for(int i = 0; i < 49; i++){
            List<Task> cList = new List<Task>();
            cList.Add(new DebugTask(i));
            deck.Add(new StrikeState());
        }
        Initiate(5);

        

    }

    public void AddList(List<CardState> cards){
        deck.AddRange(cards);
    }


    public void Initiate(int numOfDisplayVar){
        int numOfDisplay = (numOfDisplayVar <= deck.Count) ? numOfDisplayVar : deck.Count;

        for(int i = 0; i < numOfDisplay; i++){ 
            displayed.Add(Instantiate(Resources.Load("Card"), new Vector3(-4 + i*2, 0, 0), Quaternion.identity) as GameObject);
            cardPositions.Add(displayed[i].transform.position);
        }

        for(int i = 0; i < displayed.Count; i++){
            displayed[i].GetComponent<Card>().AddState(deck[i]);
        }
        displayed[indexFive].GetComponent<Card>().Highlight();

    }

    public void Select(){
        deck[currentIndex].RunTask(null, 0);
    }

    public GameObject GetSelected(){
        return displayed[indexFive];
    }

    public void Right(){
       if(currentIndex == deck.Count - 1){

        } else {
            displayed[indexFive].GetComponent<Card>().UnHighlight();
            if(indexFive == displayed.Count - 1){
                //animate moving over and destroy 0th index
                Destroy(displayed[0]);

                for(int i = 1; i < displayed.Count; i++) StartCoroutine(AnimateMovement(displayed[i], cardPositions[i], cardPositions[i - 1], 50));
                for(int i = 0; i < displayed.Count - 1; i++) displayed[i] = displayed[i + 1];
                displayed[4] = Instantiate(Resources.Load("Card"), cardPositions[4], Quaternion.identity) as GameObject;
                displayed[4].GetComponent<Card>().AddState(deck[currentIndex + 1]);
                

            } else {
                indexFive++;
            }
            displayed[indexFive].GetComponent<Card>().Highlight();
            currentIndex++;
        }
    }

    public void Left(){
        if(currentIndex == 0){

        } else {
            displayed[indexFive].GetComponent<Card>().UnHighlight();
            if(indexFive == 0){
                //animate moving over and destroy 0th index
                Destroy(displayed[4]);

                for(int i = 1; i <= displayed.Count - 1; i++) StartCoroutine(AnimateMovement(displayed[i - 1], cardPositions[i - 1], cardPositions[i], 50));
                for(int i = displayed.Count - 1; i > 0; i--) displayed[i] = displayed[i - 1];

                displayed[0] = Instantiate(Resources.Load("Card"), cardPositions[0], Quaternion.identity) as GameObject;
                displayed[0].GetComponent<Card>().AddState(deck[currentIndex - 1]);


            } else {
                indexFive--;
            }
            displayed[indexFive].GetComponent<Card>().Highlight();
            currentIndex--;
        }
    }

    private static IEnumerator AnimateMovement(GameObject g, Vector3 start, Vector3 finish, int loops){
        for(int i = 0; i < loops; i++){
            if(g == null) break;

            float f = 1/ (float) loops;
            Debug.Log("Coroutine going");
            g.transform.position = Vector3.Lerp(start, finish, i*f);
            yield return null;
        }
    }

    public void DestroyAll(){
        for(int i = 0; i < displayed.Count; i++){
            Destroy(displayed[i]);
        }
        Destroy(gameObject);
    }

}
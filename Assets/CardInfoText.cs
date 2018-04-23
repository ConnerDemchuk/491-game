using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoText : MonoBehaviour
{
    public Text info;
    Entity currentEntity;
    Player currentPlayer;
    Card selectedCard;
    Enemy currentEnemy;
    public string s;

    // Use this for initialization
    void Start()
    {
        info = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        selectedCard = GameObject.FindWithTag("SelectedCard").GetComponent<Card>();
        if (selectedCard != null)
        {
            s = "Name: " + selectedCard.GetState().ToString() + "\n";
            s += "Required Energy: " + selectedCard.GetState().GetCost();
            info.text = s;
        } else
        {
            info.text = "null";
        }
    }
}

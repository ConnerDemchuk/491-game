using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Entity : MonoBehaviour {

    private int maxHP = 100;
    private int hp = 100;

    public List<CardState> allCards;
    public List<Effect> effects;

    void Awake () {
        allCards = new List<CardState>();
        effects = new List<Effect>();
	}

    public virtual void BeginTurn() {}
    public virtual void EndTurn() {}
    public virtual void ActivateCard() {}
    public virtual List<CardState> GetDeck() { return allCards; }
    public virtual void SetDeck(List<CardState> deck) { }

    public void AddEffect(Effect e)
    {
        effects.Add(e);
    }

    public void ApplyEffects()
    {
        for(int i=0; i < effects.Count; i++)
        {
            if(effects[i].GetTurnNum() > 0)
            {
                if (effects[i].GetHeals() == true)
                {
                    Heal(effects[i].GetDamage());
                }
                else
                {
                    Damage(effects[i].GetDamage());
                }
                effects[i].DecrementTurn();
            } else
            {
                effects.RemoveAt(i);
            }
        }
    }

    public int GetMaxHP() {
        return maxHP;
    }

    public int GetHP() {
        return hp;
    }

    public void Heal(int amount) {
        hp = Math.Min(Math.Max(hp + amount, 0), maxHP);
    }

    public void Damage(int amount) {
        Heal(-amount);
    }

    public void Highlight() {
        if(GameObject.FindWithTag("Arrow")){
            Destroy(GameObject.FindWithTag("Arrow"));
        }

        gameObject.tag = "HighlightedEntity";
        Vector3 currentEntityVector = GameController.GetGameController().GetHighlightedEntity().transform.position - new Vector3(0, 1.8f, 0f);
        GameObject g = Instantiate(Resources.Load("Arrow"), currentEntityVector, Quaternion.identity) as GameObject;
        g.tag = "Arrow";

        StartCoroutine(UpAndDown(g, currentEntityVector, currentEntityVector - new Vector3(0,1f,0), 50));

    }

    public void UnHighlight(){
        if(gameObject.tag == "HighlightedEntity") gameObject.tag = "Entity";
    }

    public static void UnHighlightAll(){
        if(GameObject.FindWithTag("Arrow")){
            Destroy(GameObject.FindWithTag("Arrow"));
        }
    }

    public bool IsHighlighted(){
        return (gameObject.tag == "HighlightedEntity");
    }

    private static IEnumerator UpAndDown(GameObject g, Vector3 start, Vector3 finish, int loops){
        while(true){
            if(g == null) break;
            for(int i = 0; i < loops; i++){
                if(g == null) break;

                float f = 1/ (float) loops;
                Debug.Log("Coroutine going");
                g.transform.position = Vector3.Lerp(start, finish, i*f);
                yield return null;
            }
        
            for(int i = 0; i < loops; i++){
                if(g == null) break;

                float f = 1/ (float) loops;
                Debug.Log("Coroutine going");
                g.transform.position = Vector3.Lerp(finish, start, i*f);
                yield return null;
            }

        }

    }


}
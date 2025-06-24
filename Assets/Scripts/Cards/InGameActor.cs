using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InGameActor : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public TMPro.TextMeshProUGUI objectName, description, costHealth, Block, Weak
        , Exposed, Strength;
    public Enemy enemy;
    public Card card;
    //public int ID;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();


    }

    void OnMouseDown()
    {
        if (BattleManager.Instance.gameState == GameState.Battle)
        {
            GetComponent<TargetingCard>().StartTargeting(card);
        }
        else if (BattleManager.Instance.gameState == GameState.PrePostBattle)
        {
            BattleManager.Instance.AddCardDeck(card);
        }
        
    }
    public void UpdateTexts(int health, int block, int weak, int exposed, int strength, int mana)
    {
        costHealth.text = "Health - " + health.ToString();
        Block.text = "Block - " + block.ToString();
        Weak.text = "Weak - " + weak.ToString();
        Exposed.text = "Exposed - " + exposed.ToString();
        Strength.text = "Strength - " + strength.ToString();
        if(mana >= 0)
        {
            description.text = "Mana - " + mana.ToString();
        }
    }
    public void ReturnCard()
    {
        card.currentCard = true;
    }
    public void ReturnTarget()
    {
        if(enemy != null)
        {
            enemy.isTargeted = true;
        }
        BattleManager.Instance.Action();

    }
}


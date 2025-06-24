using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public string cardName;
    public string description;
    public int cost, sequence, attack, block, frequency, rarity, Exposed, Weak, 
        Hexed, Mark, Strength, Enraged, Regenerating, Tough, 
        Nullify, Heal;
    public float font;
    public Sprite sprite;
    public CardData cardData;
    public bool currentCard, playerTargeted, enemyTargeted, noTarget;
    public CardType cardType;
    public PlayerCharacter playerCharacter;
    public InGameActor inGameObject;


    public Card(CardData CardData)
    {
        cardData = CardData;
        cardName = CardData.cardName;
        description = CardData.description;
        cost = CardData.cost;
        sprite = CardData.sprite;
        cardType = CardData.cardType;
        attack = CardData.power;
        block = CardData.block;
        rarity = CardData.rarity;
        frequency = CardData.frequency;
        Exposed = CardData.Exposed;
        Weak = CardData.Weak;
        Hexed = CardData.Hexed;
        Mark = CardData.Mark;
        Strength = CardData.Strength;
        Enraged = CardData.Enraged;
        Regenerating = CardData.Regenerating;
        Heal = CardData.Heal;
        Tough = CardData.Tough;
        Nullify = CardData.Nullify;
        playerCharacter = CardData.playerCharacter;
        playerTargeted = CardData.playerTargeted;
        enemyTargeted = CardData.enemyTargeted;
        noTarget = CardData.noTarget;
        font = CardData.font;



    }

    public void Dispose()
    {

        sprite = null;
        cardData = null;
        currentCard = false;
        UnityEngine.Object.Destroy(inGameObject.gameObject);
        inGameObject = null;

        Debug.Log($"Card {cardName} disposed.");

    }
    public void DisposeInGameActor()
    {
        currentCard = false;
        UnityEngine.Object.Destroy(inGameObject.gameObject);
        inGameObject = null;
    }

}
public enum CardType { Attack, Skill, Enchantment }
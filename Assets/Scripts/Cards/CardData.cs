using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public string description;
    public int cost, power, block, rarity, frequency, Exposed, Weak,
        Hexed, Mark, Strength, Enraged, Regenerating, Tough,
        Cull, Nullify, Heal;
    public float font;

    public Sprite sprite;
    public CardType cardType;
    public bool playerTargeted, enemyTargeted, noTarget;
    public PlayerCharacter playerCharacter;

}
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Player : MonoBehaviour
{
    public int health, mana, maxMana, Strength, dex, block, Impower, Exposed, Weak;
    public Sprite HeroSprite, NullSprite;
    public InGameActor inGameActor;

    Sprite sprite;
   

    private void Awake()
    {
        SetStats(BattleManager.Instance.playerCharacter);

    }

    public void SetStats(PlayerCharacter character)
    {
        if (character == PlayerCharacter.Hero)
        {
            health = 50;
            maxMana = 3;
            mana = maxMana;
            Strength = 0;
            dex = 0;
            block = 0;
            sprite = HeroSprite;
        }
        else if (character == PlayerCharacter.Null)
        {

        }
    }
    public void UpdateTexts()
    {
        inGameActor.UpdateTexts(health, block, Weak, Exposed, Strength, mana);
    }

    void OnEnable()
    {
        BattleManager.OnAction += PlayerAction;
    }

    void OnDisable()
    {
        BattleManager.OnAction -= PlayerAction;
    }

    public void EnemyAttack(EnemyAttack enemyAttack, Enemy enemy)
    {
        for (int i = enemyAttack.frequency; i > 0; i--)
        {
            int damage = (enemyAttack.damage + enemy.Strength);
            if(enemy.Weak >= 1)
            {
                damage = damage * 3 / 4;
            }
            if (Exposed >= 1)
            {
                damage = damage * 3 / 2;
            }
            if (damage > block)
            {
                damage -= block;
                block = 0;
            }
            else if (damage <= block)
            {
                block -= damage;
                damage = 0;
            }
            health -= damage;
        }
        Exposed += enemyAttack.Exposed;
        Weak += enemyAttack.Weak;
        UpdateTexts();


    }


    public void NextLevel() //Turn into Event
    {
        maxMana = 3;
        Strength = 0;
        dex = 0;
    }
    public void playerTurnReset()
    {
        mana = maxMana;
        block = 0;
        if (Impower > 0)
        {
            Impower--;
        }
        if (Exposed > 0)
        {
            Exposed--;
        }
        if (Weak > 0)
        {
            Weak--;
        }
        UpdateTexts();
    }

    public void PlayerAction(Card card) //Event
    {
        Strength += card.Strength;


        mana -= card.cost;
        block += card.block;
        health += card.Heal;
        UpdateTexts();
    }
    public void NextEncounter()
    {
        mana = maxMana;
        block = 0;
        Impower = 0;
        Exposed = 0;
        Weak = 0;
        UpdateTexts();
    }
    public void Dispose()
    {
        // Safely destroy the visual actor
        if (inGameActor != null)
        {
            Destroy(inGameActor.gameObject); // destroy the GameObject if needed
            inGameActor = null;
        }

        // Clear visual or logic-only assets
        HeroSprite = null;
        NullSprite = null;
        sprite = null;
        // No GameObject destruction here — let GameController decide if/when to destroy player
    }


}


using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using static Unity.Collections.AllocatorManager;

public class Enemy
{
    public string enemyName;
    public int health, currentTurnNumber, block, Strength, Exposed, Weak;
  
    public Sprite sprite;
    public bool isTargeted, isDead;

    EnemyData enemyData;
    public InGameActor inGameActor;
    public EnemyAttackIndex enemyAttackIndex;
    EnemyAttack enemyattack;
    List<EnemyAttack> randomAttackSet, specificTurnAttackSet;
    List<EnemyAttack> tempAttackSet;


    public void PlayerAction(Card card) //Event
    {
        if (isTargeted)
        {
            for (int i = card.frequency; i > 0; i--)
            {
                int damage = (card.attack +
                    BattleManager.Instance.player.Strength);
                if (BattleManager.Instance.player.Weak >= 1)
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
                }
                else if (damage <= block)
                {
                    block -= damage;
                    damage = 0;
                }
                health -= damage;
            }
            isTargeted = false;
            Exposed += card.Exposed;
            Weak += card.Weak;
            //animations for debuffs
            UpdateAttackText();//Do I wnat to merge these functions?
            UpdateTexts();

            if (health <= 0)
            {
                Dispose();
            }
        }

    }


    void DeclementBuffsAndDebuffs()
    {
        if (block > 0)
        {
            block = 0;
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


    public void enemyAttackSelect()
    {
        bool isRandom = true;
        EnemyAttack currentAttck = null;
        if (randomAttackSet.Count == 0 && specificTurnAttackSet.Count == 0)
        {
            CreateAttackList();
            currentTurnNumber = 1;
        }
        for (int i = 0; i < specificTurnAttackSet.Count; i++)
        {
            //Make sure in Unity that the attacks are properly ordered so when they
            //are in this list, the higher turn attacks will overwrite
            //the lower turn attacks
            if (currentTurnNumber % specificTurnAttackSet[i].specificTurn == 0)
            {
                currentAttck = specificTurnAttackSet[i];
                isRandom = false;
            }
        }
        if (isRandom)
        {
            //attacks that are doubled will have higher chance to show up
            int randomNum = UnityEngine.Random.Range(0, randomAttackSet.Count);

            currentAttck = randomAttackSet[randomNum];

        }
        else
        {

        }
        enemyattack = currentAttck;
        UpdateAttackText();
    }

    void UpdateAttackText()
    {
        if (enemyattack.AttackName.Contains("Attack"))
        {
            int damage = enemyattack.damage;
            int frequency = enemyattack.frequency;
            damage += Strength;
            if (Weak >= 1)
            {
                damage = damage * 3 / 4;
            }
            if (BattleManager.Instance.player.Exposed >= 1)
            {
                damage = damage * 3 / 2;
            }
            inGameActor.description.text = enemyattack.AttackName + damage +
                "(" + frequency + ")";
            inGameActor.description.color = Color.red;
        }
        else if (enemyattack.AttackName.Contains("Buff"))
        {
            inGameActor.description.text = enemyattack.AttackName;
            inGameActor.description.color = Color.blue;
        }
        else if (enemyattack.AttackName.Contains("Debuff"))
        {
            inGameActor.description.text = enemyattack.AttackName;
            inGameActor.description.color = Color.green;
        }
        UpdateTexts();//Do i need this?
    }



    public void EnemyTurn()
    {
        if (enemyattack != null)
        {
            Attack(enemyattack);

        }

        currentTurnNumber++;
        DeclementBuffsAndDebuffs();
    }
    void Attack(EnemyAttack eAttack)
    {


        BattleManager.Instance.player.EnemyAttack(eAttack, this);
        block += eAttack.block;
        Strength += eAttack.Strength;
        Debug.Log(eAttack.AttackName);
    }
    public void CreateAttackList()
    {
        foreach (var x in enemyAttackIndex.enemyAttackData)
        {
            if (x.specificTurn == 0)
            {
                EnemyAttack eAttack = new EnemyAttack(x);
                randomAttackSet.Add(eAttack);
            }
            else if (x.specificTurn != 0)
            {
                EnemyAttack eAttack = new EnemyAttack(x);
                specificTurnAttackSet.Add(eAttack);
            }

        }
    }

    public Enemy(EnemyData EnemyData, InGameActor inGame)
    {
        BattleManager.OnAction += PlayerAction;
        enemyData = EnemyData;
        enemyName = EnemyData.enemyName;
        sprite = EnemyData.sprite;
        health = EnemyData.health;
        Strength = 0;
        Exposed = 0;
        Weak = 0;
        isTargeted = false;
        enemyAttackIndex = new EnemyAttackIndex(EnemyData);
        randomAttackSet = new List<EnemyAttack>();
        specificTurnAttackSet = new List<EnemyAttack>();
        inGameActor = inGame;
        inGame.enemy = this;
        UpdateSprites();
        UpdateTexts();
    }
    public void UpdateSprites()
    {
        inGameActor.objectName.text = enemyName;
        inGameActor.spriteRenderer.sprite = sprite;
    }
    public void UpdateTexts()
    {
        inGameActor.UpdateTexts(health, block, Weak, Exposed, Strength, -1);
    }
    
    public void Dispose()
    {
        if (inGameActor != null)
            UnityEngine.Object.Destroy(inGameActor.gameObject);
        // Unsubscribe from events
        BattleManager.OnAction -= PlayerAction;

        // Clear Unity references and custom classes
        sprite = null;
        inGameActor = null;

        enemyData = null;
        enemyAttackIndex = null;
        enemyattack = null;

        randomAttackSet?.Clear();
        specificTurnAttackSet?.Clear();
        tempAttackSet?.Clear();

        randomAttackSet = null;
        specificTurnAttackSet = null;
        tempAttackSet = null;

        isDead = true;

        // Optional: log to confirm cleanup
        Debug.Log($"Enemy {enemyName} disposed.");
    }

}
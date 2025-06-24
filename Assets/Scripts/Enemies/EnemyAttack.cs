using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using static Unity.Collections.AllocatorManager;

public class EnemyAttack
{
    public int damage, block, Strength, Exposed, Weak, Heal;
    public int frequency, attackRate, specificTurn;
    public string AttackName;

    public EnemyAttack(EnemyAttackData enemyAttackData)
    {
        damage = enemyAttackData.damage;
        block = enemyAttackData.block;
        Strength = enemyAttackData.Strength;
        Exposed = enemyAttackData.Exposed;
        frequency = enemyAttackData.frequency;
        attackRate = enemyAttackData.attackRate;
        specificTurn = enemyAttackData.specificTurn;
        AttackName = enemyAttackData.attackName;
        Weak = enemyAttackData.Weak;
        Heal = enemyAttackData.Heal;
    }

}


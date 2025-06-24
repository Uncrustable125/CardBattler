using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Enemy Attack", menuName = "EnemyAttack")]

public class EnemyAttackData : ScriptableObject
{

    public string attackName;
    public int damage, block, Strength, Exposed, Weak, Heal,
        frequency, attackRate, specificTurn;

}

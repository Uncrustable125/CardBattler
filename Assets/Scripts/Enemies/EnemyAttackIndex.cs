using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAttackIndex
{
    public EnemyAttackData[] enemyAttackData;
    public EnemyAttackIndex(EnemyData enemyData)
    {
        enemyAttackData = Resources.LoadAll("Enemies/EnemyAttacks/"+enemyData.name,
            typeof(EnemyAttackData)).Cast<EnemyAttackData>().ToArray();

    }
}

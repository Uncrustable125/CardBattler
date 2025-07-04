using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyIndex
{
    public EnemyData[] enemyData;
    public EnemyIndex()
    {
        enemyData = Resources.LoadAll("Enemies/Enemies", 
            typeof(EnemyData)).Cast<EnemyData>().ToArray();

    }
}

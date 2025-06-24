using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyIndex : MonoBehaviour
{
    public EnemyData[] enemyData;
    void Awake()
    {
        enemyData = Resources.LoadAll("Enemies/Enemies", 
            typeof(EnemyData)).Cast<EnemyData>().ToArray();

    }
}

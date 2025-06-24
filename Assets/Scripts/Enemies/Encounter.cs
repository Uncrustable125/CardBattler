using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    public int enemies;

    EnemyIndex enemyIndex;
    public EnemyData enemy0, enemy1, enemy2, enemy3;
    private void Awake()
    {
        enemyIndex = gameObject.AddComponent<EnemyIndex>();
    }
    public Encounter GetEncounter(int y)
    {
        if (y == 0)
        {
            foreach (var enemy in enemyIndex.enemyData)
            {
                if (enemy.enemyName == "Bug")
                {
                    enemy0 = enemy;
                    enemy1 = enemy;
                   enemy2 = enemy;
                    enemies = 3;
                }
            }
            return this;
        }
        else if (y == 1)
        {
            foreach (var enemy in enemyIndex.enemyData)
            {
                if (enemy.enemyName == "Bug")
                {
                    enemy0 = enemy;
                     enemy1 = enemy;


                }
                else if (enemy.enemyName == "Lamp")
                {

                     enemy2 = enemy;

                }
                enemies = 3;
            }
            return this;

        }
        else if (y == 2)
        {
            foreach (var enemy in enemyIndex.enemyData)
            {
                if (enemy.enemyName == "Bug")
                {
                    enemy0 = enemy;
                    


                }
                else if (enemy.enemyName == "Lamp")
                {
                    enemy1 = enemy;
                    enemy2 = enemy;

                }
                enemies = 3;
            }
            return this;
        }
        else if (y == 3)
        {
            return this;

        }
        return this;
    }
}

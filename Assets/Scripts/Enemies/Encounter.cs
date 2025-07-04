using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Encounter
{
    public int enemies;

    EnemyIndex enemyIndex;
    public EnemyData enemy0, enemy1, enemy2, enemy3;
    public Encounter(int y)
    {
        //        enemyIndex = gameObject.AddComponent<EnemyIndex>();
        enemyIndex = new EnemyIndex();
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

            }
        }
    }
}








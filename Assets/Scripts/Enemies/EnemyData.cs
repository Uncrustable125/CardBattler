using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]

public class EnemyData : ScriptableObject
{

        public string enemyName;
        public string description;
        public int health;
        public Sprite sprite;
        public EnemyAttackData[] enemyAttackData;
}

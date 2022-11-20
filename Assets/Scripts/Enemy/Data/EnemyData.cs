using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy/New enemy")]
public class EnemyData : ScriptableObject
{
    public string name;
    public EnemyTypeEnum enemyType;
    public int pointOfLife;
}

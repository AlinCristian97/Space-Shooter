using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "ScriptableObjects/EnemyConfig")]
public class EnemySO : ScriptableObject
{
    public float fireRate;
    public float speed;
    public float health;
}

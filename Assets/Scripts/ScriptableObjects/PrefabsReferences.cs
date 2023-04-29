using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabReferences", menuName = "TP/PrefabReferences", order = 4)]
public class PrefabsReferences : ScriptableObject
{
    [Header("Grid")]
    public GridCell gridCellPrefab;

    [Header("Managers")]
    public PoolManager poolManagerPrefab;

    [Header("Entities")]
    public PlayerController playerPrefab;
    //public EnemyController enemyPrefab;

    [Header("Bullets")]
    public BulletController playerBulletPrefab;
    public BulletController enemyBulletPrefab;

}

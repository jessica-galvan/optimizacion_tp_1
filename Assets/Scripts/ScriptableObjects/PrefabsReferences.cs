using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabReferences", menuName = "TP/PrefabReferences", order = 4)]
public class PrefabsReferences : ScriptableObject
{
    public GridCell gridCellPrefab;
    public PlayerController playerPrefab;
    //public EnemyController enemyPrefab;
}

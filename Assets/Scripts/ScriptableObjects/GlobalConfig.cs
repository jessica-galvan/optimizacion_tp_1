using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalConfig", menuName = "TP/GlobalConfig", order = 1)]
public class GlobalConfig : ScriptableObject
{
    [Header("Player")]
    public float playerWaitTimeRespawn = 2f;

    [Header("CustomUpdate Settings")]
    public int gameplayFPSTarget = 60;
    public int uiFPSTarget = 60;
    public bool activeMaxAppTarget = false;
    public int maxFPSTarget = 75;

    [Header("Enemy Settings")]
    public int totalEnemiesLevel = 100;
    public int maxEnemiesInLevelAtAllTimes = 4;
    public float spawningTime = 10f;
    public float retrySpawnTime = 4f;
    public int triesPerSpawn = 4;
}

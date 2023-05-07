using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour, IUpdate
{
    public EnemyConfig enemyConfig;
    [ReadOnly] public int totalKilled = 0;
    [ReadOnly] public int currentEnemyQuantitySpawned = 0;
    [ReadOnly] public int currentTimeFrameCollider;
    [ReadOnly] public int currentTimeFrameCheckLocaiton;

    private int totalWeight = 0;
    private GameManager gameManager;
    private float currentTime;
    private bool canSpawnEnemies;
    private int maxSpawnPoints;

    public Action<int> OnEnemyKilled = delegate { };

    public void Initialize()
    {
        canSpawnEnemies = true;
        gameManager = GameManager.Instance;
        gameManager.updateManager.fixCustomUpdater.Add(this);
        maxSpawnPoints = gameManager.levelGrid.enemySpawnPoints.Count - 1; //Precomputation. The spaces are not going to change and better do this calculation once than everywhere we might need it;
        currentTime = gameManager.globalConfig.retrySpawnTime;

        for (int i = 0; i < enemyConfig.enemyStatesWeight.Length; i++)
        {
            totalWeight += enemyConfig.enemyStatesWeight[i].weight; //PRECOMPUTATION. why should we re add it every time we get a random action when we could do it once here?
        }
    }

    public void DoUpdate()
    {
        if (gameManager.Pause) return;

        if(enemyConfig.enemyColliderSlicesFrames)
            currentTimeFrameCollider = Time.frameCount % enemyConfig.slicesColliderQuantity;

        currentTimeFrameCheckLocaiton = Time.frameCount / enemyConfig.slicesCellLocationCheckQuantity;

        if (canSpawnEnemies)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                EnemySpawn();
            }
        }
    }

    private bool HasSpaceToSpawnEnemy()
    {
        return currentEnemyQuantitySpawned < gameManager.globalConfig.maxEnemiesInLevelAtAllTimes;
    }

    public void EnemyKilled()
    {
        totalKilled++;
        OnEnemyKilled.Invoke(totalKilled);
        currentEnemyQuantitySpawned--;
        CheckWinCondition();
        canSpawnEnemies = HasSpaceToSpawnEnemy(); //is it lazy computation if we only update it when the number changes instead of doing the method on every frame of the DoUpdate?)
    }

    public void EnemySpawn()
    {
        GridCell spawnPoint = null;
        bool foundViableSpawnPoint = false;
        int previousPosition = -1;
        int triesPerSpawn = gameManager.globalConfig.triesPerSpawn; //this is just in case that ALL spawn points are occupied right at this moment, we search for a few times and then 

        while (!foundViableSpawnPoint && triesPerSpawn > 0)
        {
            int randomPosition = MiscUtils.RandomInt(0, maxSpawnPoints);
            if (randomPosition == previousPosition) continue;
            previousPosition = randomPosition;

            spawnPoint = gameManager.levelGrid.enemySpawnPoints[randomPosition];
            foundViableSpawnPoint = !spawnPoint.IsOcupied;
            triesPerSpawn--;
        }

        if (spawnPoint != null)
        {
            var enemy = gameManager.poolManager.GetEnemy();
            enemy.Spawn(spawnPoint);
            currentTime = gameManager.globalConfig.spawningTime;
            currentEnemyQuantitySpawned++;
            canSpawnEnemies = HasSpaceToSpawnEnemy();
        }
        else
        {
            currentTime = gameManager.globalConfig.retrySpawnTime;
        }
    }

    private void CheckWinCondition()
    {
        if (totalKilled == gameManager.globalConfig.totalEnemiesLevel)
        {
            GameManager.Instance.WinGame();
        }
    }

    public EnemyStates GetRandomWeightAction()
    {
        EnemyStates states = EnemyStates.Move;

        int random = Random.Range(0, totalWeight + 1);

        for (int i = 0; i < enemyConfig.enemyStatesWeight.Length; i++)
        {
            random -= enemyConfig.enemyStatesWeight[i].weight;

            if (random <= 0)
            {
                states = enemyConfig.enemyStatesWeight[i].state;
                break;
            }
        }

        return states;
    }

    private void OnDestroy()
    {
        if (GameManager.HasInstance)
        {
            gameManager.updateManager.fixCustomUpdater.Remove(this);
        }
    }
}

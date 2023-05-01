using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IUpdate
{
    public EnemyConfig enemyConfig;
    [ReadOnly] public int totalKilled = 0;
    [ReadOnly] public int currentEnemyQuantitySpawned = 0;

    private Dictionary<EnemyStates, int> enemyStatesWeight;
    private GameManager gameManager;
    private float currentTime;
    private bool canSpawnEnemies;
    private int maxSpawnPoints;

    public void Initialize()
    {
        canSpawnEnemies = true;
        gameManager = GameManager.Instance;
        maxSpawnPoints = gameManager.levelGrid.enemySpawnPoints.Count - 1; //Precomputation. The spaces are not going to change and better do this calculation once than everywhere we might need it;
        currentTime = gameManager.globalConfig.retrySpawnTime;

        enemyStatesWeight = new Dictionary<EnemyStates, int>();
        for (int i = 0; i < enemyStatesWeight.Count; i++)
        {
            enemyStatesWeight[enemyConfig.enemyStatesWeight[i].state] = enemyConfig.enemyStatesWeight[i].weight;
        }

        gameManager = GameManager.Instance;
        gameManager.updateManager.fixCustomUpdater.Add(this);
    }

    public void DoUpdate()
    {
        if (gameManager.Pause) return;

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
        return currentEnemyQuantitySpawned <= gameManager.globalConfig.maxEnemiesInLevelAtAllTimes;
    }

    public void EnemyKilled()
    {
        CheckWinCondition();
        currentEnemyQuantitySpawned--;
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
            GameManager.Instance.WinGame();
    }

    public EnemyStates GetRandomAction()
    {
        EnemyStates states = EnemyStates.Idle;

        int total = 0;
        foreach (var item in enemyStatesWeight)
        {
            total += item.Value;
        }

        int random = Random.Range(0, total + 1);

        foreach (var item in enemyStatesWeight)
        {
            random -= item.Value;
            if (random <= 0)
            {
                states = item.Key;
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

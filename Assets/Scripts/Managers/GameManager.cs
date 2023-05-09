using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public GlobalConfig globalConfig;
    public PrefabsReferences prefabReferences;
    public LevelGrid levelGrid;
    public Transform hidePoolPoint;

    [Header("Managers")]
    [ReadOnly] public InputManager inputManager;
    [ReadOnly] public UpdateManager updateManager;
    [ReadOnly] public PoolManager poolManager;
    [ReadOnly] public EnemyManager enemyManager;

    private static GameManager instance;
    private int playerDeadCount = 0;
    private bool pause = false;
    private bool won = false;

    //Properties
    public static GameManager Instance => instance;
    public static bool HasInstance => instance != null;
    public PlayerModel Player { get; private set; }
    public bool Pause => pause;
    public int PlayerDeadCounter => playerDeadCount;
    public bool Won => won;

    //Events
    public Action OnPlayerDie;
    public Action<bool> OnPause;
    public Action OnWin;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;

        levelGrid.Initialize();

        updateManager = Instantiate(prefabReferences.updateManager);
        updateManager.Initialize();

        inputManager = GetComponent<InputManager>();
        inputManager.Initialize();
        inputManager.OnPause += TogglePause;

        var playerController = Instantiate(prefabReferences.playerPrefab, levelGrid.playerSpawnPoint.spawnPoint.position, levelGrid.playerSpawnPoint.transform.rotation);
        playerController.Initialize();
        Player = playerController.model;
        Player.OnDie += OnPlayerHasDie;

        poolManager = Instantiate(prefabReferences.poolManagerPrefab);
        poolManager.Initialize();

        enemyManager = Instantiate(prefabReferences.enemyManager);
        enemyManager.Initialize();
    }

    public void Start()
    {
        AudioManager.instance.PlayMusic(AudioManager.instance.soundReferences.levelMusic);
        SpawnPlayer();
    }

    public void SetPlayer(PlayerModel player)
    {
        Player = player;
    }

    public void SetPause(bool value)
    {
        if (pause == value) return;

        pause = value;
        OnPause.Invoke(pause);
    }

    private void TogglePause()
    {
        if (Won) return;
        SetPause(!pause);
    }

    public void WinGame()
    {
        won = true;
        pause = true;

        AudioManager.instance.PlaySFXSound(AudioManager.instance.soundReferences.win);
        OnWin.Invoke();
    }

    public void OnPlayerHasDie()
    {
        playerDeadCount++;
        OnPlayerDie.Invoke();

        levelGrid.playerSpawnPoint.StartSpawnAnimation();
        StartCoroutine(PausableTimerCoroutine(globalConfig.playerWaitTimeRespawn, SpawnPlayer));
    }

    private void SpawnPlayer()
    {
        Player.Spawn(levelGrid.playerSpawnPoint);
    }

    public IEnumerator PausableTimerCoroutine(float timeDuration, Action OnEndAction)
    {
        float t = 0f;

        while (t < 1f)
        {
            if (!Pause)
            {
                t += Time.deltaTime / timeDuration;
            }
            yield return null;
        }

        OnEndAction();
    }

    public void OnDestroy()
    {
        inputManager.OnPause -= TogglePause;
    }
}

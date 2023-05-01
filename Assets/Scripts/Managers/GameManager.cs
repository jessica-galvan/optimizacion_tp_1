using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GameManager : MonoBehaviour, IUpdate
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
    private float currentTime;
    private bool won = false;

    //Properties
    public static GameManager Instance => instance;
    public static bool HasInstance => instance != null;
    public PlayerModel Player { get; private set; }
    public bool Pause => pause;
    public float CurrentTime => currentTime;
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
        updateManager.fixCustomUpdater.Add(this);

        inputManager = GetComponent<InputManager>();
        inputManager.Initialize();
        inputManager.OnPause += TogglePause;

        poolManager = Instantiate(prefabReferences.poolManagerPrefab);
        poolManager.Initialize();

        var playerController = Instantiate(prefabReferences.playerPrefab, levelGrid.playerSpawnPoint.spawnPoint.position, levelGrid.playerSpawnPoint.transform.rotation);
        playerController.Initialize();
        Player = playerController.model;
        Player.Spawn(levelGrid.playerSpawnPoint);
        Player.OnDie += OnPlayerHasDie;
    }


    public void DoUpdate()
    {
        if (Pause) return;

        currentTime += Time.deltaTime;
        TestingCheats();
    }

    private void TestingCheats()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            WinGame();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            Player.TakeDamage();
        }
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

        OnWin.Invoke();
    }

    public void OnPlayerHasDie()
    {
        playerDeadCount++;
        OnPlayerDie.Invoke();

        StartCoroutine(PausableTimerCoroutine(globalConfig.playerWaitTimeRespawn, () => Player.Spawn(levelGrid.playerSpawnPoint)));
    }

    public void OnDestroy()
    {
        inputManager.OnPause -= TogglePause;
        updateManager.fixCustomUpdater.Remove(this);
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
}

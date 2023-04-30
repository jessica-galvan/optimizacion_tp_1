using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GameManager : MonoBehaviour, IUpdate
{
    [Header("References")]
    public PrefabsReferences prefabReferences;
    public LevelGrid levelGrid;
    public Transform hidePoolPoint;

    [Header("Player Dead Config")]
    public float waitTimeToRespawn = 2f;

    [Header("Managers")]
    [ReadOnly] public InputManager inputManager;
    [ReadOnly] public UpdateManager updateManager;
    [ReadOnly] public PoolManager poolManager;

    private static GameManager _instance;
    private int playerDeadCount = 0;
    private bool _pause = false;
    private float _currentTime;
    private bool won = false;

    //Properties
    public static GameManager Instance => _instance;
    public static bool HasInstance => _instance != null;
    public PlayerModel Player { get; private set; }
    public bool Pause => _pause;
    public float CurrentTime => _currentTime;
    public int PlayerDeadCounter => playerDeadCount;
    public bool Won => won;

    //Events
    public Action OnPlayerDie;
    public Action<bool> OnPause;
    public Action OnWin;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }

        _instance = this;

        levelGrid.ReGenerateMatrix();
        
        poolManager = Instantiate(prefabReferences.poolManagerPrefab);
        updateManager = Instantiate(prefabReferences.updateManager);

        inputManager = GetComponent<InputManager>();
        inputManager.OnPause += TogglePause;

        updateManager.Initialize();
        updateManager.fixCustomUpdater.Add(this);

        var playerController = Instantiate(prefabReferences.playerPrefab, levelGrid.playerSpawnPoint.spawnPoint.position, levelGrid.playerSpawnPoint.transform.rotation);
        playerController.Initialize();
        Player = playerController.model;
        Player.Spawn(levelGrid.playerSpawnPoint);
        Player.OnDie += OnPlayerHasDie;
    }


    private void Start()
    {

        //TODO enemy manager
    }

    public void DoUpdate()
    {
        if (!Pause)
        {
            _currentTime += Time.deltaTime;
        }
    }

    private void TestingCheats()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            WinGame();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            Player.Die();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {

        }
    }

    public void SetPlayer(PlayerModel player)
    {
        Player = player;
    }

    public void SetPause(bool value)
    {
        if (_pause == value) return;

        _pause = value;
        OnPause.Invoke(_pause);
    }

    private void TogglePause()
    {
        if (Won) return;
        SetPause(!_pause);
    }

    public void WinGame()
    {
        won = true;
        _pause = true;

        OnWin.Invoke();
    }

    public void OnPlayerHasDie()
    {
        playerDeadCount++;
        OnPlayerDie.Invoke();

        StartCoroutine(PlayerRespawn());
    }

    private IEnumerator PlayerRespawn()
    {
        float t = 0f;

        while (t < 1f)
        {
            if (!Pause)
                t += Time.deltaTime / waitTimeToRespawn;
            yield return null;
        }

        Player.Spawn(levelGrid.playerSpawnPoint);
    }

    public void OnDestroy()
    {
        inputManager.OnPause -= TogglePause;
        updateManager.fixCustomUpdater.Remove(this);
    }
}

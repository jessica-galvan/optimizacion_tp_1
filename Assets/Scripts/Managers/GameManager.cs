using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour, IUpdate
{
    [Header("References")]
    public PrefabsReferences prefabReferences;
    public LevelGrid levelGrid;
    public float spawningOffset = 1;
    public Transform hidePoolPoint;

    [Header("Managers")]
    [ReadOnly] public InputManager inputManager;
    [ReadOnly] public UpdateManager updateManager;
    [ReadOnly] public PoolManager poolManager;

    private static GameManager _instance;
    private bool _pause = false;
    private float _currentTime;

    //Properties
    public static GameManager Instance => _instance;
    public static bool HasInstance => _instance != null;
    public PlayerModel Player { get; private set; }
    public bool Pause => _pause;
    public float CurrentTime => _currentTime;

    //Events
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

        poolManager = Instantiate(prefabReferences.poolManagerPrefab);
        updateManager = GetComponent<UpdateManager>();
        inputManager = GetComponent<InputManager>();

        levelGrid.ReGenerateMatrix();

        Player = Instantiate(prefabReferences.playerPrefab, levelGrid.playerSpawnPoint.spawnPoint.position, levelGrid.playerSpawnPoint.transform.rotation).model;
        levelGrid.playerSpawnPoint.SetOccupiedStatus(true, Player);
    }


    private void Start()
    {
        GameManager.Instance.updateManager.gameplayCustomUpdate.Add(this);
        inputManager.OnPause += TogglePause;

        //TODO enemy manager
    }

    public void DoUpdate()
    {
        if (!Pause)
        {
            _currentTime += Time.deltaTime;
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
        SetPause(!_pause);
    }

    public void OnDestroy()
    {
        inputManager.OnPause -= TogglePause;
        GameManager.Instance.updateManager.gameplayCustomUpdate.Remove(this);
    }
}

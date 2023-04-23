using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour, IUpdate
{
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

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }

        _instance = this;

        poolManager = GetComponent<PoolManager>();
        updateManager = GetComponent<UpdateManager>();
        inputManager = GetComponent<InputManager>();
    }

    private void Start()
    {
        updateManager.AddToGameplayUpdate(this);
        inputManager.OnPause += TogglePause;
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
        updateManager.RemoveToGameplayUpdate(this);
    }
}

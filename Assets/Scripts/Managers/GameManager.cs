using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [ReadOnly] public InputManager inputManager;
    [ReadOnly] public UpdateManager updateManager;
    [ReadOnly] public PoolManager poolManager;

    private static GameManager _instance;
    private bool _pause = false;

    public static GameManager Instance => _instance;
    public static bool HasInstance => _instance != null;
    public PlayerModel Player { get; private set; }
    public bool Pause => _pause;

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

    public void SetPlayer(PlayerModel player)
    {
        Player = player;
    }
}

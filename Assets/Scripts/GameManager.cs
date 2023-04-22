using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UpdateManager updateManager;

    public static bool HasInstance => instance != null;
    public PlayerModel Player { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    public void SetPlayer(PlayerModel player)
    {
        Player = player;
    }
}

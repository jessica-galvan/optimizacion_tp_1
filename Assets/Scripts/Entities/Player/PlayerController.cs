using System.Collections.Generic;
using UnityEngine;

public enum PlayerEnums
{
    Idle,
    Moving,
}

public class PlayerController : MonoBehaviour, IUpdate
{
    [ReadOnly] public PlayerModel model;

    private FSM<PlayerEnums> fsm;

    public void Initialize()
    {
        model = GetComponent<PlayerModel>();
        model.Initialize();
        InitializeFSM();
        GameManager.Instance.updateManager.fixCustomUpdater.Add(this);
    }

    public void InitializeFSM()
    {
        fsm = new FSM<PlayerEnums>();

        var idle = new PlayerStateIdle<PlayerEnums>(model, fsm, PlayerEnums.Moving);
        var move = new PlayerStateMove<PlayerEnums>(model, fsm, PlayerEnums.Idle);

        idle.AddTransition(PlayerEnums.Moving, move);
        move.AddTransition(PlayerEnums.Idle, idle);

        fsm.SetInit(idle);
    }

    public void DoUpdate()
    {
        if (GameManager.Instance.Pause || !model.Alive || GameManager.Instance.Won) return;

        fsm.OnUpdate();
    }

    private void OnDestroy()
    {
        if (GameManager.HasInstance)
            GameManager.Instance.updateManager.fixCustomUpdater.Remove(this);
    }
}

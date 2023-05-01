using System.Collections.Generic;
using UnityEngine;

public enum PlayerEnums
{
    Idle,
    Moving,
}

public class PlayerController : MonoBehaviour, IUpdate
{
    public PlayerModel model;

    private FSM<PlayerEnums> fsm;

    public PlayerEnums CurrentState => fsm.CurrentState;

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

        var idle = new PlayerStateIdle<PlayerEnums>(PlayerEnums.Moving);
        var move = new PlayerStateMove<PlayerEnums>(PlayerEnums.Idle);

        idle.InitializeState(model, fsm);
        move.InitializeState(model, fsm);

        idle.AddTransition(PlayerEnums.Moving, move);
        move.AddTransition(PlayerEnums.Idle, idle);

        fsm.SetInit(idle);
    }

    public void DoUpdate()
    {
        if (GameManager.Instance.Pause) return;

        fsm.OnUpdate();
    }

    private void OnDestroy()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.updateManager.fixCustomUpdater.Remove(this);
        }
    }
}

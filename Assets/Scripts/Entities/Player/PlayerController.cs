using System.Collections.Generic;
using UnityEngine;

public enum PlayerEnums
{
    Idle,
    Running,
}

public class PlayerController : MonoBehaviour, IUpdate
{
    public PlayerModel model;
    private FSM<PlayerEnums> fsm;
    private List<PlayerStateBase<PlayerEnums>> states;

    public void Initialize()
    {
        model = GetComponentInChildren<PlayerModel>();
        model.Initialize();
        InitializeFSM();
        GameManager.Instance.updateManager.fixCustomUpdater.Add(this);
    }

    public void InitializeFSM()
    {
        //el player state base se crea para no tener que hacer siempre
        //la misma función y no tener que pasar siempre los mismos componentes
        //que son el model y el fsm.

        fsm = new FSM<PlayerEnums>();
        states = new List<PlayerStateBase<PlayerEnums>>();

        var idle = new PlayerStateIdle<PlayerEnums>(PlayerEnums.Running);
        var move = new PlayerStateMove<PlayerEnums>(PlayerEnums.Idle);

        idle.InitializeState(model, fsm);
        move.InitializeState(model, fsm);

        //si al idle le paso este input "playerEnums.Running" va a "move"
        idle.AddTransition(PlayerEnums.Running, move);

        //si al idle le paso este input "playerEnums.Idle" va a "idle"
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
            GameManager.Instance.updateManager.fixCustomUpdater.Remove(this);
    }
}

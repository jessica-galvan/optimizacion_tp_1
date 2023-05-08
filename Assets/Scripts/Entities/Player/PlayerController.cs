using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates
{
    Idle,
    Moving,
}

public class PlayerController : MonoBehaviour, IUpdate
{
    [ReadOnly] public PlayerModel model;

    private FSM<PlayerStates> fsm;

    public void Initialize()
    {
        model = GetComponent<PlayerModel>();
        model.Initialize();
        InitializeFSM();
        GameManager.Instance.updateManager.fixCustomUpdater.Add(this);
    }

    public void InitializeFSM()
    {
        fsm = new FSM<PlayerStates>();

        var idle = new PlayerStateIdle<PlayerStates>(model, fsm, PlayerStates.Moving);
        var move = new PlayerStateMove<PlayerStates>(model, fsm, PlayerStates.Idle);

        idle.AddTransition(PlayerStates.Moving, move);
        move.AddTransition(PlayerStates.Idle, idle);

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
        {
            GameManager.Instance.updateManager.fixCustomUpdater.Remove(this);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public enum EnemyStates
{
    Idle,
    Move,
}

public class EnemyController : EntityController, IPoolable, IUpdate
{
    public EnemyModel model;

    private FSM<EnemyStates> fsm;
    private Vector3 hidePoint;
    private INode root;
    private Dictionary<EnemyStates, EnemyBaseState<EnemyStates>> enemyStates = new Dictionary<EnemyStates, EnemyBaseState<EnemyStates>>();

    private void Start()
    {
        model = GetComponentInChildren<EnemyModel>();
        model.OnDie += OnDie;
        InitializeFSM();
    }

    public void InitializeFSM()
    {
        fsm = new FSM<EnemyStates>();

        var idle = new EnemyIdleState<EnemyStates>(EnemyStates.Move, ResetAction);
        var move = new EnemyMovingState<EnemyStates>(EnemyStates.Idle, ResetAction);

        idle.InitializeState(model, fsm);
        move.InitializeState(model, fsm);

        idle.AddTransition(EnemyStates.Move, move);
        move.AddTransition(EnemyStates.Idle, idle);

        fsm.SetInit(idle);
    }

    public void ResetAction()
    {
        fsm.Transition(GameManager.Instance.enemyManager.GetRandomAction());
    }

    public void DoUpdate()
    {
        if (GameManager.Instance.Pause) return;

        fsm.OnUpdate();
    }

    #region PoolManager
    public void Initialize(Vector3 hidePosition)
    {
        hidePoint = hidePosition;
    }

    public void Spawn(GridCell cell)
    {
        model.Spawn(cell);
    }

    public void ReturnToPool()
    {
        GameManager.Instance.updateManager.gameplayCustomUpdate.Remove(this);
        transform.position = hidePoint;
    }
    #endregion

    private void OnDie()
    {
        GameManager.Instance.poolManager.ReturnEnemy(this);
        GameManager.Instance.enemyManager.EnemyKilled();
    }
}

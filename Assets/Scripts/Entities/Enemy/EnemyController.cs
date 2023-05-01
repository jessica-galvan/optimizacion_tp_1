using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    Idle,
    Move,
}

public class EnemyController : EntityController, IPoolable, IUpdate
{
    [ReadOnly] public EnemyModel model;

    private FSM<EnemyStates> fsm;
    private Vector3 hidePoint;
    [ReadOnly] [SerializeField] private bool isActive;

    private void Start()
    {
        model = GetComponent<EnemyModel>();
        model.Initialize();
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

    public void Spawn(GridCell spawnPoint)
    {
        isActive = true;
        gameObject.SetActive(false);
        model.Spawn(spawnPoint);
    }

    public void ReturnToPool()
    {
        isActive = false;
        transform.position = hidePoint;
        gameObject.SetActive(false);
        GameManager.Instance.updateManager.gameplayCustomUpdate.Remove(this);
    }
    #endregion

    private void OnDie()
    {
        GameManager.Instance.poolManager.ReturnEnemy(this);
        GameManager.Instance.enemyManager.EnemyKilled();
    }
}

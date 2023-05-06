using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

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

    private void Awake()
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
        var newSate = GameManager.Instance.enemyManager.GetRandomAction();
        print("ResetAction newState " + newSate);
        fsm.Transition(newSate);
    }

    public void DoUpdate()
    {
        if (GameManager.Instance.Pause) return;

        fsm.OnUpdate();

        //SLICE frame, we don't need to check collisions every frame, so we can slice it and do it every other one to just be sure everything it's ok
        if (!model.enemyConfig.enemyColliderSlicesFrames || model.gameManager.enemyManager.currentTimeFrame == 0)
        {
            model.CheckCollisions();
        }
    }

    #region PoolManager
    public void Initialize(Vector3 hidePosition)
    {
        hidePoint = hidePosition;
    }

    public void Spawn(GridCell spawnPoint)
    {
        gameObject.SetActive(true);
        model.Spawn(spawnPoint);
        isActive = true;
        GameManager.Instance.updateManager.gameplayCustomUpdate.Add(this);
        ResetAction();
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

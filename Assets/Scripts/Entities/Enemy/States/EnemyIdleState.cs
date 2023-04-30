using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState<T> : EnemyBaseState<T>
{
    private T inputRunning;
    private float currentTime;
    private Action onEndActivityCallback;

    public EnemyIdleState(T myInputRunning, Action onEndActivityCallback)
    {
        inputRunning = myInputRunning;
        this.onEndActivityCallback = onEndActivityCallback;
    }

    public override void Awake()
    {
        base.Awake();
        currentTime = MiscUtils.RandomFloat(model.enemyConfig.minIdleWaitTime, model.enemyConfig.maxIdleWaitTime);
    }

    public override void Execute()
    {
        currentTime -= Time.deltaTime;

        if (currentTime < 0)
        {
            onEndActivityCallback();
        }
    }

    private void OnMove(Vector3 movement)
    {
        if (movement != Vector3.zero)
            fsm.Transition(inputRunning);
    }
}
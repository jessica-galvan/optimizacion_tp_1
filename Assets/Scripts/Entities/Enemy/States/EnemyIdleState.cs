using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdleState<T> : EnemyBaseState<T>
{
    private float currentTime;

    public EnemyIdleState(T transitionInput, Action onEndActivityCallback) : base(transitionInput, onEndActivityCallback)
    {

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
            Exit();
        }
    }

    private void Exit()
    {
        onEndActivityCallback();
    }
}
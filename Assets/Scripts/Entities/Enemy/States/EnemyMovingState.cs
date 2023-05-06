using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovingState<T> : EnemyBaseState<T>
{
    public EnemyMovingState(T transitionInput, Action onEndActivityCallback) : base(transitionInput, onEndActivityCallback)
    {

    }

    public override void Awake()
    {
        base.Awake();

        if (!model.HasTargetCell)
        {
            Exit();
            return;
        }
        Debug.Log("Init MovingState");
    }

    public override void Execute()
    {
        base.Execute();

        if (model.CanMoveFoward())
        {
            model.Move(model.CurrentDirection);

            if (model.HasTargetCell && model.HasArrivedToPlace())
            {
                Exit();
            }
        }
        else
        {
            Debug.Log("Collision detecter and exit execute");
            Exit();
        }
    }

    private void Exit()
    {
        model.CleanTargetCell();
        onEndActivityCallback();
    }

    public override void Sleep()
    {
        Debug.Log($"Exit MovingState");
    }
}

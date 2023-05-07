using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovingState<T> : EnemyBaseState<T>
{
    public EnemyMovingState(T transitionInput, Action onEndActivityCallback) : base(transitionInput, onEndActivityCallback)
    {

    }

    public override void Execute()
    {
        base.Execute();

        model.CheckWhereWeAre();

        if (model.CanMoveFoward(model.transform.forward))
        {
            model.Move(model.CurrentDirection);
        }
        else
        {
            Exit();
        }
    }

    private void Exit()
    {
        model.GetRandomDirection();
        onEndActivityCallback();
    }
}

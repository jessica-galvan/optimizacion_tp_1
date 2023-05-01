using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovingState<T> : EnemyBaseState<T>
{
    private Vector3 direction;

    public EnemyMovingState(T transitionInput, Action onEndActivityCallback) : base(transitionInput, onEndActivityCallback)
    {

    }

    public override void Awake()
    {
        base.Awake();

        //Get a posible direction that we can move. if nothin works, then set it back to stop moving. 
        var targetDirection = model.GetRandomDirection();
        if(targetDirection != null) //expected path, there should usually be at least ONE spawn open
        {
            direction = (targetDirection.spawnPoint.position - model.transform.position).normalized;
            direction = new Vector3(direction.x, 0, direction.z);
        }
        else
        {
            fsm.Transition(transitionInput);
        }
    }

    public override void Execute()
    {
        base.Execute();

        model.Move(direction);

        if (model.CheckWhereWeAre())
        {
            onEndActivityCallback();
        }
    }
}

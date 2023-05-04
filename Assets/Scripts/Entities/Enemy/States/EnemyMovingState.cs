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
        var targetPoint = model.GetRandomDirection(); //Here we already get the target cell and the target direction
        if (targetPoint != null) //expected path, there should usually be at least ONE spawn open
        {
            direction = (targetPoint.spawnPoint.position - model.transform.position).normalized;
            direction = new Vector3(direction.x, 0, direction.z);
            model.LookDirection(direction);
        }
        else
        {
            fsm.Transition(transitionInput);
        }
    }

    public override void Execute()
    {
        base.Execute();

        model.ShootingCooldown();

        if (model.CanMoveFoward())
        {
            model.Move(direction);

            if (model.HasArrivedToPlace())
            {
                onEndActivityCallback();
            }
        }
        else
        {
            fsm.Transition(transitionInput);
        }
    }
}

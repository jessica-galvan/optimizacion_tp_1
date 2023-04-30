using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovingState<T> : EnemyBaseState<T>
{
    private T inputRunning;
    private Action onEndActivityCallback;

    public EnemyMovingState(T myInputRunning, Action onEndActivityCallback)
    {
        inputRunning = myInputRunning;
        this.onEndActivityCallback = onEndActivityCallback;
    }

    public override void Awake()
    {
        base.Awake();
        //Get a posible direction that we can move. if nothin works, then set it back to stop moving. 
    }

    public override void Execute()
    {
        base.Execute();
        //TODO check if it's in the new area, call to random again
    }
}

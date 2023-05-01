using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState<T> : State<T>
{
    protected EnemyModel model;
    protected FSM<T> fsm;
    protected T transitionInput;
    protected Action onEndActivityCallback;

    public EnemyBaseState(T transitionInput, Action onEndActivityCallback)
    {
        this.transitionInput = transitionInput;
        this.onEndActivityCallback = onEndActivityCallback;
    }

    public void InitializeState(EnemyModel model, FSM<T> stateFSM)
    {
        this.model = model;
        fsm = stateFSM;
    }


}

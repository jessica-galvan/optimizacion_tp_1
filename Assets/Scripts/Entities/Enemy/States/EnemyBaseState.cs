using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState<T> : State<T>
{
    protected EnemyModel model;
    protected FSM<T> fsm;

    public void InitializeState(EnemyModel model, FSM<T> stateFSM)
    {
        this.model = model;
        fsm = stateFSM;
    }
}

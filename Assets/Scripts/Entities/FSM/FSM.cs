using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T>
{
    private IState<T> current;
    private bool hasState;

    public FSM()
    {
    }

    public FSM(IState<T> initialState)
    {
        SetInit(initialState);
    }

    public void SetInit(IState<T> initialState)
    {
        current = initialState;
        hasState = current != null;
        current.Awake();
    }

    public void OnUpdate()
    {
        if (hasState)
        {
            current.Execute();
        }
    }

    public void Transition(T input)
    {
        IState<T> newState = current.GetTransition(input);
        hasState = newState != null;
        if (!hasState) return;

        current.Sleep();
        current = newState;
        current.Awake();
    }
}

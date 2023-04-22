using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMove<T> : PlayerStateBase<T>
{
    T inputIdle;

    public PlayerStateMove(T myInputRunning)
    {
        inputIdle = myInputRunning;
    }

    public override void Awake()
    {
        base.Awake();
    }

    public override void Execute()
    {
        base.Execute();
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        if (h == 0 && v == 0)
        {
            fsm.Transition(inputIdle);
            return;
        }

        Vector3 direction = new Vector3(h, 0, v).normalized;

        model.Move(direction);
        model.LookDirection(direction);
    }

    public override void Sleep()
    {
        base.Sleep();
        model.Move(Vector3.zero);
    }
}

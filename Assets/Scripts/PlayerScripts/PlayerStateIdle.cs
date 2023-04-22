using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle<T> : PlayerStateBase<T>
{
    T inputRunning;

    public PlayerStateIdle(T myInputRunning)
    {
        inputRunning = myInputRunning;
    }

    public override void Execute()
    {
        base.Execute();
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            fsm.Transition(inputRunning);
        }
    }
}
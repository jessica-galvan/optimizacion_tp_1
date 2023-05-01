using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMove<T> : PlayerStateBase<T>
{
    private T inputIdle;
    private InputManager inputManager;
    private Vector3 previousDirection;

    public PlayerStateMove(T myInputRunning)
    {
        inputIdle = myInputRunning;
        inputManager = GameManager.Instance.inputManager;
    }

    public override void Awake()
    {
        base.Awake();

        inputManager.OnAttack += OnShoot;
        inputManager.OnMove += OnMove;
        inputManager.OnIdle += OnIdle;
    }

    public override void Execute()
    {
        base.Execute();
        inputManager.PlayerUpdate();
    }

    private void OnMove(Vector3 direction)
    {
        model.Move(direction);
        model.CheckWhereWeAre();
        if(previousDirection != direction)
        {
            previousDirection = direction;
            model.LookDirection(direction);
        }
    }

    private void OnIdle()
    {
        fsm.Transition(inputIdle);
    }

    private void OnShoot()
    {
        model.Shoot();
    }

    public override void Sleep()
    {
        base.Sleep();
        inputManager.OnAttack -= OnShoot;
        inputManager.OnMove -= OnMove;
    }
}

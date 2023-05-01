using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateIdle<T> : PlayerStateBase<T>
{
    public PlayerStateIdle(PlayerModel playerModel, FSM<T> playerFSM, T transitionInput) : base(playerModel, playerFSM, transitionInput)
    {

    }

    public override void Awake()
    {
        base.Awake();

        model.Idle();

        inputManager.OnAttack += OnShoot;
        inputManager.OnMove += OnMove;
    }

    public override void Execute()
    {
        base.Execute();
        inputManager.PlayerUpdate();
    }

    private void OnMove(Vector3 movement)
    {
        fsm.Transition(inputTransition);
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
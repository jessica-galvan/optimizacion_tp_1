using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMove<T> : PlayerStateBase<T>
{
    private Vector3 previousDireciton;

    public PlayerStateMove(PlayerModel playerModel, FSM<T> playerFSM, T transitionInput) : base(playerModel, playerFSM, transitionInput)
    {

    }

    public override void Awake()
    {
        base.Awake();

        inputManager.OnAttack += OnShoot;
        inputManager.OnMove += OnMove;
        inputManager.OnStopMoving += Idle;
    }

    public override void Execute()
    {
        base.Execute();
        inputManager.PlayerUpdate();
        model.ShootingCooldown();
    }

    private void OnMove(Vector3 direction)
    {
        model.CheckWhereWeAre();
        model.Move(direction);

        if(previousDireciton != direction)
        {
            previousDireciton = direction;
            model.LookDirection(direction);
        }
    }

    private void OnShoot()
    {
        model.Shoot();
    }

    private void Idle()
    {
        fsm.Transition(inputTransition);
    }

    public override void Sleep()
    {
        base.Sleep();
        inputManager.OnAttack -= OnShoot;
        inputManager.OnMove -= OnMove;
        inputManager.OnStopMoving -= Idle;
    }
}

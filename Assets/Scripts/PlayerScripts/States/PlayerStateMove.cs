using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMove<T> : PlayerStateBase<T>
{
    private T inputIdle;
    private PlayerModel playerModel;
    private InputManager inputManager;

    public PlayerStateMove(T myInputRunning, PlayerModel model)
    {
        inputIdle = myInputRunning;
        playerModel = model;
        inputManager = GameManager.Instance.inputManager;
    }

    public override void Awake()
    {
        base.Awake();

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
        if (movement == Vector3.zero)
        {
            fsm.Transition(inputIdle);
            return;
        }

        Vector3 direction = new Vector3(movement.x, 0, movement.z).normalized;
        model.Move(direction);
        model.LookDirection(direction);
    }

    private void OnShoot()
    {
        playerModel.Shoot();
    }

    public override void Sleep()
    {
        base.Sleep();
        inputManager.OnAttack -= OnShoot;
        inputManager.OnMove -= OnMove;
        model.Move(Vector3.zero);
    }
}

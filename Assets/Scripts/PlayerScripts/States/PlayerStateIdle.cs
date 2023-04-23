using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateIdle<T> : PlayerStateBase<T>
{
    private T inputRunning;
    private PlayerModel playerModel;
    private InputManager inputManager;

    public PlayerStateIdle(T myInputRunning, PlayerModel model)
    {
        inputRunning = myInputRunning;
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
        if(movement != Vector3.zero)
            fsm.Transition(inputRunning);
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
    }
}
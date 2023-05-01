using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateBase<T> : State<T>
{
    protected T inputTransition;
    protected PlayerModel model;
    protected FSM<T> fsm;
    protected InputManager inputManager;

    public PlayerStateBase(PlayerModel playerModel, FSM<T> playerFSM, T transitionInput)
    {
        model = playerModel;
        fsm = playerFSM;
        inputTransition = transitionInput;
        inputManager = GameManager.Instance.inputManager;
    }
}

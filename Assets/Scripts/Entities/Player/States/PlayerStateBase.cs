using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateBase<T> : State<T>
{
    //PROTECTED: publica para los que heredan,
    //privada para los que no.
    protected PlayerModel model;
    protected FSM<T> fsm;

    public void InitializeState(PlayerModel playerModel, FSM<T> playerFSM)
    {
        model = playerModel;
        fsm = playerFSM;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour, IUpdate
{
    #region KeyCodes
    public KeyCode attack = KeyCode.Space;
    public KeyCode pause = KeyCode.Escape;
    private const string HORIZONTAL_AXIS = "Horizontal";
    private const string VERTICAL_AXIS = "Vertical";
    #endregion

    #region Events
    public Action OnPause;
    public Action OnAttack;
    public Action<Vector3> OnMove;
    #endregion

    private bool previousMovingState = false;
    private GameManager gameManager;

    public bool IsMoving { get; private set; }

    #region Unity
    private void Awake()
    {
        gameManager = GameManager.Instance;
        gameManager.updateManager.fixCustomUpdater.Add(this);
    }

    public void DoUpdate()
    {
        if(!gameManager.Won)
            CheckPause();
    }

    public void PlayerUpdate()
    {
        if (!gameManager.Pause && gameManager.Player.Alive)
        {
            CheckAttack();
            CheckMovement();
        }
    }
    #endregion

    #region Private
    private void CheckMovement()
    {
        float horizontal = Input.GetAxisRaw(HORIZONTAL_AXIS);
        float vertical = Input.GetAxisRaw(VERTICAL_AXIS);

        bool verticalMovement = vertical != 0;
        bool horizontalMovement = horizontal != 0;

        IsMoving = horizontalMovement != verticalMovement;

        if(previousMovingState != IsMoving)
        {
            previousMovingState = IsMoving;

            if(!IsMoving)
                OnMove?.Invoke(new Vector3(0, 0, 0));
        }

        if(IsMoving)
            OnMove?.Invoke(new Vector3(horizontal, 0, vertical));
    }

    private void CheckAttack()
    {
        if (Input.GetKeyDown(attack))
        {
            OnAttack?.Invoke();
        }
    }
    private void CheckPause()
    {
        if (Input.GetKeyDown(pause))
        {
            OnPause?.Invoke();
        }
    }
    #endregion
}

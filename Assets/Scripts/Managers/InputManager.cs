using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour, IUpdate
{
    private const string HORIZONTAL_AXIS = "Horizontal";
    private const string VERTICAL_AXIS = "Vertical";


    public KeyCode attack = KeyCode.Space;
    public KeyCode pause = KeyCode.Escape;

    public Action OnPause;
    public Action OnAttack;
    public Action<Vector3> OnMove;
    public Action OnStopMoving;

    private bool previousMovingState = false;
    private GameManager gameManager;

    public bool IsMoving { get; private set; }

    public void Initialize()
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

            if (!IsMoving)
            {
                OnStopMoving?.Invoke();
            }
        }

        if (IsMoving)
        {
            OnMove?.Invoke(new Vector3(horizontal, 0, vertical));
        }
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

    private void OnDestroy()
    {
        gameManager.updateManager.fixCustomUpdater.Remove(this);
    }
}

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

    public bool IsMoving { get; private set; }

    #region Unity
    private void Awake()
    {
        GameManager.Instance.updateManager.AddToGameplayUpdate(this);
    }

    public void DoUpdate()
    {
        CheckPause();
    }

    public void PlayerUpdate()
    {
        if (!GameManager.Instance.Pause)
        {
            CheckAttack();
            CheckMovement();
        }
    }
    #endregion

    #region Private
    private void CheckMovement() //Moverlo para controllar en idle (chequeo si no me muevo) y en move (si me muevo) del player. 
    {
        float horizontal = Input.GetAxisRaw(HORIZONTAL_AXIS);
        float vertical = Input.GetAxisRaw(VERTICAL_AXIS);
        IsMoving = (vertical != 0 || horizontal != 0) ? true : false;
        OnMove?.Invoke(new Vector3(horizontal, 0, vertical));
    }
    private void CheckAttack()
    {
        if (Input.GetKeyDown(attack))
            OnAttack?.Invoke();
    }
    private void CheckPause()
    {
        if (Input.GetKeyDown(pause))
            OnPause?.Invoke();
    }
    #endregion
}

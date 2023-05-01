using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class EntityModel : MonoBehaviour, IDamagable
{
    [Header("References")]
    [SerializeField] private GameObject model;
    public MovementConfig movementConfig;
    public Transform firepoint;
    public BulletType bulletType;

    [Header("Movement")]
    public float speed;

    protected Rigidbody rb;
    protected GridCell currentCell;
    protected GridCell targetCell;
    protected GameManager gameManager;
    protected RaycastHit[] currentRaycastBuffer = new RaycastHit[5];
    [ReadOnly][SerializeField] protected Vector3 currentDirection;

    public Rigidbody RB => rb;

    public Action OnSpawned = delegate { };
    public Action OnDie = delegate { };

    public virtual void Initialize()
    {
        gameManager = GameManager.Instance;
    }

    public virtual void Spawn(GridCell spawnPoint)
    {
        currentCell = spawnPoint;
        transform.position = currentCell.spawnPoint.position;
        currentCell.SetOccupiedStatus(true, this);
        gameObject.SetActive(true);
        OnSpawned.Invoke();
    }

    public virtual void Shoot()
    {
        //TODO: add sound and feedback
        var bullet = GameManager.Instance.poolManager.GetBullet(BulletType.Player);
        bullet.SetTarget(firepoint, transform.forward);
    }

    public void Move(Vector3 direction)
    {
        if (GetNextCell(direction))
        {
            rb.velocity = direction * speed;
        }
    }

    public void Idle()
    {
        rb.velocity = Vector3.zero;
    }

    public void LookDirection(Vector3 dir)
    {
        dir.y = 0; //Sacar una vez que utilizemos Y
        transform.forward = dir;
    }

    public bool CanMoveFoward()
    {
        int hitCount = Physics.RaycastNonAlloc(new Ray(firepoint.position, transform.forward), currentRaycastBuffer, movementConfig.maxRayDistance, movementConfig.raycastDectection);
        bool canShoot = false;
        for (int i = 0; i < hitCount; i++)
        {
            //currentRaycastBuffer[i]; //TODO implemente non alloc raycast;
        }
        return canShoot;
    }

    public bool GetNextCell(Vector3 direction)
    {
        targetCell = gameManager.levelGrid.GetNextCell(currentCell, direction);

        bool isValid = currentCell != targetCell ? ValidCell(targetCell) : false;

        return isValid;
    }

    public void CheckWhereWeAre() //call only while in moving;
    {
        if( targetCell != null)
        {
            var distance = Vector3.SqrMagnitude(targetCell.spawnPoint.position - transform.position);
            if(distance <= gameManager.levelGrid.cellCenterDistance)
            {
                UpdateCurrentCellStatus(targetCell);
            }
        }
    }

    public virtual bool ValidCell(GridCell targetCell)
    {
        bool answer = false;
        if (currentCell != targetCell)
        {
            if (targetCell.IsOcupied && targetCell.Entity != null)
            {
                //TODO check if's not the player, then change direction, if it's player, stay in place and wait to be able to shoot
                //player should be able to move?
            }
            else
            {
                //TODO move towards that direction; when reached the cell then change status of old cell and new cell
            }
        }

        return answer;
    }

    public void UpdateCurrentCellStatus(GridCell gridCell)
    {
        if(currentCell != null)
        {
            currentCell.SetOccupiedStatus(false, this);
        }

        currentCell = gridCell;
        currentCell.SetOccupiedStatus(true, this);
    }

    public virtual void TakeDamage()
    {
        //TODO set visuals to off but still be there?
        //TODO do death feedback!
        OnDie.Invoke();
    }
}

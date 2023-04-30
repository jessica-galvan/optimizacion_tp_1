using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class EntityModel : MonoBehaviour, IDamagable
{
    [Header("References")]
    [SerializeField] private GameObject model;
    public Transform firepoint;
    public BulletType bulletType;

    [Header("Movement")]
    public float speed;

    protected Rigidbody rb;
    protected LevelGrid levelGrid;
    protected GridCell currentCell;
    protected GridCell targetCell;
    [ReadOnly][SerializeField] protected Vector3 currentDirection;

    public Rigidbody RB => rb;

    public Action OnSpawned = delegate { };
    public Action OnDie = delegate { };

    public virtual void Initialize()
    {
        levelGrid = GameManager.Instance.levelGrid;
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
        if (direction == Vector3.zero) return;

        if (GetNextCell(direction))
        {
            Vector3 directionSpeed = targetCell.spawnPoint.position * speed;
            directionSpeed.y = rb.velocity.y;
            directionSpeed.z = transform.position.z;
            rb.velocity = direction * speed;
        }
    }

    public void LookDirection(Vector3 dir)
    {
        if (dir == Vector3.zero) return;
        if (dir == currentDirection) return;
        dir.y = 0; //Sacar una vez que utilizemos Y
        model.transform.forward = dir;
    }

    public bool GetNextCell(Vector3 direction)
    {
        targetCell = levelGrid.GetNextCell(currentCell, direction);

        bool isValid = currentCell != targetCell ? ValidCell(targetCell) : false;

        return isValid;
    }

    public void CheckWhereWeAre() //call only while in moving;
    {
        if( targetCell != null)
        {
            var distance = Vector3.SqrMagnitude(targetCell.spawnPoint.position - transform.position);
            if(distance <= levelGrid.cellCenterDistance)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class EntityModel : MonoBehaviour, IDamagable
{
    [Header("References")]
    [SerializeField] private GameObject model;
    public EntityConfig entityConfig;
    public Transform firepoint;

    protected GameManager gameManager;
    protected Rigidbody rb;

    //Cell System && movement
    protected GridCell currentCell;
    protected GridCell targetCell;
    protected bool hasTargetCell;
    [ReadOnly][SerializeField] protected Vector3 currentDirection;
    protected RaycastHit[] currentRaycastBuffer = new RaycastHit[5];
    protected RaycastHit[] currentCollisionBuffer = new RaycastHit[5];
    protected Coroutine collisionCoroutine = null;

    //Shooting
    protected bool canShoot = true;
    protected float cooldownShootTimer = 0f;

    public bool CanCheckCollision { get; private set; }

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
        if (!canShoot) return;
        var bullet = GameManager.Instance.poolManager.GetBullet(entityConfig.bulletType);
        bullet.SetTarget(firepoint, transform.forward);
        canShoot = false;
        cooldownShootTimer = 0f;
    }

    public virtual void Move(Vector3 direction)
    {
        //if (!CanMoveFoward()) return;
        rb.velocity = direction * entityConfig.speed;
    }

    public void Idle()
    {
        rb.velocity = Vector3.zero;
    }

    public void LookDirection(Vector3 dir)
    {
        transform.forward = dir;
    }

    public bool CanMoveFoward()
    {
        int hitCount = Physics.RaycastNonAlloc(new Ray(firepoint.position, transform.forward), currentRaycastBuffer, entityConfig.maxRayDistance, entityConfig.raycastDectection);
        return hitCount == 0;
    }

    public void CheckCollisions()
    {
        int hitCount = Physics.SphereCastNonAlloc(transform.position, entityConfig.collisionDistance, transform.forward, currentCollisionBuffer, entityConfig.collisionDectection);

        if(hitCount == 0)
        {
            StartCollisionTimer();
        }
        else
        {
            TakeDamage();
        }

    }

    public void StartCollisionTimer()
    {
        if(collisionCoroutine != null)
        {
            StopCoroutine(collisionCoroutine);
        }

        CanCheckCollision = false;
        gameManager.PausableTimerCoroutine(entityConfig.collisionCheckTimer, EndAction);

        void EndAction()
        {
            collisionCoroutine = null;
            CanCheckCollision = true;
        }
    }

    public void GetNextCell(Vector3 direction)
    {
        var auxCell = gameManager.levelGrid.GetNextCell(currentCell, direction);

        if (ValidCell(auxCell))
        {
            hasTargetCell = true;
            targetCell = auxCell;
        }
        else
        {
            hasTargetCell = false;
            targetCell = null;
        }
    }

    public void CheckWhereWeAre(Vector3 direction) //call only while in moving;
    {
        if(hasTargetCell)
        {
            var distance = Vector3.SqrMagnitude(targetCell.spawnPoint.position - transform.position);
            if(distance <= gameManager.levelGrid.cellCenterDistance)
            {
                GetNextCell(direction);
                UpdateCurrentCellStatus(targetCell);
            }
        }
    }

    public void ShootingCooldown()
    {
        if (canShoot) return;

        cooldownShootTimer += Time.deltaTime;

        if(cooldownShootTimer >= entityConfig.cooldownShooting)
        {
            canShoot = true;
        }
    }

    public virtual bool ValidCell(GridCell targetCell)
    {
        bool answer = true;

        //only if it's not our cell somehow, the target cell is occupied and the entity is null
        //then we return it's not a valid cell, as it has a wall on it
        if (currentCell != targetCell && targetCell.IsOcupied && targetCell.Entity == null)
        {
            answer = false;
        }

        //is it expected path if we check in the importance order? like first that it's not the same,
        //then that it's occupied and lastly it is has an owner?

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

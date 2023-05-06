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

    [PortLabelHidden] public GameManager gameManager;
    protected Rigidbody rb;

    //Cell System && movement
    protected GridCell currentCell;
    protected GridCell targetCell;
    public bool HasTargetCell { get; protected set; }
    protected Vector3 currentDirection;
    protected RaycastHit[] currentRaycastBuffer = new RaycastHit[5];

    //Shooting
    protected bool canShoot = true;
    protected float cooldownShootTimer = 0f;

    public Action OnSpawned = delegate { };
    public Action OnDie = delegate { };

    public virtual void Initialize()
    {
        rb = gameObject.GetComponentInParent<Rigidbody>();
        gameManager = GameManager.Instance;
    }

    public virtual void Spawn(GridCell spawnPoint)
    {
        UpdateCurrentCellStatus(spawnPoint);
        transform.position = currentCell.spawnPoint.position;
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
        transform.position += direction * entityConfig.speed * Time.deltaTime;
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


    public void GetNextCell(Vector3 direction)
    {
        var auxCell = gameManager.levelGrid.GetNextCell(currentCell, direction);

        if (ValidCell(auxCell))
        {
            HasTargetCell = true;
            targetCell = auxCell;
        }
        else
        {
            CleanTargetCell();
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
        if (targetCell == null || (currentCell != targetCell && targetCell.IsOcupied && targetCell.Entity == null))
        {
            answer = false;
        }

        //is it expected path if we check in the importance order? like first that it's not the same,
        //then that it's occupied and lastly it is has an owner?

        return answer;
    }

    public void CleanTargetCell()
    {
        targetCell = null;
        HasTargetCell = false;
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

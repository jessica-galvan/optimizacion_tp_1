using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EntityModel : MonoBehaviour, IDamagable
{
    [Header("References")]
    [HideInInspector] public GameManager gameManager;
    [SerializeField] protected EntityConfig entityConfig;
    [SerializeField] protected Transform firepoint;

    //Cell System && movement
    [ReadOnly] [SerializeField] protected GridCell currentCell;
    [ReadOnly] [SerializeField] protected Vector3 currentDirection;
    protected Rigidbody rb;
    protected Vector2Int currentGridPos;
    protected RaycastHit[] currentRaycastBuffer = new RaycastHit[5];

    //Shooting
    protected bool canShoot = true;
    protected float cooldownShootTimer = 0f;
    protected float currentStuckCounter = 0f;

    public Action OnSpawned = delegate { };
    public Action OnDie = delegate { };

    public virtual void Initialize()
    {
        rb = gameObject.GetComponentInParent<Rigidbody>();
        gameManager = GameManager.Instance;
    }

    public virtual void Spawn(GridCell spawnPoint)
    {
        AudioManager.instance.PlaySFXSound(AudioManager.instance.soundReferences.spawnSound);
        UpdateCurrentCellStatus(spawnPoint);
        transform.position = currentCell.spawnPoint.position;
        LookDirection(transform.forward);
        gameObject.SetActive(true);
        OnSpawned.Invoke();
    }

    public virtual void Shoot()
    {
        var bullet = GameManager.Instance.poolManager.GetBullet(entityConfig.bulletType);
        bullet.SetTarget(firepoint, transform.forward);
        canShoot = false;
        cooldownShootTimer = 0f;
    }

    public virtual void Move(Vector3 direction)
    {
        rb.velocity = direction * entityConfig.speed;
        //transform.position += direction * entityConfig.speed * Time.deltaTime;
    }

    public void Idle()
    {
        rb.velocity = Vector3.zero;
    }

    public void LookDirection(Vector3 dir)
    {
        currentDirection = dir;
        transform.forward = dir;
    }

    public virtual bool CanMoveFoward(Vector3 direction)
    {
        int hitCount = Physics.RaycastNonAlloc(new Ray(firepoint.position, direction), currentRaycastBuffer, entityConfig.maxRayDistance, entityConfig.raycastDectection);
        return hitCount == 0;
    }

    public virtual void CheckWhereWeAre() //call only while in moving;
    {
        var newGridPos = gameManager.levelGrid.GetGridPosFromWorld(transform.position);

        if (currentGridPos != newGridPos)
        {
            currentGridPos = newGridPos;
            var newCell = gameManager.levelGrid.GetGridFromVector2Int(newGridPos);
            UpdateCurrentCellStatus(newCell);
        }
        else
        {
            CheckIfStuck(newGridPos);
        }
    }

    public virtual void CheckIfStuck(Vector2Int currentPos)
    {

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

    public void UpdateCurrentCellStatus(GridCell gridCell)
    {
        if(currentCell != null)
        {
            currentCell.SetOccupiedStatus(false, this);
        }

        currentStuckCounter = 0;
        currentCell = gridCell;
        currentCell.SetOccupiedStatus(true, this);
    }

    public virtual void TakeDamage()
    {
        var particle = gameManager.poolManager.GethParticle(ParticleController.ParticleType.Death);
        particle.Spawn(transform);
        OnDie.Invoke();
    }
}

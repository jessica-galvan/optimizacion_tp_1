using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityModel : MonoBehaviour, IDamagable
{
    [Header("References")]
    [SerializeField] private GameObject model;
    public Transform firepoint;
    public BulletType bulletType;

    [Header("Movement")]
    public float rotationSpeed;
    public float speed;

    protected Rigidbody rb;
    protected LevelGrid levelGrid;
    protected GridCell currentCell;
    protected GridCell targetCell;

    public Vector3 GetForward => transform.forward;
    public float GetSpeed => rb.velocity.magnitude;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        levelGrid = GameManager.Instance.levelGrid;
    }


    public virtual void Shoot()
    {
        //TODO: add negative sound and feedback
        var bullet = GameManager.Instance.poolManager.GetBullet(isPlayer: true);
        bullet.SetTarget(firepoint, transform.forward);
    }

    public void Move(Vector3 direction) //el modelo solo recibe la dirección
    {
        Vector3 directionSpeed = direction * speed;
        directionSpeed.y = rb.velocity.y;
        rb.velocity = direction * speed;
    }

    public void LookDirection(Vector3 dir)
    {
        if (dir == Vector3.zero) return;
        dir.y = 0; //Sacar una vez que utilizemos Y
        model.transform.forward = Vector3.RotateTowards(model.transform.forward, dir, Time.deltaTime * rotationSpeed, 0f);
    }

    public bool GetNextCell(Vector3 direction)
    {
        direction = Vector3.Normalize(direction);
        var targetCell = levelGrid.GetNextCell(currentCell, direction);

        bool answer = false;

        if(currentCell != targetCell)
        {
            answer = ValidateCell(targetCell);
        }
        else
        {
            answer = false;
        }

        return answer;
    }

    public virtual bool ValidateCell(GridCell targetCell)
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

    public virtual void Die()
    {
        //TODO add effects
        //TODO in case of player respawn
        //TODO in case of enemy, move back to enemy pool
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyModel : EntityModel
{
    [Header("Enemy")]
    public EnemyConfig enemyConfig;

    private PlayerModel player;
    private RaycastHit[] currentCollisionBuffer = new RaycastHit[2];

    //as they are instantiated AFTER the player, then we can do this in the awake
    public override void Initialize()
    {
        base.Initialize();
        player = GameManager.Instance.Player;
    }

    public GridCell GetRandomDirection(bool skipCurrentDirection = false)
    {
        GridCell newDirection = null;
        //TODO do a random instead of a for. and while no newDirection is settle, do a new one up until max number of times has been done?
        for (int i = 0; i < enemyConfig.posibleDirectionsCount; i++)
        {
            if (skipCurrentDirection && enemyConfig.posibleDirections[i] == currentDirection) continue;
            var auxCell = gameManager.levelGrid.GetNextCell(currentCell, enemyConfig.posibleDirections[i]);

            if (auxCell.IsOcupied) continue;
            targetCell = auxCell;
            newDirection = targetCell;
            break;
        }

        return newDirection;
    }

    public bool HasArrivedToPlace()
    {
        bool isOnCenter = false;
        if (hasTargetCell)
        {
            var distance = Vector3.SqrMagnitude(targetCell.spawnPoint.position - transform.position);
            if (distance <= gameManager.levelGrid.cellCenterDistance)
            {
                if (distance <= entityConfig.distanceFromCenter)
                {
                    hasTargetCell = false;
                    targetCell = null;
                    isOnCenter = true;
                }
                UpdateCurrentCellStatus(targetCell);
            }
        }
        return isOnCenter;
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
    }

    public void CheckCollisions()
    {
        Vector3 distance = (player.transform.position + enemyConfig.offset) - transform.position;
        distance = new Vector3(Mathf.Abs(distance.x), 0, Mathf.Abs(distance.z));
        bool xDistance = distance.x <= gameManager.enemyManager.enemyConfig.precollisionBox.x;
        bool zDistance = distance.z <= gameManager.enemyManager.enemyConfig.precollisionBox.z;

        if (xDistance && zDistance)
        {
            int hitCount = Physics.BoxCastNonAlloc(transform.position, enemyConfig.collisionBox, transform.forward, currentCollisionBuffer, Quaternion.identity,enemyConfig.preCollisionDetection, enemyConfig.collisionDectection);

            if (hitCount > 0) //the only one that will appear here is the player? 
            {
                for (int i = 0; i < currentCollisionBuffer.Length; i++)
                {
                    player.TakeDamage();
                }
                TakeDamage();
            }
        }

        //Although radious is easier, the game is by cells, so box is better. 
        //float distance = (player.transform.position - transform.position).magnitude;
        //if (distance < enemyConfig.preCollisionDetection) //If too far... don't do anything
        //{
        //    int hitCount = Physics.SphereCastNonAlloc(transform.position, enemyConfig.collisionRadious, transform.forward, currentCollisionBuffer, enemyConfig.collisionDectection);

        //    if (hitCount == 0)
        //    {
        //        TakeDamage();
        //    }
        //}
    }

#if UNITY_EDITOR
    public void DrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, enemyConfig.preCollisionDetection);
        Gizmos.DrawWireCube(transform.position + enemyConfig.offset, enemyConfig.precollisionBox);
        
        Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(transform.position, enemyConfig.collisionRadious);
        Gizmos.DrawWireCube(transform.position + enemyConfig.offset, enemyConfig.collisionBox);

    }
#endif
}

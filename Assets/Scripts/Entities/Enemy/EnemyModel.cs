using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyModel : EntityModel
{
    [Header("Enemy")]
    public EnemyConfig enemyConfig;

    private PlayerModel player;
    private RaycastHit[] currentPlayerCollisionBuffer = new RaycastHit[2];

    public override void Initialize()
    {
        base.Initialize();
        player = GameManager.Instance.Player;
    }

    public GridCell GetRandomDirection()
    {
        GridCell newDirection = null;

        bool foundViableDirection = false;

        var directions = enemyConfig.posibleDirections;

        while (!foundViableDirection && directions.Count > 0)
        {
            int randomPosition = MiscUtils.RandomInt(0, directions.Count - 1 );

            var auxCell = gameManager.levelGrid.GetNextCell(currentCell, directions[randomPosition]);
            if (auxCell == null ||auxCell.IsOcupied)
            {
                directions.RemoveAt(randomPosition);
                continue;
            }

            targetCell = auxCell;
            newDirection = targetCell;
            HasTargetCell = true;
            foundViableDirection = true;
            currentDirection = directions[randomPosition];
            print($"New Direction : {currentDirection} and RadomNumber = {randomPosition}");
        }

        return newDirection;
    }

    public bool HasArrivedToPlace()
    {
        bool isOnCenter = false;
        var distance = Vector3.SqrMagnitude(targetCell.spawnPoint.position - transform.position);
        if (distance <= gameManager.levelGrid.cellCenterDistance)
        {
            if (distance <= entityConfig.distanceFromCenter)
            {
                isOnCenter = true;
                UpdateCurrentCellStatus(targetCell);
                CleanTargetCell();
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
            int hitCount = Physics.BoxCastNonAlloc(transform.position, enemyConfig.collisionBox, transform.forward, currentPlayerCollisionBuffer, Quaternion.identity,enemyConfig.preCollisionDetection, enemyConfig.collisionDectection);

            if (hitCount > 0) //the only one that will appear here is the player? 
            {
                for (int i = 0; i < currentPlayerCollisionBuffer.Length; i++)
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

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
    public LayerMask floorLayer;

    private PlayerModel player;
    private RaycastHit[] currentPlayerCollisionBuffer = new RaycastHit[2];
    private RaycastHit[] locationRaycast = new RaycastHit[1];

    public Vector3 CurrentDirection => currentDirection;

    public override void Initialize()
    {
        base.Initialize();
        player = GameManager.Instance.Player;
    }

    public void GetRandomDirection()
    {
        bool foundViableDirection = false;
        var directions = new List<Vector3>(enemyConfig.posibleDirections);

        while (!foundViableDirection && directions.Count > 0)
        {
            int randomPosition = MiscUtils.RandomInt(0, directions.Count - 1 );

            var auxCell = gameManager.levelGrid.GetNextCell(currentCell, directions[randomPosition]);
            if (auxCell == null ||auxCell.IsOcupied)
            {
                directions.RemoveAt(randomPosition);
                continue;
            }

            SetNewTarget(auxCell, directions[randomPosition]);
            foundViableDirection = true;
        }
    }

    public void ChangeDirection()
    {
        for (int i = 0; i < enemyConfig.posibleDirections.Count; i++)
        {
            if (enemyConfig.posibleDirections[i] == currentDirection) continue;


            

            var auxCell = gameManager.levelGrid.GetNextCell(currentCell, enemyConfig.posibleDirections[i]);
            if (auxCell == null || auxCell.IsOcupied) continue;
            SetNewTarget(auxCell, enemyConfig.posibleDirections[i]);
            UpdateCurrentCellStatus(targetCell);
            LookDirection(enemyConfig.posibleDirections[i]);
            break;
        }
    }

    public bool HasArrivedToPlace()
    {
        bool isOnCenter = false;

        if (HasTargetCell)
        {
            var aux = targetCell.spawnPoint.position - transform.position;
            var distance = Vector3.SqrMagnitude(aux);

            if (distance <= entityConfig.distanceFromCenter)
            {
                print($"{gameObject.name} has arrived to the center of cell {targetCell.gameObject.name}");
                isOnCenter = true;
                UpdateCurrentCellStatus(targetCell);

            }
        }

        return isOnCenter;
    }

    public void UpdateDirection()
    {
        CleanTargetCell();
        GetRandomDirection();
        if (HasTargetCell)
        {
            LookDirection(currentDirection);
        }
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

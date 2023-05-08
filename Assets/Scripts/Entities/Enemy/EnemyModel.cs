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
    public Transform[] wallCollider = new Transform[2]; 

    private PlayerModel player;
    private RaycastHit[] currentPlayerCollisionBuffer = new RaycastHit[1];

    public Vector3 CurrentDirection => currentDirection;

    public override void Initialize()
    {
        base.Initialize();
        player = GameManager.Instance.Player;
    }

    public override void CheckWhereWeAre()
    {
        if (gameManager.enemyManager.currentTimeFrameCheckLocation != 0) return;
        base.CheckWhereWeAre();
    }

    public override bool CanMoveFoward(Vector3 direction)
    {
        bool canMove = true;
        for (int i = 0; i < wallCollider.Length; i++)
        {
            int hitCount = Physics.RaycastNonAlloc(new Ray(wallCollider[i].position, direction), currentRaycastBuffer, entityConfig.maxRayDistance, entityConfig.raycastDectection);
            canMove &= hitCount == 0;
        }
        return canMove;
    }

    public override void CheckIfStuck(Vector2Int currentPos)
    {
        currentStuckCounter++;
        if(currentStuckCounter >= 10)
        {
            GetRandomDirection();
        }
    }

    public void GetRandomDirection()
    {
        bool foundViableDirection = false;

        var newGridPos = gameManager.levelGrid.GetGridPosFromWorld(transform.position);
        var current = gameManager.levelGrid.GetGridFromVector2Int(newGridPos);

        var directions = new List<Vector3>(enemyConfig.posibleDirections);

        while (!foundViableDirection && directions.Count > 0)
        {
            int randomPosition = MiscUtils.RandomInt(0, directions.Count - 1 );

            if (currentDirection == directions[randomPosition]) //if we are changing directions, we are not going to check the current one. 
            {
                directions.RemoveAt(randomPosition);
                continue;
            }

            var auxCell = gameManager.levelGrid.GetNextCell(current, directions[randomPosition]);
            if (auxCell == null ||auxCell.IsOcupied)
            {
                directions.RemoveAt(randomPosition);
                continue;
            }

            LookDirection(directions[randomPosition]);
            foundViableDirection = true;
        }
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
                player.TakeDamage();
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
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, enemyConfig.preCollisionDetection);
        Gizmos.DrawWireCube(transform.position + enemyConfig.offset, enemyConfig.precollisionBox);
        
        Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(transform.position, enemyConfig.collisionRadious);
        Gizmos.DrawWireCube(transform.position + enemyConfig.offset, enemyConfig.collisionBox);

        Gizmos.color = Color.blue;
        for (int i = 0; i < wallCollider.Length; i++)
        {
            Gizmos.DrawLine(wallCollider[i].position, wallCollider[i].position + (transform.forward * entityConfig.maxRayDistance));
        }
    }
#endif
}

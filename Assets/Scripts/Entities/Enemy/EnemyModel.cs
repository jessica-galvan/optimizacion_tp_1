using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyModel : EntityModel
{
    public EnemyConfig enemyConfig;

    public bool CanAttack { get; set; }

    private RaycastHit[] currentRaycastBuffer = new RaycastHit[5];

    public bool CheckWhereWeAre() //call only while in moving;
    {
        bool isOnCenter = false;
        if (targetCell != null)
        {
            var distance = Vector3.SqrMagnitude(targetCell.spawnPoint.position - transform.position);
            if (distance <= gameManager.levelGrid.cellCenterDistance)
            {
                if(distance <= enemyConfig.distanceFromCenter)
                {
                    isOnCenter = true;
                }
                UpdateCurrentCellStatus(targetCell);
            }
        }
        return isOnCenter;
    }

    public GridCell GetRandomDirection(bool skipCurrentDirection = false)
    {
        GridCell newDirection = null;
        //TODO do a random instead of a for. and while no newDirection is settle, do a new one up until max number of times has been done?
        for (int i = 0; i < enemyConfig.posibleDirectionsCount; i++)
        {
            if (skipCurrentDirection && enemyConfig.posibleDirections[i] == currentDirection) continue;

            if (GetNextCell(enemyConfig.posibleDirections[i]))
            {
                newDirection = targetCell;
            }
        }

        return newDirection;
    }

    public override bool ValidCell(GridCell targetCell)
    {
        bool answer = true;

        if (currentCell != targetCell)
        {
            if (targetCell.IsOcupied)
            {
                if (targetCell.Entity != null) //EXPECTED PAD: most of the cells that are occupied are walls. So they don't have an entity on them. Thus we first check that one
                {
                    answer = false;
                }
                else
                {
                    answer = targetCell != gameManager.Player.CurrentCell; // if it's not the player THEEN it's a enemy and thus we don't go that way
                }
            }
        }

        return answer;
    }

    protected IEnumerator AttackTimer(float time)
    {
        yield return new WaitForSeconds(time);
        CanAttack = true;
        //print("Can attack again!");
    }

    public bool CanShoot()
    {
        int hitCount = Physics.RaycastNonAlloc(new Ray(firepoint.position, transform.forward), currentRaycastBuffer, enemyConfig.maxRayDistance, enemyConfig.raycastDectection);
        bool canShoot = false;
        for (int i = 0; i < hitCount; i++)
        {
            //currentRaycastBuffer[i];
        }
        return canShoot;
    }
}

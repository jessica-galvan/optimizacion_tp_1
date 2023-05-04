using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyModel : EntityModel
{
    [Header("Enemy")]
    public EnemyConfig enemyConfig;

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
}

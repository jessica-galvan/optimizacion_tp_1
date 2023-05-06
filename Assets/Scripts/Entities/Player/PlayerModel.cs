using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : EntityModel
{
    public Vector3 spawnLookDirection = new Vector3(0, 0, 1);
    public bool Alive { get; private set; }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Spawn(GridCell spawn)
    {
        base.Spawn(spawn);
        Alive = true;
        LookDirection(spawnLookDirection);
    }

    public void CheckWhereWeAre(Vector3 direction) //call only while in moving;
    {
        if (hasTargetCell)
        {
            var distance = Vector3.SqrMagnitude(targetCell.spawnPoint.position - transform.position);
            if (distance <= gameManager.levelGrid.cellCenterDistance)
            {
                GetNextCell(direction);
                UpdateCurrentCellStatus(targetCell);
            }
        }
    }

    public override void TakeDamage()
    {
        Alive = false;
        base.TakeDamage();
        gameObject.SetActive(false);
    }
}


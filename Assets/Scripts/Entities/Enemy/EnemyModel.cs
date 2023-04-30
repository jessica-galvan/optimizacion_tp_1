using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyModel : EntityModel
{
    public override bool ValidateCell(GridCell targetCell)
    {
        bool answer = true;

        if (currentCell != targetCell)
        {
            if (targetCell.IsOcupied && targetCell.Entity != null)
            {
                answer = false;
            }
        }

        return answer;
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        //TODO return to pool!
    }
}

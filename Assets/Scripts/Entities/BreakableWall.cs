using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IDamagable
{
    public GridCell cell;
    public List<GameObject> wallParts = new List<GameObject>();

    public void TakeDamage()
    {
        if (wallParts.Count == 0) return;

        int randomPosition = MiscUtils.RandomInt(0, wallParts.Count - 1);
        wallParts[randomPosition].SetActive(false);
        wallParts.RemoveAt(randomPosition);

        if (wallParts.Count == 0)
        {
            cell.SetOccupiedStatus(false);
            gameObject.SetActive(false);
        }
    }
}
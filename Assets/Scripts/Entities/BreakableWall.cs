using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IDamagable
{
    public GridCell cell;
    public GameObject[] wallParts = new GameObject[0];
    [SerializeField][ReadOnly] private int points = 4;

    public void TakeDamage()
    {
        points -= 1;
        wallParts[points].SetActive(false);

        if(points <= 0)
        {
            cell.SetOccupiedStatus(false);
            gameObject.SetActive(false);
        }
    }
}

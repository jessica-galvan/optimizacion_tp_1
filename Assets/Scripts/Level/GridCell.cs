using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public enum Type
    {
        Empty,
        BorderWall,
        UnbreakableWall,
        DestroyableWall,
        PlayerSpawnPoint,
        EnemySpawnPoint
    }

    [Header("References")]
    public GameObject breakableWall;
    public GameObject unbreakableWall;
    public GameObject borderWall;

    [Header("Settings")]
    public Transform spawnPoint;
    public Type cellType;

    private int posX;
    private int posY;

    public int X => posX;
    public int Y => posY;

    public EntityModel Entity { get; private set; }
    public bool IsOcupied { get; private set; }

    public void SetVisuals()
    {
        //TODO: change visuals of the cell depending on what type it is
        switch (cellType)
        {
            case Type.BorderWall:
                IsOcupied = true;
                borderWall.SetActive(true);
                break;
            case Type.UnbreakableWall:
                unbreakableWall.SetActive(true);
                IsOcupied = true;
                break;
            case Type.DestroyableWall:
                breakableWall.SetActive(true);
                IsOcupied = true;
                break;
            case Type.Empty:
            case Type.PlayerSpawnPoint:
            case Type.EnemySpawnPoint:
                breakableWall.SetActive(false);
                borderWall.SetActive(false);
                unbreakableWall.SetActive(false);
                IsOcupied = false;
                break;
        }
    }

    public void SetPosition(int x, int y)
    {
        posX = x;
        posY = y;
    }

    public Vector2Int GetPosition()
    {
        return new Vector2Int(posX, posY);
    }

    public void SetOccupiedStatus(bool newStatus, EntityModel entity = null)
    {
        IsOcupied = newStatus;
        if(entity != null && Entity == entity)
        {
            Entity = null;
        }
    }
}

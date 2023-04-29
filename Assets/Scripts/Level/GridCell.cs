using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public enum Type
    {
        Empty,
        UnbreakableWall,
        DestroyableWall,
        PlayerSpawnPoint,
        EnemySpawnPoint
    }

    [Header("Settings")]
    public Type cellType;

    private int posX;
    private int posY;

    public int X => posX;
    public int Y => posY;

    public bool IsOcupied { get; private set; }

    public void SetVisuals()
    {
        switch (cellType)
        {
            case Type.UnbreakableWall:
                break;
            case Type.DestroyableWall:
                break;
            case Type.PlayerSpawnPoint:
            case Type.EnemySpawnPoint:
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

    public void SetOccupiedStatus(bool newStatus)
    {
        IsOcupied = newStatus;
    }
}

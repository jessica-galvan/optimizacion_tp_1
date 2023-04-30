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

    public SpriteRenderer spawnVisual;
    public Color playerSpawn = Color.green;
    public Color enemySpawn = Color.red;

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
        breakableWall.SetActive(false);
        borderWall.SetActive(false);
        unbreakableWall.SetActive(false);
        ShowSpawnCell(false);

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
                IsOcupied = false;
                break;
            case Type.PlayerSpawnPoint:
                spawnVisual.color = playerSpawn;
                ShowSpawnCell(true);
                IsOcupied = false;
                break;
            case Type.EnemySpawnPoint:
                spawnVisual.color = Color.red;
                ShowSpawnCell(true);
                IsOcupied = false;
                break;
        }

    }

    public void ShowSpawnCell(bool value)
    {
        spawnVisual.enabled = value;
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

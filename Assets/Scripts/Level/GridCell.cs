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
    public BreakableWall breakableWall;
    public GameObject unbreakableWall;
    public GameObject borderWall;

    public SpriteRenderer spawnVisual;
    public Color playerSpawn = Color.green;
    public Color enemySpawn = Color.red;

    [Header("Settings")]
    public Transform spawnPoint;
    public Type cellType;

    [Header("Info")]
    [SerializeField] [ReadOnly] private bool ocupied = false;
    [SerializeField][ReadOnly] private int posX;
    [SerializeField][ReadOnly] private int posY;

    public int X => posX;
    public int Y => posY;

    public EntityModel Entity { get; private set; }
    public bool IsOcupied => ocupied;

    public void SetVisuals()
    {
        breakableWall.gameObject.SetActive(false);
        borderWall.SetActive(false);
        unbreakableWall.SetActive(false);
        ShowSpawnCell(false);
        SetOccupiedStatus(false);
        string extraName = string.Empty;

        switch (cellType)
        {
            case Type.BorderWall:
                borderWall.SetActive(true);
                SetOccupiedStatus(true);
                extraName = $"_{cellType}";
                break;
            case Type.UnbreakableWall:
                unbreakableWall.SetActive(true);
                SetOccupiedStatus(true);
                break;
            case Type.DestroyableWall:
                breakableWall.gameObject.SetActive(true);
                SetOccupiedStatus(true);
                break;
            case Type.PlayerSpawnPoint:
                spawnVisual.color = playerSpawn;
                ShowSpawnCell(true);
                extraName = $"_{cellType}";
                break;
            case Type.EnemySpawnPoint:
                spawnVisual.color = Color.red;
                ShowSpawnCell(true);
                extraName = $"_{cellType}";
                break;
            case Type.Empty:
                break;
        }

        gameObject.name = $"Grid({X},{Y}){extraName}";
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
        ocupied = newStatus;
        if(entity != null && Entity == entity)
        {
            Entity = null;
        }
    }
}

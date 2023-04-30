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
    [SerializeField] [ReadOnly] private bool ocupied = false;

    private int posX;
    private int posY;

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

        switch (cellType)
        {
            case Type.BorderWall:
                borderWall.SetActive(true);
                SetOccupiedStatus(true);
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
                break;
            case Type.EnemySpawnPoint:
                spawnVisual.color = Color.red;
                ShowSpawnCell(true);
                break;
            case Type.Empty:
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
        ocupied = newStatus;
        if(entity != null && Entity == entity)
        {
            Entity = null;
        }
    }
}

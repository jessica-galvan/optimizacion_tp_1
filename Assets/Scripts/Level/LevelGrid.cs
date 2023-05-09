using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    public PrefabsReferences prefabsReferences;
    public Vector2Int gridSize = new Vector2Int(12,12);

    [SerializeField] private float gridSpaceSize = 5f;

    private GridCell[,] levelGrid;

    [Header("Level Spawn Points")]
    public float cellCenterDistance = 1.5f;
    public List<GridCell> gridList;
    public List<Vector2Int> gridPos = new List<Vector2Int>();
    public GridCell playerSpawnPoint;
    public List<GridCell> enemySpawnPoints = new List<GridCell>();

    [HideInInspector] public int extraBorderCells = 2;
    private Vector2Int realGridSize = new Vector2Int(14, 14);

    public void Initialize()
    {
        realGridSize = new Vector2Int(gridSize.x + extraBorderCells, gridSize.y + extraBorderCells);
        ResetPosInfo();

        //HEAP OPTIMIZATION: now that we finish reconstructing the dictionary, we can clear both lists to liberate some memory
        gridList.Clear();
        gridPos.Clear();
    }

#if UNITY_EDITOR
    public void CreateGrid()
    {
        if(prefabsReferences != null & prefabsReferences.gridCellPrefab == null)
        {
            Debug.LogError("Error: Either PrefabReferences or Grid Cell Prefab is missing");
            return;
        }

        //now let's make a grid. We add an extra grid because we need a unbreakable wall. Two sides.
        int gridYSize = gridSize.y + extraBorderCells;
        int gridXSize = gridSize.x + extraBorderCells;
        for (int y = 0; y < gridYSize; y++)
        {
            for (int x = 0; x < gridXSize; x++)
            {
                //create a new GridCell for each space;
                GridCell gridCell = PrefabUtility.InstantiatePrefab(prefabsReferences.gridCellPrefab) as GridCell;
                gridCell.transform.SetPositionAndRotation(new Vector3(x * gridSpaceSize, 0, y * gridSpaceSize), Quaternion.identity);
                gridCell.transform.SetParent(transform);
                gridCell.SetPosition(x, y);

                if(x == 0 || y == 0 || x > gridSize.x  || y > gridSize.y)
                {
                    gridCell.cellType = GridCell.Type.BorderWall;
                    gridCell.SetVisuals();
                }

                gridList.Add(gridCell);
                gridPos.Add(new Vector2Int(x, y));
            }
        }
    }

    public void ClearGrid()
    {
        playerSpawnPoint = null;
        enemySpawnPoints.Clear();
    }

    public void ClearSettings()
    {
        ClearGrid();
        for (int i = gridList.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(gridList[i].gameObject);
        }
        gridList.Clear();
        gridPos.Clear();
    }

    public void SaveAndValidateSetUp()
    {
        //first let's clear the original info as the set up might have changed
        ClearGrid();
        gridPos.Clear();

        var maxX = gridSize.x + extraBorderCells;
        var maxY = gridSize.y + extraBorderCells;

        //now we run all the matrix to get the new spawning points
        for (int i = 0; i < gridList.Count; i++)
        {
            Vector2Int pos = CalculateGridPos(i, maxX, maxY);
            gridList[i].SetPosition(pos.x, pos.y);
            gridPos.Add(pos);
            SetData(gridList[i]);
        }

        //and now lets validate that there is at least one player spawn point and one enemy spawn point
        if (playerSpawnPoint == null)
        {
            Debug.LogError("There is no player spawning point in this level");
        }

        if (enemySpawnPoints.Count == 0)
        {
            Debug.LogError("There is no enemy spawning point in this level");
        }
    }

#endif

    public void ResetPosInfo()
    {
        var maxX = gridSize.x + extraBorderCells;
        var maxY = gridSize.y + extraBorderCells;
        levelGrid = new GridCell[maxX, maxY];
        for (int i = 0; i < gridList.Count; i++)
        {
            var pos = gridPos[i];
            bool occupied = gridList[i].cellType != GridCell.Type.Empty && gridList[i].cellType != GridCell.Type.EnemySpawnPoint; //player spot is set later to be occupied anyway
            gridList[i].SetOccupiedStatus(occupied);
            gridList[i].SetPosition(pos.x, pos.y);
            levelGrid[pos.x, pos.y] = gridList[i];
        }
    }

    public Vector2Int CalculateGridPos(int currentCell, int maxX, int maxY)
    {
        int auxY = currentCell / maxY;
        int ypos = Mathf.FloorToInt(auxY);
        int xpos = Mathf.Abs(currentCell - (ypos * maxY));
        return new Vector2Int(xpos, ypos);
    }

    public void SetData(GridCell currentGrid)
    {
        currentGrid.SetVisuals();
        if (currentGrid.cellType == GridCell.Type.EnemySpawnPoint)
        {
            enemySpawnPoints.Add(currentGrid);
        }
        else if (currentGrid.cellType == GridCell.Type.PlayerSpawnPoint)
        {
            if (playerSpawnPoint != null)
            {
                Debug.LogError("There should only be one player spawning point");
                return;
            }

            playerSpawnPoint = currentGrid;
        }
    }

    public Vector2Int GetGridPosFromWorld(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / gridSpaceSize);
        x = Mathf.Clamp(x, 0, realGridSize.x);

        int y = Mathf.RoundToInt(worldPos.z / gridSpaceSize);
        y = Mathf.Clamp(y, 0, realGridSize.y);

        return new Vector2Int(x, y);
    }

    public Vector3 GetWorldPosFromGrid(Vector2Int gridPos)
    {
        float x = gridPos.x * gridSpaceSize;
        float y = gridPos.y * gridSpaceSize;

        return new Vector3(x, 0, y);
    }

    public GridCell GetGridFromVector2Int(Vector2Int gridPos)
    {
        return levelGrid[gridPos.x, gridPos.y];
    }

    public GridCell GetNextCell(GridCell currentCell, Vector3 direction)
    {
        int xPos = direction.x != 0 ? (int)Mathf.Clamp(currentCell.X + direction.x, 0, realGridSize.x) : currentCell.X;
        int yPos = direction.z != 0 ? (int)Mathf.Clamp(currentCell.Y + direction.z, 0, realGridSize.y) : currentCell.Y;
        var cell = levelGrid[xPos, yPos];

        return cell;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    public PrefabsReferences prefabsReferences;
    [SerializeField] private Vector2Int gridSize = new Vector2Int(12,12);

    [SerializeField] private float gridSpaceSize = 5f;

    private GridCell[,] levelGrid;

    [Header("Level Spawn Points")]
    public List<GridCell> gridList;
    public GridCell playerSpawnPoint;
    public List<GridCell> enemySpawnPoints = new List<GridCell>();

    public void ReGenerateMatrix()
    {
        levelGrid = new GridCell[gridSize.x, gridSize.y];
        for (int i = 0; i < gridList.Count; i++)
        {
            levelGrid[gridList[i].X, gridList[i].Y] = gridList[i];
        }
    }

    public void CreateGrid()
    {
        if(prefabsReferences != null & prefabsReferences.gridCellPrefab == null)
        {
            Debug.LogError("Error: Either PrefabReferences or Grid Cell Prefab is missing");
            return;
        }

        //now let's make a grid. We add an extra grid because we need a unbreakable wall
        for (int y = 0; y <= gridSize.y; y++)
        {
            for (int x = 0; x <= gridSize.x; x++)
            {
                //create a new GridCell for each space;
                GridCell gridCell = PrefabUtility.InstantiatePrefab(prefabsReferences.gridCellPrefab) as GridCell;
                gridCell.transform.SetPositionAndRotation(new Vector3(x * gridSpaceSize, 0, y * gridSpaceSize), Quaternion.identity);
                gridCell.transform.SetParent(transform);
                gridCell.SetPosition(x, y);
                gridCell.gameObject.name = $"Grid({x},{y})";

                if(x == 0 || y == 0 || x == gridSize.x || y == gridSize.y)
                {
                    gridCell.gameObject.name += $"_{GridCell.Type.BorderWall}";
                    gridCell.cellType = GridCell.Type.BorderWall;
                    gridCell.SetVisuals();
                }

                gridList.Add(gridCell);
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
            DestroyImmediate(gridList[i]); //TODO find why it's not really working?
        }
        Debug.Log("Clearing might not destroy the items currently, if that's the case, select all grid cells and erase them by hand");
        gridList.Clear();
    }

    public void SaveAndValidateSetUp()
    {
        //first let's clear the original info as the set up might have changed
        ClearGrid();

        //now we run all the matrix to get the new spawning points
        for (int i = 0; i < gridList.Count; i++)
        {
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

    private void SetData(GridCell currentGrid)
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
        int x = Mathf.FloorToInt(worldPos.x / gridSpaceSize);
        int y = Mathf.FloorToInt(worldPos.z / gridSpaceSize);

        x = Mathf.Clamp(x, 0, gridSize.x);
        y = Mathf.Clamp(y, 0, gridSize.y);

        return new Vector2Int(x, y);
    }

    public Vector3 GetWorldPosFromGrid(Vector2Int gridPos)
    {
        float x = gridPos.x * gridSpaceSize;
        float y = gridPos.y * gridSpaceSize;

        return new Vector3(x, 0, y);
    }

    public GridCell GetNextCell(GridCell currentCell, Vector2 direction)
    {
        int xPos = direction.x != 0 ? (int)Mathf.Clamp(currentCell.X + direction.x, 0, gridSize.x) : currentCell.X;
        int yPos = direction.x != 0 ? (int)Mathf.Clamp(currentCell.Y + direction.y, 0, gridSize.y) : currentCell.Y;
        return levelGrid[xPos, yPos];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize = new Vector2Int(12,12);
    [SerializeField] private GridCell gridCellPrefab;

    private float gridSpaceSize = 5f;
    private GridCell[,] gameGrid;

    private List<GridCell> enemySpawnPoints = new List<GridCell>();
    private GridCell playerSpawnPoint;

    public void CreateGrid()
    {
        gameGrid = new GridCell[gridSize.y, gridSize.x];

        if(gridCellPrefab == null)
        {
            Debug.LogError("Error: Grid Cell Prefab missing");
            return;
        }

        //now let's make a grid
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                //create a new GridCell for each space;
                gameGrid[x, y] = Instantiate(gridCellPrefab, new Vector3(x * gridSpaceSize, y * gridSpaceSize), Quaternion.identity);
                gameGrid[x, y].transform.SetParent(transform);
                gameGrid[x, y].SetPosition(x, y);
                gameGrid[x, y].gameObject.name = $"Grid({x},{y})";
            }
        }
    }

    public void SaveAndValidateSetUp()
    {
        //first let's clear the original info as the set up might have changed
        playerSpawnPoint = null;
        enemySpawnPoints.Clear();

        //now we run all the matrix to get the new spawning points
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                var currentGrid = gameGrid[x, y];

                currentGrid.SetVisuals();
                if (currentGrid.cellType == GridCell.Type.EnemySpawnPoint)
                {
                    enemySpawnPoints.Add(currentGrid);
                }
                else if (currentGrid.cellType == GridCell.Type.PlayerSpawnPoint)
                {
                    if(playerSpawnPoint != null)
                    {
                        Debug.LogError("There should only be one player spawning point");
                        continue;
                    }

                    playerSpawnPoint = currentGrid;
                }
            }
        }

        //and now lets validate that there is at least one player spawn point and one enemy spawn point
        if(playerSpawnPoint == null)
        {
            Debug.LogError("There is no player spawning point in this level");
        }

        if (enemySpawnPoints.Count == 0)
        {
            Debug.LogError("There is no enemy spawning point in this level");
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
}

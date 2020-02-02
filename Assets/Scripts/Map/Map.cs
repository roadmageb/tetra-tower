using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Start is called before the first frame update
    public static int count = 0;

    public static int gridWidth = 10;
    public static int gridHeight = 20;
    public static Transform[,] grid = new Transform[gridWidth, gridHeight];
    GameObject tetSpawn;
    GameObject pistonSpawner;
    PistonSpawner ps;

    // Start is called before the first frame update
    void Start()
    {
        tetSpawn = GameObject.Find("TetrominoSpawnerObj");
        pistonSpawner = GameObject.Find("PistonSpawnerObj");
        ps = pistonSpawner.GetComponent<PistonSpawner>();
        ps.spawnNth(0);
        ps.spawnNth(2);
        ps.spawnNth(4);
        ps.spawnNth(6);
        ps.spawnNth(8);
        ps.spawnNth(9);
        ps.spawnNth(11);
        SpawnNextTetromino();
    }

    public bool isFullRowAt(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    public void DeleteMinoAt(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public void MoveRowDown(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void MoveAllRowsDown(int y)
    {
        for (int i = y; i < gridHeight; ++i)
        {
            MoveRowDown(i);
        }
    }

    public void DeleteRow()
    {
        for (int y = 0; y < gridHeight; ++y)
        {
            if (isFullRowAt(y))
            {
                DeleteMinoAt(y);

                MoveAllRowsDown(y + 1);

                --y;
            }
            else
            {
                //Debug.Log($"{y} is not full\n");
            }
        }
    }

    public void UpdateGrid(Tetromino tetromino)
    {
        for (int y = 0; y < gridHeight; ++y)
        {
            for (int x = 0; x < gridWidth; ++x)
            {
                if (grid[x, y] != null)
                {
                    if (grid[x, y].parent == tetromino.transform)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }

        foreach (Transform mino in tetromino.transform)
        {
            Vector2 pos = Round(mino.position);

            if (pos.y < gridHeight)
            {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }

    public Transform GetTransformAtGridPosition(Vector3 pos)
    {
        if (pos.y > gridHeight - 1)
        {
            return null;
        }
        else
        {
            return grid[(int)pos.x, (int)pos.y];
        }
    }

    public void SpawnNextTetromino()
    {
        tetSpawn.GetComponent<TetrominoSpawner>().spawnShuffled();
    }

    public bool CheckIsInsideGrid(Vector3 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }

    public Vector3 Round(Vector3 pos)
    {
        return new Vector3((int)Mathf.Round(pos.x), (int)Mathf.Round(pos.y), 0);
    }

    public void SpawnPistonsAtNthRow(int n)
    {
        pistonSpawner.GetComponent<PistonSpawner>().spawnNth(n);

    }
}

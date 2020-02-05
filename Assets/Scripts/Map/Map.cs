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
    TetrominoSpawner tetSpawner;
    PistonSpawner pistSpawner;
    RowRemover rowRemover;

    // Start is called before the first frame update
    void Start()
    {
        var tetSpawnerObj = GameObject.Find("TetrominoSpawnerObj");
        tetSpawner = tetSpawnerObj.GetComponent<TetrominoSpawner>();

        var pistonSpawnerObj = GameObject.Find("PistonSpawnerObj");
        pistSpawner = pistonSpawnerObj.GetComponent<PistonSpawner>();

        var rowRemoverObj = GameObject.Find("RowRemoverObj");
        rowRemover = rowRemoverObj.GetComponent<RowRemover>();

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
            //Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public void MoveRowDown(int y, int num)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - num] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - num].position += new Vector3(0, -num, 0);
            }
        }
    }

    public void MoveAllRowsDown(int from, int num)
    {
        for (int i = from; i < gridHeight; ++i)
        {
            MoveRowDown(i, num);
        }
    }

    IEnumerator waitAndDelete(bool[] bitmap)
    {
        int start = -1;
        int count = 0;
        for (int i = 0; i < bitmap.Length; ++i)
        {
            if (bitmap[i]) {
                start = i;
                count++;
                pistSpawner.spawnNth(i);
            }
        }
        yield return new WaitForSeconds(14);
        MoveAllRowsDown(start, count);
    }

    public void RemoveRowsIfFull()
    {
        bool[] fullRowBitmap = new bool[gridHeight];
        int fullRowCount = 0;
        for (int y = 0; y < gridHeight; ++y)
        {
            if (isFullRowAt(y))
            {
                fullRowCount++;
                fullRowBitmap[y] = true;
                DeleteMinoAt(y);
            }
        }

        if (fullRowCount > 0)
        {
            StartCoroutine(waitAndDelete(fullRowBitmap));
        }
    }

    public void DeleteRow()
    {
        bool[] fullRowBitmap = new bool[gridHeight];
        int fullRowCount = 0;
        for (int y = 0; y < gridHeight; ++y)
        {
            if (isFullRowAt(y))
            {
                fullRowCount++;
                fullRowBitmap[y] = true;
                DeleteMinoAt(y);
                MoveAllRowsDown(y + 1, 1);
                //rowRemover.RemoveRow(y);
                --y;
            }
            else
            {
                //Debug.Log($"{y} is not full\n");
            }
        }
        if (fullRowCount > 0)
        {
            StartCoroutine(waitAndDelete(fullRowBitmap));
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
        tetSpawner.spawnShuffled();
    }

    public bool CheckIsInsideGrid(Vector3 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }

    public Vector3 Round(Vector3 pos)
    {
        return new Vector3((int)Mathf.Round(pos.x), (int)Mathf.Round(pos.y), 0);
    }
}

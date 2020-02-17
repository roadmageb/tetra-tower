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
    public static GridUtils gridUtils = new GridUtils();
    TetrominoSpawner tetSpawner;
    PistonSpawner pistonSpawner;
    RowSlider rowSlider;

    // Start is called before the first frame update
    void Start()
    {
        var tetSpawnerObj = GameObject.Find("TetrominoSpawnerObj");
        tetSpawner = tetSpawnerObj.GetComponent<TetrominoSpawner>();

        var pistonSpawnerObj = GameObject.Find("PistonSpawnerObj");
        pistonSpawner= pistonSpawnerObj.GetComponent<PistonSpawner>();

        gridUtils.Initialize(grid);

        rowSlider = new GameObject().AddComponent<RowSlider>();
        rowSlider.Initialize(gridUtils);

        SpawnNextTetromino();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            reportError();
        }
    }

    void reportError()
    {
        printGrid();
        Debug.Log(rowSlider.coroutineCount);
    }

    public void DestroyRow(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public void DestroyRowsIfFull(bool[] isFull)
    {
        for (int i = 0; i < isFull.Length; ++i)
        {
            if (isFull[i])
            {
                DestroyRow(i);
            }
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
                // replaced by rowSlider
                //grid[x, y - num].position += new Vector3(0, -num, 0);
            }
        }
    }

    public void MoveAllRowsDown(bool[] isFull, int[] shiftAmount)
    {
        for (int i = 1; i < gridHeight; ++i)
        {
            if (!isFull[i]) {
                MoveRowDown(i, -shiftAmount[i]);
            }
        }
    }

    IEnumerator waitAndDelete(bool[] isFull)
    {
        pistonSpawner.spawnIfFull(isFull);

        yield return null;
        while (Piston.pistonCount != 0)
        {
            yield return null;
        }

        DestroyRowsIfFull(isFull);
        rowSlider.slideDown();

        yield return null; // required for waiting one frame so that coroutineCount can increase.
        while (rowSlider.coroutineCount != 0)
        {
            yield return null;
        }
        MoveAllRowsDown(isFull, gridUtils.shiftDown);
    }


    public void RemoveRowsIfFull()
    {
        gridUtils.isFullUpdate();
        gridUtils.shiftAmountUpdate();

        if (gridUtils.fullRowCount > 0)
        {
            StartCoroutine(waitAndDelete(gridUtils.isFull));
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
        //printGrid();
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

    public void printGrid()
    {
        string gridStr = "";
        for( int y = gridHeight - 1; y >= 0; --y)
        {
            for (int x = 0; x < gridWidth; ++x)
            {
                if (grid[x, y] == null)
                {
                    gridStr += "-";
                }
                else
                {
                    gridStr += "*";
                }
            }
            gridStr += '\n';
        }

        Debug.Log(gridStr);
    }
}

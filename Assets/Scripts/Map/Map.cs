using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

public class Map : MonoBehaviour
{
    public GameObject tetrominoSpawnerPrefab;
    public GameObject pistonSpawnerPrefab;
    public GameObject debugMapPrefab;

    public TetrominoSpawner tetrominoSpawner;
    public PistonSpawner pistonSpawner;
    public DebugMap debugMap;

    public static int count = 0;

    public const int gridWidth = 10;
    public const int gridHeight = 20;
    public Transform[,] grid;
    public GridUtils gridUtils = new GridUtils();
    public RowSlider rowSlider;

    public Vector3Int basePosition;

    public bool[] isFull;
    public bool[] isRowEmpty;
    public int[] shiftAmount;

    public Tetromino currentTetromino;

    public bool inputLock;

    public int scaleFactor;
    public Vector3Int scaleVector;

    public GameObject pistonMask;

    public Camera mainCamera;

    public RowDestroyer rowDestroyer;

    void Start()
    {
        scaleFactor = 3;
        scaleVector = Vector3Int.one * (scaleFactor - 1);
        transform.localScale += scaleVector;

        transform.position = new Vector3Int(1, -10 * scaleFactor, 0);

        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        mainCamera.orthographicSize = 40;

        basePosition = Vector3Utils.ToVector3Int(transform.position) + new Vector3Int(scaleFactor, scaleFactor, 0);

        tetrominoSpawner = Instantiate(tetrominoSpawnerPrefab).GetComponent<TetrominoSpawner>();
        tetrominoSpawner.Initialize(this);

        pistonSpawner = Instantiate(pistonSpawnerPrefab).GetComponent<PistonSpawner>();
        pistonSpawner.Initialize(this);

        pistonMask = GameObject.Find("PistonMask");
        pistonMask.transform.position = transform.position + scaleFactor * new Vector3(5.5f, 10.5f, 0);
        pistonMask.transform.localScale += scaleVector;

        debugMap = GameObject.Find("DebugMap").GetComponent<DebugMap>();

        grid = new Transform[gridWidth, gridHeight];

        gridUtils.Initialize(grid);

        rowSlider = new GameObject().AddComponent<RowSlider>();
        rowSlider.name = "RowSlider";
        rowSlider.Initialize(this);

        SpawnNextTetromino();

        rowDestroyer = new GameObject().AddComponent<RowDestroyer>();
        rowDestroyer.Initialize(this);
    }

    void DestroyRow(int y)
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


    void MoveRowDown(int y, int num)
    {
        if (num == 0)
        {
            return; // do nothing
        }

        for (int x = 0; x < gridWidth; ++x)
        {
            if (grid[x, y] != null)
            {
                // for delayed transform shift
                var mino = grid[x, y].gameObject.GetComponent<Mino>();
                mino.slideDestination = y - num;

                grid[x, y - num] = grid[x, y];
                grid[x, y] = null;

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

    IEnumerator DebugDelete(bool[] isFull)
    {

        pistonSpawner.spawnIfFull(isFull);
        yield return null;
        while (Piston.pistonCount != 0)
        {
            yield return null; 
        }

        inputLock = true;
        rowDestroyer.RequestDestroyRows();
        //yield return new WaitForSeconds(5);


        while (!rowDestroyer.destroyFinished)
        {
            yield return null;
        }
        rowDestroyer.requestCount--;
        if (rowDestroyer.requestCount != 0)
        {
            yield break;
        }
        else
        {
            rowDestroyer.destroyFinished = false;
        }
        gridUtils.IsRowEmptyUpdate();

        Debug.Log("move row down Finished!");
        rowSlider.slideDown();
        yield return null;
        while (rowSlider.coroutineCount != 0)
        {
            Debug.Log("coroutineCount = " + rowSlider.coroutineCount);
            yield return null;
        }
        Debug.Log("rowslider Finished!");
        Debug.Log("lock release");
        inputLock = false;
    }

    public void RemoveRowsIfFull()
    {

        gridUtils.isFullUpdate();
        gridUtils.shiftAmountUpdate();

        isFull = gridUtils.isFull;
        shiftAmount = gridUtils.shiftDown;
        //isRowEmpty = gridUtils.isRowEmpty;

        if (gridUtils.fullRowCount > 0)
        {
            //StartCoroutine(WaitAndDelete(gridUtils.isFull));
            StartCoroutine(DebugDelete(gridUtils.isFull));
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

        foreach (Transform child in tetromino.transform)
        {
            var mino = child.gameObject.GetComponent<Mino>();
            //Vector3 pos = Vector3Utils.Map(RelativePosition(mino.position), Mathf.Round);
            var pos = mino.GetGridPosition();

            if (pos.y < gridHeight)
            {
                grid[pos.x, pos.y] = child;
            }
        }
        //printGrid();
    }

    public Transform GetTransformAtGridPosition(Vector3Int pos)
    {
        if (pos.y > gridHeight - 1)
        {
            throw new System.Exception("y too high");
            //return null;
        }
        else
        {
            return grid[pos.x, pos.y];
        }
    }

    public void SpawnNextTetromino()
    {
        tetrominoSpawner.spawnShuffled();
    }

    public bool CheckIsInsideGrid(Vector3Int pos)
    {
        return (pos.x >= 0 && pos.x < gridWidth && pos.y >= 0);
    }

} 
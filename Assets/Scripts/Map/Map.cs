using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Linq;

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
    //public RowSlider rowSlider;

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

    public bool tetrominoFalling;

    public int coroutineCount;

    public Vector3[] rowPosition;

    public bool isFalling;

    void Start()
    {
        scaleFactor = 16;
        scaleVector = Vector3Int.one * (scaleFactor - 1);
        transform.localScale += scaleVector;

        transform.position = new Vector3Int(1, -10 * scaleFactor, 0);

        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        mainCamera.orthographicSize = 12 * scaleFactor;

        basePosition = Vector3Utils.ToVector3Int(transform.position) + new Vector3Int(scaleFactor, scaleFactor, 0);

        tetrominoSpawner = Instantiate(tetrominoSpawnerPrefab).GetComponent<TetrominoSpawner>();
        tetrominoSpawner.Initialize(this);

        pistonSpawner = Instantiate(pistonSpawnerPrefab).GetComponent<PistonSpawner>();
        pistonSpawner.Initialize(this);

        pistonMask = GameObject.Find("PistonMask");
        pistonMask.transform.position = transform.position + scaleFactor * new Vector3(5.5f, 10.5f, 0);
        pistonMask.transform.localScale += scaleVector;

        debugMap = GameObject.Find("DebugMap").GetComponent<DebugMap>();
        debugMap.transform.position = scaleFactor * debugMap.transform.position;
        debugMap.transform.localScale += scaleVector;

        grid = new Transform[gridWidth, gridHeight];

        gridUtils.Initialize(grid);

        //rowSlider = new GameObject().AddComponent<RowSlider>();
        //rowSlider.name = "RowSlider";
        //rowSlider.Initialize(this);

        SpawnNextTetromino();

        rowDestroyer = new GameObject().AddComponent<RowDestroyer>();
        rowDestroyer.Initialize(this);

        tetrominoFalling = false;

        coroutineCount = 0;

        rowPosition = new Vector3[Map.gridHeight];
        ResetRowPosition();
    }

    void ResetRowPosition()
    {
        for (int i = 0; i < Map.gridHeight; ++i)
        {
            rowPosition[i] = basePosition + scaleFactor * new Vector3(0, i, 0);
        }
    }




    IEnumerator DebugDelete(bool[] isFull)
    {

        while (gridIsFalling)
        {
            yield return null;
        }

        RowSlider rowSlider = new GameObject().AddComponent<RowSlider>();


        while (tetrominoFalling)
        {
            yield return null;
        }

        var pistonSet = pistonSpawner.spawnIfFull(isFull);
        while (!pistonSet.FinishSet())
        {
            yield return null; 
        }

        inputLock = true;
        var gridCopy = gridUtils.GridShallowCopy();
        var gridCopyUtils = new GridUtils();
        gridCopyUtils.Initialize(gridCopy);
        rowSlider.Initialize(this, gridCopyUtils);

        rowDestroyer.RequestDestroyRows(pistonSpawner, pistonSet);

        while (!rowDestroyer.destroyFinished)
        {
            yield return null;
        }

        inputLock = false;

        gridIsFalling = true;

        rowSlider.SlideDown(pistonSet);
        Debug.Log("move row down Finished!");

        yield return null;
        while (rowSlider.coroutineCount != 0)
        {
            Debug.Log("coroutineCount = " + rowSlider.coroutineCount);
            yield return null;
        }

        gridIsFalling = false;

        Debug.Log("rowslider Finished!");
        Debug.Log("lock release");
    }

    public void RemoveRowsIfFull(Tetromino tetromino)
    {
        bool[] rows = new bool[Map.gridHeight];

        foreach (Mino mino in tetromino.GetComponentsInChildren<Mino>())
        {
            var pos = mino.GetGridPosition();
            rows[pos.y] = true;
        }

        gridUtils.isFullUpdate();
        gridUtils.shiftAmountUpdate();

        isFull = gridUtils.isFull;
        shiftAmount = gridUtils.shiftDown;
        //isRowEmpty = gridUtils.isRowEmpty;


        for (int i = 0; i < rows.Length; ++i)
        {
            if (rows[i] && isFull[i])
            {
                StartCoroutine(DebugDelete(gridUtils.isFull));
                return;
            }
        }
    }


    public void UpdateGrid(Tetromino tetromino)
    {

        foreach (Transform child in tetromino.transform)
        {
            var mino = child.gameObject.GetComponent<Mino>();
            var pos = mino.GetGridPosition();
            mino.slideDestination = pos.y;

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
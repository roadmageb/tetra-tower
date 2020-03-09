using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Linq;

public class Map : MonoBehaviour
{
    public Gravity tetrominoGravity;
    public Gravity gridGravity;

    public GameObject tetrominoSpawnerPrefab;
    public GameObject pistonSpawnerPrefab;
    public GameObject debugMapPrefab;
    public GameObject playerPrefab;

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

    public Tetromino currentTetromino;

    public bool inputLock;

    public int scaleFactor;
    public Vector3Int scaleVector;

    public GameObject pistonMask;

    public Camera mainCamera;

    //public RowDestroyer rowDestroyer;

    public bool tetrominoFalling;

    public Vector3[] rowPosition;

    public bool gridIsFalling;

    void Awake()
    {

        tetrominoGravity = new GameObject().AddComponent<Gravity>();
        tetrominoGravity.name = "TetrominoGravity";

        gridGravity = new GameObject().AddComponent<Gravity>();
        gridGravity.name = "GridGravity";


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

        rowPosition = new Vector3[Map.gridHeight];
        ResetRowPosition();

        currentTetromino.ImmediateFall(playerPrefab);

    }

    void ResetRowPosition()
    {
        for (int i = 0; i < Map.gridHeight; ++i)
        {
            rowPosition[i] = basePosition + scaleFactor * new Vector3(0, i, 0);
        }
    }




    IEnumerator DebugDelete(bool[] isFull, int[] shiftAmount)
    {

        while (gridIsFalling)
        {
            yield return null;
        }

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

        while (tetrominoFalling)
        {
            yield return null;
        }

        var gridCopy = gridUtils.GridShallowCopy(); // must be below pistonSpawner. Otherwise new minos not updated
        var gridCopyUtils = new GridUtils();
        gridCopyUtils.Initialize(gridCopy);

        var isFullCopy = gridCopyUtils.MakeIsFull();
        var isEmptyCopy = gridCopyUtils.MakeIsEmpty();
        var shiftAmountCopy = gridCopyUtils.MakeShiftAmount(isFullCopy, isEmptyCopy);

        RowDestroyer rowDestroyer = new GameObject().AddComponent<RowDestroyer>();
        rowDestroyer.Initialize(this, gridCopyUtils);
        rowDestroyer.RequestDestroyRows(isFull, shiftAmountCopy, pistonSet); // must be shiftAmountCopy. Otherwise new tetromino not move down.

        gridIsFalling = true;
        RowSlider rowSlider = new GameObject().AddComponent<RowSlider>();
        rowSlider.Initialize(this, gridCopyUtils);
        rowSlider.SlideDown(isEmptyCopy, pistonSet);


        while (rowSlider.coroutineCount != 0)
        {
            Debug.Log("coroutineCount = " + rowSlider.coroutineCount);
            yield return null;
        }
        inputLock = false;

        gridIsFalling = false;

        Debug.Log("rowslider Finished!");
        Debug.Log("lock release");
    }

    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            for (int i = 0; i < Map.gridWidth; ++i)
            {
                for (int j = 0; j < Map.gridHeight; ++j)
                {
                    if (grid[i, j])
                    {
                        grid[i, j].gameObject.GetComponent<Renderer>().enabled = true;
                    }
                }
            }
        }
        if (Input.GetKeyDown("w"))
        {
            for (int i = 0; i < Map.gridWidth; ++i)
            {
                for (int j = 0; j < Map.gridHeight; ++j)
                {
                    if (grid[i, j])
                    {
                        grid[i, j].gameObject.GetComponent<Renderer>().enabled = false;
                    }
                }
            }
        }
    }

    public void RemoveRowsIfFull(Tetromino tetromino)
    {
        bool[] rows = new bool[Map.gridHeight];

        foreach (Mino mino in tetromino.GetComponentsInChildren<Mino>())
        {
            var pos = mino.GetGridPosition();
            rows[pos.y] = true;
        }

        var isFull = gridUtils.MakeIsFull();
        var isEmpty = gridUtils.MakeIsEmpty();
        var shiftAmount = gridUtils.MakeShiftAmount(isFull, isEmpty);

        for (int i = 0; i < rows.Length; ++i)
        {
            if (rows[i] && isFull[i])
            {
                StartCoroutine(DebugDelete(isFull, shiftAmount));
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowSlider : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isSliding;
    Map map;

    GridUtils gridUtils;

    // coroutines do not run in parallel. No need for semaphores.
    public int coroutineCount { get; set; }

    public bool[,] gridBitmap;



    void Start()
    {
        isSliding = false;
        coroutineCount = 0;

    }

    public void Initialize(Map map)
    {
        this.map = map;
        gridUtils = map.gridUtils;
    }

    IEnumerator SlideDownRow(int row)
    {
        coroutineCount++;

        float gravity = 9.8f;
        float gravityAdd = 40;
        Vector3 velocity = Vector3.zero;
        Vector3 shift;

        if (map.gridUtils.isRowEmpty[row])
        {
            coroutineCount--;
            yield break;
        }

        while (true)
        {
            velocity.y -= gravity * Time.deltaTime;
            gravity += gravityAdd * Time.deltaTime;
            shift = velocity * Time.deltaTime;

            bool finished = false;

            for (int i = 0; i < Map.gridWidth; ++i)
            {
                Debug.Log(i + ' ' + row);
                if (gridBitmap[i, row])
                {

                    if (map.grid[i, row].position.y > ConvertGridYtoRealY(row))
                    {
                        map.grid[i, row].position += shift;

                        map.rowPosition[row] = map.grid[i, row].position;
                    } 
                    else
                    {
                        Vector3 pos = map.grid[i, row].position;
                        pos.y = ConvertGridYtoRealY(row);
                        map.grid[i, row].position = pos;

                        map.rowPosition[row].y = pos.y;
                        finished = true;
                    }
                }
            }

            if (finished)
            {
                coroutineCount--;
                yield break;
            }
            yield return null; // required for continuous flow
        }

    }
    
    public void Initialize(GridUtils gridUtils)
    {
        this.gridUtils = gridUtils;
    }

    void MoveRowBy(int row, Vector3 shift)
    {
        for (int col = 0; col < Map.gridWidth; ++col)
        {
            var mino = map.grid[col, row];
            if (mino)
            {
                mino.position += shift;
            }
        }
    }

    public float ConvertGridYtoRealY(int y)
    {
        return map.basePosition.y + map.scaleFactor * y;
    }

    public void slideDown()
    {
        for (int row = 0; row < Map.gridHeight; ++row) // must be starting from 0
        {
            StartCoroutine(SlideDownRow(row));
        }
    }

    public void UpdateGridBitmap()
    {
        gridBitmap = new bool[Map.gridWidth, Map.gridHeight];

        for(int row = 0; row < Map.gridHeight; ++row)
        {
            for (int col = 0; col < Map.gridWidth; ++col)
            {
                if (map.grid[col, row])
                {
                    gridBitmap[col, row] = true;
                }
            }
        }
    }
} 
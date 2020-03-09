using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RowSlider : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isSliding;
    Map map;
    Transform[,] gridBeforeDestroy;


    GridUtils gridUtils;

    // coroutines do not run in parallel. No need for semaphores.
    public int coroutineCount { get; set; }

    //public bool[,] gridBitmap;




    void Start()
    {
        isSliding = false;
        coroutineCount = 0;

    }

    public void Initialize(Map map, GridUtils gridUtils)
    {
        name = "Row Slider";
        this.map = map;
        this.gridUtils = gridUtils;
    }

    IEnumerator SlideDownRow(int row)
    {
        coroutineCount++;

        Vector3 shift;

        if (gridUtils.IsRowEmpty(row))
        {
            coroutineCount--;
            yield break;
        }

        while (true)
        {

            shift = map.gridGravity.Shift(Time.deltaTime);

            int finished = 0;

            for (int i = 0; i < Map.gridWidth; ++i)
            {
                if (gridUtils.grid[i, row])
                {
                    var mino = gridUtils.grid[i, row].gameObject.GetComponent<Mino>();

                    if (gridUtils.grid[i, row].position.y > ConvertGridYtoRealY(mino.slideDestination))
                    {
                        gridUtils.grid[i, row].position += shift;

                        map.rowPosition[row] = gridUtils.grid[i, row].position;
                    } 
                    else
                    {
                        Vector3 pos = gridUtils.grid[i, row].position;
                        pos.y = ConvertGridYtoRealY(mino.slideDestination);
                        gridUtils.grid[i, row].position = pos;

                        map.rowPosition[row].y = pos.y;
                        finished++;
                    }
                }
                else
                {
                    finished++;
                }
            }

            if (finished == Map.gridWidth)
            {
                coroutineCount--;
                yield break;
            }
            yield return null; // required for continuous flow
        }


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

    public void SlideDown(PistonSet set)
    {

        for (int row = set.LowestRow(); row < set.NextLowestRow(); ++row) // must be starting from 0
        {
            StartCoroutine(SlideDownRow(row));
        }
    }
} 
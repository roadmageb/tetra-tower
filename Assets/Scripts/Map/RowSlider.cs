using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RowSlider : MonoBehaviour
{
    public bool isSliding;
    public Map map;
    public GridUtils gridUtils;
    public int coroutineCount;

    public bool[] isFullDebug;
    public bool[] isEmptyDebug;
    public int[] shiftDownDebug;
    public PistonSet setDebug;

    public void Initialize(Map map, GridUtils gridUtils)
    {
        name = "Row Slider";
        this.map = map;
        this.gridUtils = gridUtils;

        isSliding = false;

        coroutineCount = 0;
    }

    IEnumerator SlideDownRow(int row)
    {
        coroutineCount++;
        //Debug.Log("coroutineCount increased to " + coroutineCount);

        Vector3 shift;

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
                //Debug.Log("coroutineCount decreased to " + coroutineCount);
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

    public void SlideDown(bool[] isEmpty, PistonSet set)
    {

        isEmptyDebug = isEmpty;
        setDebug = set;

        for (int row = set.LowestRow(); row < set.NextLowestRow(); ++row) // must be starting from 0
        {
            if (!isEmpty[row])
            {
                StartCoroutine(SlideDownRow(row));
            }
        }
    }
} 
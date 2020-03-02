﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowDestroyer : MonoBehaviour
{
    public Map map;
    public bool destroyFinished;

    void Start()
    {
        destroyFinished = false;
    }

    public void Initialize(Map map)
    {
        this.map = map;
    }

    public void Reset()
    {
        destroyFinished = false;
    }

    // Update is called once per frame  
    public void RequestDestroyRows(PistonSpawner spawner, PistonSet set)
    {
        map.gridUtils.isFullUpdate();
        map.gridUtils.UpdateIsRowEmpty();

        map.gridUtils.shiftAmountUpdate();
        DestroyRows(map.gridUtils.isFull, set);
        MoveRowsDown(map.gridUtils.isFull, map.gridUtils.shiftDown, set);

        destroyFinished = true;
    }
    public void MoveRowsDown(bool[] isFull, int[] shiftAmount, PistonSet set)
    {
        for (int i = set.LowestRow(); i < set.NextLowestRow(); ++i)
        {
            if (!isFull[i]) {
                MoveRowDown(i, -shiftAmount[i]);
            }
        }
    }
    void MoveRowDown(int y, int num)
    {
        if (num == 0)
        {
            return; // do nothing
        }

        for (int x = 0; x < Map.gridWidth; ++x)
        {
            if (map.grid[x, y] != null)
            {
                // for delayed transform shift
                var mino = map.grid[x, y].gameObject.GetComponent<Mino>();
                mino.slideDestination = y - num;

                map.grid[x, y - num] = map.grid[x, y];
                map.grid[x, y] = null;

            }
        }
    }

    public void DestroyRows(bool[] isFull, PistonSet set)
    {
        for (int i = set.LowestRow(); i < set.NextLowestRow(); ++i)
        {
            if (isFull[i])
            {
                DestroyRow(i);
            }
        }
    }
    void DestroyRow(int y)
    {
        for (int x = 0; x < Map.gridWidth; ++x)
        {
            Destroy(map.grid[x, y].gameObject);
            map.grid[x, y] = null;
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowDestroyer : MonoBehaviour
{
    public Map map;
    public GridUtils gridUtils;

    public int[] shiftAmountForDebug;
    public bool[] isFullForDebug;
    public bool[] isEmptyForDebug;
    public PistonSet setForDebug;

    public void Initialize(Map map, GridUtils gridCopyUtils)
    {
        this.name = "Row Destroyer";
        this.map = map;
        gridUtils = gridCopyUtils;
    }

    // Update is called once per frame  
    public void RequestDestroyRows(bool[] isFull, int[] shiftAmount, PistonSet set)
    {
        /* Debugging purpose */
        isFullForDebug = isFull;
        shiftAmountForDebug = shiftAmount;
        setForDebug = set;

        MoveTetrominoDown(isFull);
        DestroyRows(isFull, set);
        MoveRowsDown(isFull, shiftAmount, set);

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

    public void MoveTetrominoDown(bool[] isFull)
    {
        // if piston Exist, cancel this function.
        if (map.pistonSpawner.pistonExists())
        {
            return;
        }

        var tet = map.currentTetromino;
        if (tet.isFalling)
        {
            return;
        }
        var shiftAmount = new int[Map.gridHeight];
        int shift = 0;

        for (int i = 0; i < Map.gridHeight; ++i)
        {
            if (isFull[i])
            {
                shift += 1;
            }
            else
            {
                shiftAmount[i] = shift;
            }
        }

        var y = tet.gridPosition.y;
        tet.SlideBy(shiftAmount[y]);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridUtils
{
    // Start is called before the first frame update

    public bool[] isFull { get; set; }
    public int[] shiftDown { get; set; }
    public int fullRowCount { get; set; }

    public Transform[,] grid;
    int width;
    int height;

    public void Initialize(Transform[,] _grid)
    {
        grid = _grid;
        width  = grid.GetLength(0);
        height = grid.GetLength(1);
        isFull = new bool[height];
        shiftDown = new int[height];
        fullRowCount = 0;
    }

    public bool[] isFullUpdate()
    {
        fullRowCount = 0;
        isFull = new bool[height]; // clear

        bool isFullRowAt(int rowNum){
            for (int x = 0; x < width; ++x)
            {
                if (grid[x, rowNum] == null)
                {
                    return false;
                }
            }
            return true;
        }

        for(int i = 0; i < height; ++i)
        {
            if (isFullRowAt(i))
            {
                isFull[i] = true;
                fullRowCount++;
            }
        }

        return isFull;
    }

    public int[] shiftAmountUpdate()
    {
        shiftDown = new int[height];

        int shift = 0;

        for (int i = 0; i < height; ++i)
        {
            if (isFull[i])
            {
                shift -= 1;
            } 
            else
            {
                shiftDown[i] = shift;
            }
        }

        return shiftDown;

    }

    public void Test()
    {
        isFull = new bool[]{ false, true, false, false, false, true, false, true, false };
        shiftAmountUpdate();

        int[] expected = new[] { 0, 0, -1, -1, -1, 0, -2, 0, -3 };

        for(int i = 0; i < expected.Length; ++i)
        {
            Debug.Assert(shiftDown[i] == expected[i]);
        }


    }

    public void DebugArrays()
    {
        Debug.Log(isFull);
        Debug.Log(shiftDown);
    }
    public void printGrid(Transform[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);
        string gridStr = "";
        for( int y = height - 1; y >= 0; --y)
        {
            for (int x = 0; x < width; ++x)
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

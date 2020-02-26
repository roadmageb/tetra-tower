﻿using System.Collections;
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

    float[] rowDestinations;


    float[] rowY;



    void Start()
    {
        isSliding = false;
        coroutineCount = 0;

    }

    public void Initialize(Map map)
    {
        this.map = map;
        this.gridUtils = map.gridUtils;
    }

    IEnumerator slideDownRowOld(int row, int amount)
    {
        coroutineCount++;
        float gravity = 9.8f;
        float gravityAdd = 40;
        Vector3 velocity = Vector3.zero;
        Vector3 shift;

        bool minoExists = false;
        for (int i = 0; i < Map.gridWidth; ++i)
        {
            if (map.grid[i, row] != null)
            {
                minoExists = true;
            }
        }

        if (!minoExists)
        {
            coroutineCount--;
            yield break;
        }

        while (true)
        {
            velocity.y -= gravity * Time.deltaTime;
            gravity += gravityAdd * Time.deltaTime;
            shift = velocity * Time.deltaTime;

            for (int i = 0; i < Map.gridWidth; ++i)
            {
                if (map.grid[i, row] != null)
                {
                    if (map.grid[i, row].position.y > row + amount)
                    {
                        map.grid[i, row].position += shift;
                    } else
                    {
                        Vector3 pos = map.grid[i, row].position;
                        pos.y = row + amount;
                        map.grid[i, row].position = pos;
                        coroutineCount--;
                        yield break;
                    }
                }
            }
            yield return null;
        }
    }

    IEnumerator SlideDownRow(int row)
    {
        coroutineCount++;

        float gravity = 9.8f;
        float gravityAdd = 40;
        Vector3 velocity = Vector3.zero;
        Vector3 shift;

        if (map.isRowEmpty[row])
        {
            coroutineCount--;
            yield break;
        }

        while (true)
        {
            velocity.y -= gravity * Time.deltaTime;
            gravity += gravityAdd * Time.deltaTime;
            shift = velocity * Time.deltaTime;

            bool exit = false;

            for (int i = 0; i < Map.gridWidth; ++i)
            {
                if (map.grid[i, row] != null)
                {
                    var mino = map.grid[i, row].gameObject.GetComponent<Mino>();

                    if (map.grid[i, row].position.y > mino.fallDestination)
                    {
                        map.grid[i, row].position += shift;
                    } else
                    {
                        Vector3 pos = map.grid[i, row].position;
                        pos.y = mino.fallDestination;
                        map.grid[i, row].position = pos;

                        exit = true;
                    }
                }
            }

            if (exit)
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

    public void slideDown()
    {
        for (int row = 0; row < Map.gridHeight; ++row) // must be starting from 0
        {
            StartCoroutine(SlideDownRow(row));
        }
    }
} 
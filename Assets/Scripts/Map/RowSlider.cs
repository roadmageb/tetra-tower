using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowSlider : MonoBehaviour
{
    // Start is called before the first frame update

    bool isSliding;

    GridUtils gridUtils;

    // coroutines do not run in parallel. No need for semaphores.
    public int coroutineCount { get; set; }


    void Start()
    {
        isSliding = false;
        coroutineCount = 0;
    }

    IEnumerator slideDownRow(int row, int amount)
    {
        coroutineCount++;
        float gravity = 9.8f;
        float gravityAdd = 40;
        Vector3 velocity = Vector3.zero;
        Vector3 shift;

        bool minoExists = false;
        for( int i = 0; i < Constants.gridWidth; ++i)
        {
            if (Map.grid[i, row] != null)
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

            for (int i = 0; i < Constants.gridWidth; ++i)
            {
                if (Map.grid[i, row] != null)
                {
                    if (Map.grid[i, row].position.y > row + amount)
                    {
                        Map.grid[i, row].position += shift;
                    } else
                    {
                        Vector3 pos = Map.grid[i, row].position;
                        pos.y = row + amount;
                        Map.grid[i, row].position = pos;
                        coroutineCount--;
                        yield break;
                    }
                }
            }
            yield return null;
        }
    }



    void slideDownRows(bool[] isFull, int[] shiftAmount)
    {
        for (int row = 1; row < Constants.gridHeight; ++row)
        {
            StartCoroutine(slideDownRow(row, shiftAmount[row]));
        }

    }
    
    public void Initialize(GridUtils gridUtils)
    {
        this.gridUtils = gridUtils;
    }

    public void slideDown()
    {
        isSliding = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSliding)
        {
            slideDownRows(gridUtils.isFull, gridUtils.shiftDown);
            isSliding = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            isSliding = true;
        }
    }
} 
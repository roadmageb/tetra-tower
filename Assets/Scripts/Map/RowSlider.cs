using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowSlider : MonoBehaviour
{
    // Start is called before the first frame update

    bool isSliding;
    Map map;

    GridUtils gridUtils;

    // coroutines do not run in parallel. No need for semaphores.
    public int coroutineCount { get; set; }


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

                        if (i == Map.gridWidth - 1)
                        {
                            coroutineCount--;
                            yield break;
                        }
                    }
                }
            }
            yield return null;
        }

    }

    void SlideDownRows()
    {
        for (int row = 0; row < Map.gridHeight; ++row)
        {
            StartCoroutine(SlideDownRow(row));
        }
    }


    void slideDownRowsOld(bool[] isFull, int[] shiftAmount)
    {
        for (int row = 1; row < Map.gridHeight; ++row)
        {
            StartCoroutine(slideDownRowOld(row, shiftAmount[row]));
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
            SlideDownRows();
            isSliding = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            isSliding = true;
        }
    }
} 
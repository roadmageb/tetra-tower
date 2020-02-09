using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowSlider : MonoBehaviour
{
    // Start is called before the first frame update

    Map map;
    bool isSliding;
    int rowNum;
    int count;

    void Start()
    {
        map = GameObject.FindGameObjectWithTag("MapTag").GetComponent<Map>();
        isSliding = false;
        rowNum = 1;
        count = 0;
    }

    void slideDown()
    {
        for (int y = rowNum; y < Map.gridHeight; ++y)
        {
            for (int x = 0; x < Map.gridWidth; ++x)
            {
                if (Map.grid[x,y] != null)
                {
                    Map.grid[x,y].position += new Vector3(0, -0.01f, 0);
                }
            }
        }
    }
    
    public void Initialize(int row)
    {
        isSliding = true;
        rowNum = row;
    }

    // Update is called once per frame
    void Update()
    {

        if (isSliding)
        {
            slideDown();
            count++;
            if (count == 100)
            {
                count = 0;
                isSliding = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            rowNum = 1;
            isSliding = true;
        }
    }
} 
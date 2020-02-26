using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonSpawner : MonoBehaviour
{
    public GameObject PistonLeftPrefab;
    public GameObject PistonRightPrefab;

    PistonLeft pistonLeft;
    PistonRight pistonRight;

    public float pistonZ = 1;

    public Map map;

    public void Initialize(Map map)
    {
        this.map = map;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            spawnNth(0);
            spawnNth(2);
            spawnNth(4);
            spawnNth(6);
            spawnNth(8);
            spawnNth(9);
            spawnNth(11);
        }
    }

    public void spawnIfFull(bool[] isFull)
    {
        for (int i = 0; i < isFull.Length; ++i)
        {
            if (isFull[i])
            {
                spawnNth(i);
            }
        }

    }

    public void spawnNth(int n)
    {
        spawnLeftNth(n);
        spawnRightNth(n);
    }

    public void spawnLeftNth(int n)
    {
        GameObject pistonLeftObj = Instantiate(PistonLeftPrefab);
        pistonLeftObj.transform.localScale += map.scaleVector;
        pistonLeftObj.transform.position = map.basePosition + map.scaleFactor * new Vector3(-0.5f, (float) n, 0);
        pistonLeft = pistonLeftObj.GetComponent<PistonLeft>();
        pistonLeft.Initialize(map);
    }

    public void spawnRightNth(int n)
    {
        GameObject pistonRightObj= Instantiate(PistonRightPrefab);
        pistonRightObj.transform.position = map.basePosition + map.scaleFactor * new Vector3(9.5f, (float) n, pistonZ);
        pistonRightObj.transform.localScale += map.scaleVector;
        pistonRight = pistonRightObj.GetComponent<PistonRight>();
        pistonRight.Initialize(map);
    }
} 
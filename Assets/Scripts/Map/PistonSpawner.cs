using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonSpawner : MonoBehaviour
{
    public GameObject PistonLeftPrefab;
    public GameObject PistonRightPrefab;

    PistonLeft pistonLeft;
    PistonRight pistonRight;

    // Start is called before the first frame update
    void Start()
    {
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
        GameObject pistonLeftObj = Instantiate(PistonLeftPrefab, new Vector3(-7.5f, (float)n, -1), Quaternion.identity) as GameObject;
        pistonLeft = pistonLeftObj.GetComponent<PistonLeft>();
    }

    public void spawnRightNth(int n)
    {
        GameObject pistonRightObj = Instantiate(PistonRightPrefab, new Vector3(16.5f, (float)n, -1), Quaternion.identity) as GameObject;
        pistonRight = pistonRightObj.GetComponent<PistonRight>();
    }
} 
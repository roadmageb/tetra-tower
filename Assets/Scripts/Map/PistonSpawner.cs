using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonSpawner : MonoBehaviour
{
    public GameObject PistonLeftPrefab;
    public GameObject PistonRightPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnNth(int n)
    {
        spawnLeftNth(n);
        spawnRightNth(n);
    }

    public void spawnLeftNth(int n)
    {
        GameObject pistonLeftObj = Instantiate(PistonLeftPrefab, new Vector3(-7.5f, (float)n, 0), Quaternion.identity) as GameObject;
        PistonLeft pistonLeft = pistonLeftObj.GetComponent<PistonLeft>();
    }

    public void spawnRightNth(int n)
    {
        GameObject pistonRightObj = Instantiate(PistonRightPrefab, new Vector3(16.5f, (float)n, 0), Quaternion.identity) as GameObject;
        PistonRight pistonRight = pistonRightObj.GetComponent<PistonRight>();
    }
}

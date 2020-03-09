using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonSpawner : MonoBehaviour
{
    public GameObject PistonLeftPrefab;
    public GameObject PistonRightPrefab;

    [SerializeField]
    private int _pistonSetCount;

    public int pistonSetCount
    {
        get
        {
            return _pistonSetCount;
        }

        set
        {
            _pistonSetCount = value;
        }
    }

    PistonLeft pistonLeft;
    PistonRight pistonRight;

    public float pistonZ = 1;

    public Map map;

    public bool[] nthPistonExists;


    public void Initialize(Map map)
    {
        this.name = "Piston Spawner";
        pistonSetCount = 0;
        this.map = map;
        nthPistonExists = new bool[Map.gridHeight];
    }

    public PistonSet spawnIfFull(bool[] isFull)
    {
        var pistonSet = new GameObject().AddComponent<PistonSet>();
        pistonSet.Init(this);

        for (int i = 0; i < isFull.Length; ++i)
        {
            if (isFull[i] && !nthPistonExists[i])
            {
                var pair = SpawnNthPair(i);
                pistonSet.Add(pair);
            }
        }
        return pistonSet;
    }

    public PistonPair SpawnNthPair(int n)
    {
        var left = SpawnLeftNth(n);
        var right = SpawnRightNth(n);

        var pistonPair = new GameObject().AddComponent<PistonPair>().MakePair(left, right, n);
        pistonPair.Init(this);

        nthPistonExists[n] = true;

        pistonSetCount++;
        return pistonPair;
    }

    public PistonLeft SpawnLeftNth(int n)
    {
        GameObject pistonLeftObj = Instantiate(PistonLeftPrefab);
        pistonLeftObj.transform.localScale += map.scaleVector;
        pistonLeftObj.transform.position = map.basePosition + map.scaleFactor * new Vector3(-0.5f, (float) n, 0);
        pistonLeft = pistonLeftObj.GetComponent<PistonLeft>();
        pistonLeft.Initialize(n, map);

        return pistonLeft;
    }

    public PistonRight SpawnRightNth(int n)
    {
        GameObject pistonRightObj= Instantiate(PistonRightPrefab);
        pistonRightObj.transform.position = map.basePosition + map.scaleFactor * new Vector3(9.5f, (float) n, 0);
        pistonRightObj.transform.localScale += map.scaleVector;
        pistonRight = pistonRightObj.GetComponent<PistonRight>();
        pistonRight.Initialize(n, map);

        return pistonRight;
    }

    public bool pistonExists()
    {
        foreach (var exist in nthPistonExists)
        {
            if (exist)
            {
                return true;
            }
        }
        return false;
    }

} 
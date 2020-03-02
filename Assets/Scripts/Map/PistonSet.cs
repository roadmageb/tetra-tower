using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PistonSet: MonoBehaviour
{
    [SerializeField]
    private List<PistonPair> pairs;

    public PistonSpawner pistonSpawner;

    public void Init(PistonSpawner pistonSpawner)
    {
        pairs = new List<PistonPair>(4);
        this.pistonSpawner = pistonSpawner;
    }

    public void Add(PistonPair pair)
    {
        pair.set = this;
        pairs.Add(pair);
    }
                                                                  
    public bool FinishSet()
    {
        foreach (var i in pairs)
        {
            if (!i.FinishPair())
            {
                return false;
            }
        }

        return true;
    }

    public int LowestRow()
    {
        Debug.Assert(pairs.Count > 0);
        return pairs[0].row;
    }

    public int HighestRow()
    {
        return pairs[pairs.Count - 1].row;
    }
    
    public int NextLowestRow()
    {
        var nextLowest = Array.FindIndex(pistonSpawner.nthPistonExists, HighestRow() + 1, (bool x) => x);
        if (nextLowest == -1) // not found
        {
            nextLowest = Map.gridHeight;
        }
        return nextLowest;

    }

}

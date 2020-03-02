using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonPair : MonoBehaviour
{
    public PistonLeft left;
    public PistonRight right;

    public int row;

    public PistonSet set;

    public bool leftFinish;
    public bool rightFinish;

    public PistonSpawner pistonSpawner;

    public void Init(PistonSpawner pistonSpawner)
    {
        this.pistonSpawner = pistonSpawner;
        leftFinish = false;
        rightFinish = false;
    }


    void Update()
    {
        if (FinishPair())
        {
            pistonSpawner.nthPistonExists[row] = false;
            Destroy(gameObject);
        }
    }

    public bool FinishPair()
    {
        return leftFinish && rightFinish;
    }

    public PistonPair MakePair(PistonLeft left, PistonRight right, int row)
    {
        Debug.Assert(left.currentRow == right.currentRow);
        Debug.Assert(left.currentRow == row);

        this.left = left;
        this.right = right;
        this.row = row;

        left.pair = this;
        right.pair = this;
        return this;
    }
}

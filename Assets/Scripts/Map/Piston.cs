using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piston: MonoBehaviour
{
    public int currentRow;

    public bool isMoving;
    public bool isMovingCenter;
    public float compressVelocity;
    public float releaseVelocity;
    public Map map;

    public PistonPair pair;


    public void Initialize(int row, Map map)
    {
        this.map = map;
        currentRow = row;
        isMoving = true;
        isMovingCenter = true;
        //compressVelocity = 0.5f * map.scaleFactor;
        //releaseVelocity = 3.0f * map.scaleFactor;
        compressVelocity = 2.0f * map.scaleFactor;
        releaseVelocity = 12.0f * map.scaleFactor;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving == true)
        {
            pistonMovement();
        }
    }

    void pistonMovement()
    {
        if (isMovingCenter)
        {
            compress();
        } else
        {
            release();
        }
    }

    protected abstract void compress();
    protected abstract void release();

}

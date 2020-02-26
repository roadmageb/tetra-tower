using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Piston: MonoBehaviour
{
    public static int pistonCount {get; set;}

    protected bool isMoving;
    protected bool isMovingCenter;
    protected float compressVelocity;
    protected float releaseVelocity;
    public Map map;

    public void Initialize(Map map)
    {
        this.map = map;
        pistonCount++;
        isMoving = true;
        isMovingCenter = true;
        compressVelocity = 5f * map.scaleFactor;
        releaseVelocity = 30f * map.scaleFactor;
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

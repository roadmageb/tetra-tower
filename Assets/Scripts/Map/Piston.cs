using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Piston: MonoBehaviour
{
    public static int pistonCount {get; set;}

    protected bool isMoving;
    protected bool isMovingCenter;
    //protected float compressVelocity = 0.5f;
    //protected float releaseVelocity = 3.0f;
    protected float compressVelocity = 5f;
    protected float releaseVelocity = 30f;

    // Start is called before the first frame update
    void Start()
    {
        pistonCount++;
        isMoving = true;
        isMovingCenter = true;
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

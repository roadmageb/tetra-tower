using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Piston: MonoBehaviour
{

    protected bool isMoving;
    protected bool isMovingCenter;
    protected float pistonSpeed = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
        isMovingCenter = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isMoving == false) { 
                isMoving = true;
                isMovingCenter = true;
                return;
            }
        }

        if (isMoving == true)
        {
            pistonMovement();
        }
    }

    void pistonMovement()
    {
        if (isMovingCenter)
        {
            moveToCenter();
        } else
        {
            moveAway();
        }
    }

    protected abstract void moveToCenter();
    protected abstract void moveAway();

}

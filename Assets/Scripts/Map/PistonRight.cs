using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonRight: MonoBehaviour
{

    bool isMoving;
    bool isMovingCenter;
    float pistonSpeed = 0.02f;

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

    void moveToCenter()
    {
        if (transform.position.x <= 10.5)
        {
            isMovingCenter = false;
            return;
        }
        transform.position -= new Vector3(pistonSpeed, 0, 0);
    }


    void moveAway()
    {
        if (transform.position.x >= 16.5)
        {
            isMoving = false;
            Destroy(gameObject);
        }
        transform.position += new Vector3(pistonSpeed, 0, 0);
    }

}

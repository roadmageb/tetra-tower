using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PistonRight : Piston
{
    protected override void moveToCenter()
    {
        if (transform.position.x <= 10.5)
        {
            isMovingCenter = false;
            return;
        }
        transform.position -= new Vector3(pistonSpeed, 0, 0);
    }

    protected override void moveAway()
    {
        if (transform.position.x >= 16.5)
        {
            isMoving = false;
            Destroy(gameObject);
        }
        transform.position += new Vector3(pistonSpeed, 0, 0);
    }
}

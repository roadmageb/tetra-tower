using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PistonLeft : Piston
{
    protected override void moveToCenter()
    {
        if (transform.position.x >= Constants.Base.x - 1.5)
        {
            isMovingCenter = false;
            return;
        }
        transform.position += new Vector3(pistonSpeed, 0, 0);
    }

    protected override void moveAway()
    {
        if (transform.position.x <= Constants.Base.x - 7.5)
        {
            isMoving = false;
            Destroy(gameObject);
        }
        transform.position -= new Vector3(pistonSpeed, 0, 0);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PistonRight : Piston
{
    protected override void compress()
    {
        if (transform.position.x <= Constants.Base.x + 10.5)
        {
            isMovingCenter = false;
            return;
        }
        transform.position -= new Vector3(compressVelocity * Time.deltaTime, 0, 0);
    }

    protected override void release()
    {
        if (transform.position.x >= Constants.Base.x + 16.5)
        {
            isMoving = false;
            pistonCount--;
            Destroy(gameObject);
        }
        transform.position += new Vector3(releaseVelocity * Time.deltaTime, 0, 0);
    }
}

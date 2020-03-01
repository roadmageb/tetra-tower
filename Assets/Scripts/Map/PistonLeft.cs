﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonLeft : Piston
{
    protected override void compress()
    {
        if (transform.position.x >= map.basePosition.x + map.scaleFactor * 4.5 )
        {
            isMovingCenter = false;
            return;
        }
        transform.position += new Vector3(compressVelocity * Time.deltaTime, 0, 0);
    }

    protected override void release()
    {
        if (transform.position.x <= map.basePosition.x )
        {
            isMoving = false;
            pistonCount--;
            Destroy(gameObject);
        }
        transform.position -= new Vector3(releaseVelocity * Time.deltaTime, 0, 0);
    }

}

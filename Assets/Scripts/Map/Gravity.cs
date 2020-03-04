using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float gravity = 1000F;
    public float gravityAdd = 4000;
    public float gravityMul = 1.1f;
    public Vector3 initialVelocity = Vector3.zero;
    public Vector3 velocity;
    public Vector3 shift;

    void Start()
    {
        gravity = 1000F;
        gravityAdd = 4000;
        gravityMul = 1.1F;
        initialVelocity = Vector3.zero;
        velocity = initialVelocity;
        shift = Vector3.zero;
    }
    
    public Vector3 Shift(float deltaTime)
    {
        velocity.y -= gravity * deltaTime;
        //gravity += gravityAdd * deltaTime;
        gravity = gravityMul * gravity;
        shift = velocity * deltaTime;
        return shift;
    }

    public void Reset()
    {
        Start();
    }

}

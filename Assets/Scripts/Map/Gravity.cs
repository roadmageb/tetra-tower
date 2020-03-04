using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float gravity = 1000F;
    public float gravityAdd = 4000;
    public float gravityMul = 1.1f;
    public Vector3 initialVelocity = Vector3.zero;
    public Vector3 acceleration;
    public Vector3 velocity;
    public Vector3 shift;
    public int timestep;


    void Start()
    {
        gravity = 9.8F * 16;
        gravityAdd = 4000;
        gravityMul = 1.1F;
        initialVelocity = Vector3.zero;
        velocity = initialVelocity;
        shift = Vector3.zero;
        acceleration = Vector3.zero;
        timestep = 0;
    }
    
    public Vector3 Shift(float deltaTime)
    {
        /*
        velocity.y -= gravity * gravity * deltaTime;
        //gravity += gravityAdd * deltaTime;
        //gravity = gravityMul * gravity;
        shift = velocity * deltaTime;
        return shift;
        */
        velocity.y -= gravity * deltaTime;
        velocity.y *= 1.01f;
        return shift = velocity * deltaTime;

    }

    public void Reset()
    {
        velocity = initialVelocity;
    }

}

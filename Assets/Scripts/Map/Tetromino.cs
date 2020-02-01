﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    // Start is called before the first frame update

    public bool allowRotation = true;
    public bool limitRotation = false;

    bool isFalling = false;
    const float gravity = 9.8F;
    Vector3 velocity = new Vector3(0, 0, 0);
    Vector3 shift;

    void Start()
    {
    }
   
    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            velocity.y -= gravity * Time.deltaTime;
            shift = velocity * Time.deltaTime;
            Debug.Log(shift);
            transform.position += shift;

            // TODO: IsValidPosition a bit buggy when tasking transform position with floats.
            if (!IsValidPosition())
            {
                transform.position += new Vector3(0, 1, 0);
                var tp = transform.position;
                transform.position = new Vector3(tp.x, Mathf.Floor(tp.y), tp.z); 
                
                isFalling = false;

                // initialize
                velocity -= velocity;
                FindObjectOfType<Map>().UpdateGrid(this);
                prepareNextTetromino();
            }
            return;
        }
        CheckUserInput();
    }


    void CheckUserInput()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            var shift = new Vector3(1, 0, 0);
            if (CheckIsValidPosition(shift))
            {
                transform.position += shift;
                FindObjectOfType<Map>().UpdateGrid(this);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            var shift = new Vector3(-1, 0, 0);
            if (CheckIsValidPosition(shift))
            {
                transform.position += shift;
                FindObjectOfType<Map>().UpdateGrid(this);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!allowRotation)
            {
                return;
            }

            int rotateAngle = 90;

            if (limitRotation)
            {
                if (transform.rotation.eulerAngles.z >= 90)
                {
                    rotateAngle = -90;
                }
            }

            transform.Rotate(0, 0, -rotateAngle);
            if (!CheckIsValidPosition(new Vector3(0, 0, 0)))
            {
                transform.Rotate(0, 0, rotateAngle);
                FindObjectOfType<Map>().UpdateGrid(this);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            var shift = new Vector3(0, -1, 0);
            isFalling = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            var shift = new Vector3(0, -1, 0);
            if (CheckIsValidPosition(shift))
            {
                transform.position += shift;
                FindObjectOfType<Map>().UpdateGrid(this);
            } else
            {
                prepareNextTetromino();
            }
        }
        else
        {
            //
        }
    }

    void prepareNextTetromino()
    {
        FindObjectOfType<Map>().DeleteRow();
        enabled = false;
        FindObjectOfType<Map>().SpawnNextTetromino();
    }

    // TODO: Refactor CheckIsValidPosition function into IsValidPosition function (better)
    bool CheckIsValidPosition (Vector3 shift)
    {
        foreach (Transform mino in transform)
        {
            Vector3 pos = FindObjectOfType<Map>().Round(mino.position) + shift;
            if( FindObjectOfType<Map>().CheckIsInsideGrid(pos) == false)
            {
                return false;
            }

            if( FindObjectOfType<Map>().GetTransformAtGridPosition(pos) != null &&
                FindObjectOfType<Map>().GetTransformAtGridPosition(pos).parent != transform)
            {
                return false;
            }
        }

        return true;
    }

    bool IsValidPosition ()
    {
        foreach (Transform mino in transform)
        {
            Vector3 pos = FindObjectOfType<Map>().Round(mino.position);
            if( FindObjectOfType<Map>().CheckIsInsideGrid(pos) == false)
            {
                return false;
            }

            if( FindObjectOfType<Map>().GetTransformAtGridPosition(pos) != null &&
                FindObjectOfType<Map>().GetTransformAtGridPosition(pos).parent != transform)
            {
                return false;
            }
        }

        return true;
    }
}


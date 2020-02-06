using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    // Start is called before the first frame update

    public bool allowRotation = true;

    bool isFalling = false;
    float gravity = 9.8F;
    float gravityAdd = 40;
    Vector3 velocity = Vector3.zero;
    Vector3 shift;

    Vector3 fallDestination;

    Map map;

    void Start()
    {
        //map = GameObject.Find("Map").GetComponent<Map>();
        map = GameObject.FindGameObjectWithTag("MapTag").GetComponent<Map>();
    }
   
    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            Fall();
            return;
        }
        CheckUserInput();
    }

    void Fall()
    {
        velocity.y -= gravity * Time.deltaTime;
        gravity += gravityAdd * Time.deltaTime;
        shift = velocity * Time.deltaTime;
        transform.position += shift;
        Debug.Log(transform.position + " " + fallDestination);
        
        if (transform.position.y <= fallDestination.y)
        {
            transform.position = fallDestination;
            isFalling = false;

            // initialize
            velocity = Vector3.zero;
            map.UpdateGrid(this);
            prepareNextTetromino();
        }
    }


    void CheckUserInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            var shift = new Vector3(1, 0, 0);
            if (canShift(shift))
            {
                transform.position += shift;
                map.UpdateGrid(this);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            var shift = new Vector3(-1, 0, 0);
            if (canShift(shift))
            {
                transform.position += shift;
                map.UpdateGrid(this);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!allowRotation)
            {
                return;
            }

            rotateCounterclockwise();
            if (!IsValidPosition())
            {
                rotateClockwise();
            }
            map.UpdateGrid(this);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            isFalling = true;
            ComputeDestinationPosition();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            var shift = new Vector3(0, -1, 0);
            if (canShift(shift))
            {
                transform.position += shift;
                map.UpdateGrid(this);
            } else
            {
                map.UpdateGrid(this);
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
        //map.DeleteRow();
        map.RemoveRowsIfFull();
        enabled = false;
        map.SpawnNextTetromino();
    }

    bool canShift(Vector3 shift)
    {
        foreach (Transform mino in transform)
        {

            Vector3 pos = mino.position + shift;
            //pos.y = Mathf.Floor(pos.y);
            Debug.Log(pos.ToString("F16"));
            pos = Vector3Utils.Map(pos, Mathf.Round);
            if( map.CheckIsInsideGrid(pos) == false)
            {
                return false;
            }

            if( map.GetTransformAtGridPosition(pos) != null &&
                map.GetTransformAtGridPosition(pos).parent != transform)
            {
                return false;
            }
        }

        return true;
    }

    bool IsValidPosition ()
    {
        return canShift(new Vector3(0, 0, 0)); // return true if the current position is valid
    }

    void rotateClockwise()
    {
        rotateByDegree(90);
    }

    void rotateCounterclockwise()
    {
        rotateByDegree(-90);
    }

    void rotateByDegree(float degree)
    {
        transform.Rotate(0, 0, degree);
        transform.position = Vector3Utils.Map(transform.position, Mathf.Round);
        foreach (Transform mino in transform)
        {
            mino.position = Vector3Utils.Map(mino.position, Mathf.Round);
        }

    }

    void ComputeDestinationPosition()
    {
        Vector3 pos = Vector3.zero;

        while (canShift(pos))
        {
            pos += Vector3.down;
        }

        pos = pos + Vector3.up;
        fallDestination = transform.position + pos;
        fallDestination = Vector3Utils.Map(fallDestination, Mathf.Round);
    }
}
 
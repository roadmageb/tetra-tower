using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    // Start is called before the first frame update

    public bool allowRotation = true;

    public bool isFalling = false;
    public float gravity = 9.8F;
    public float gravityAdd = 40;
    public Vector3 velocity;
    public Vector3 initialVelocity;

    public Vector3 shift;
    public Vector3Int gridPosition;

    public Vector3Int fallDestination;
    public Vector3Int realFallDestination;
    public Map map;

    public void Move(Vector3Int offset)
    {
        gridPosition += offset;
        transform.position = map.basePosition + map.scaleFactor * gridPosition;
    }

    public void Initialize(Map map, Vector3Int gridPosition)
    {
        this.map = map;
        this.gridPosition = gridPosition;
        this.transform.position = map.basePosition + map.scaleFactor * gridPosition;
        initialVelocity = new Vector3(0, -100, 0);
        this.velocity = initialVelocity;
    }
   
    void Update()
    {
        if (isFalling)
        {
            Fall();
            //ImmediateFallForDebug();
            return;
        }

        if (!map.inputLock)
        {
            PlayerInput();
        }
    }

    void Fall()
    {
        velocity.y -= gravity * Time.deltaTime;
        gravity += gravityAdd * Time.deltaTime;
        shift = velocity * Time.deltaTime;
        transform.position += shift;
        //Debug.Log(transform.position + " " + fallDestination);
        
        if (transform.position.y <= realFallDestination.y)
        {
            gridPosition = fallDestination;
            transform.position = realFallDestination;
            isFalling = false;

            // initialize
            velocity = initialVelocity;

            map.tetrominoFalling = false;

            prepareNextTetromino();
        }
    }

    void ImmediateFallForDebug()
    {
        gridPosition = fallDestination;

        transform.position = map.basePosition + map.scaleFactor * gridPosition;

        isFalling = false;

        map.UpdateGrid(this);
        prepareNextTetromino();
    }


    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            var shift = new Vector3Int(1, 0, 0);
            if (canShift(shift))
            {
                Move(shift);
                //map.UpdateGrid(this);

            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            var shift = new Vector3Int(-1, 0, 0);
            if (canShift(shift))
            {
                Move(shift);
                //map.UpdateGrid(this);
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
            //map.UpdateGrid(this);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            isFalling = true;
            map.tetrominoFalling = true;
            ComputeDestinationPosition();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            var shift = new Vector3Int(0, -1, 0);
            if (canShift(shift))
            {
                Move(shift);
                //map.UpdateGrid(this);
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

    bool canShift(Vector3Int shift)
    {
        foreach (Transform child in transform)
        {
            var mino = child.gameObject.GetComponent<Mino>();
            var pos = shift + mino.GetGridPosition();

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
        return canShift(new Vector3Int(0, 0, 0)); // return true if the current position is valid
    }

    void rotateClockwise()
    {
        rotateCounterclockwise();
        rotateCounterclockwise();
        rotateCounterclockwise();
    }

    void rotateCounterclockwise()
    {
        foreach (Mino mino in GetComponentsInChildren<Mino>())
        {
            var newX = - mino.localPosition.y;
            var newY = mino.localPosition.x;
            mino.localPosition.x = newX;
            mino.localPosition.y = newY;

            mino.transform.localPosition = mino.localPosition;
        }
    }

    void ComputeDestinationPosition()
    {
        Vector3Int shift = Vector3Int.zero;

        while (canShift(shift))
        {
            shift += Vector3Int.down;
        }

        shift = shift + Vector3Int.up;
        //fallDestination = Vector3IntUtils.Map(transform.position + pos, Mathf.Round);
        fallDestination = gridPosition + shift;
        realFallDestination = map.basePosition + map.scaleFactor * fallDestination;
        gridPosition = fallDestination;
        map.UpdateGrid(this);

    }
}
 
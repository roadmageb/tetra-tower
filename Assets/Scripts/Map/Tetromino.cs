using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    // Start is called before the first frame update

    public bool allowRotation = true;

    public bool isFalling = false;
    public float gravity = 1F;
    public float gravityAdd = 4000;
    public float gravityMul =1.001f;
    public Vector3 velocity;
    public Vector3 initialVelocity;

    public Vector3 shift;
    public Vector3Int gridPosition;

    public Vector3Int fallDestination;
    public Vector3Int realFallDestination;
    public Map map;

    public Vector3Int PositionBeforeRotation;

    public void MoveBy(Vector3Int offset)
    {
        gridPosition += offset;
        transform.position = map.basePosition + map.scaleFactor * gridPosition;
    }

    public void MoveTo(Vector3Int location)
    {
        gridPosition = location;
        transform.position = map.basePosition + map.scaleFactor * gridPosition;
    }

    public void Initialize(Map map, Vector3Int gridPosition)
    {
        this.map = map;
        this.gridPosition = gridPosition;
        this.transform.position = map.basePosition + map.scaleFactor * gridPosition;
        initialVelocity = new Vector3(0, 0, 0);
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
        //gravity += gravityAdd * Time.deltaTime;
        gravity = gravityMul * gravity;
        shift = velocity * Time.deltaTime;

        int finishCount = 0;

        foreach (Mino mino in GetComponentsInChildren<Mino>())
        {
            mino.transform.position += shift;

            if (mino.transform.position.y < map.basePosition.y + map.scaleFactor * mino.slideDestination)
            {
                var pos = mino.transform.position;
                pos.y = map.basePosition.y + map.scaleFactor * mino.slideDestination;
                mino.transform.position = pos;
                finishCount++;
            }
        }

        if (finishCount == 4)
        {
            velocity = initialVelocity;
            map.tetrominoFalling = false;
            isFalling = false;
            prepareNextTetromino();
        }
    }

    void FallOld()
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
                MoveBy(shift);
                //map.UpdateGrid(this);

            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            var shift = new Vector3Int(-1, 0, 0);
            if (canShift(shift))
            {
                MoveBy(shift);
                //map.UpdateGrid(this);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PositionBeforeRotation = gridPosition;
            Rotate();
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
                MoveBy(shift);
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
        map.RemoveRowsIfFull(this);
        //CreateRooms();
        enabled = false;
        map.SpawnNextTetromino();
    }

    void CreateRooms()
    {
        foreach (Mino mino in GetComponentsInChildren<Mino>())
        {
            mino.MakeRoom();
        }
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

    void Rotate()
    {
        if (!allowRotation)
        {
            return;
        }

        rotateCounterclockwise();

        if (!IsValidPosition())
        {
            rotateClockwise();
            var x = gridPosition.x;
            if (x == 0 || x == 1)
            {
                MoveBy(Vector3Int.right);
                Rotate();
            }
            else if (x == Map.gridWidth - 1 || x == Map.gridWidth - 2)
            {
                MoveBy(Vector3Int.left);
                Rotate();
            }
            else
            {
                MoveTo(PositionBeforeRotation);
            }
        }
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
        fallDestination = gridPosition + shift;
        realFallDestination = map.basePosition + map.scaleFactor * fallDestination;
        gridPosition = fallDestination;
        map.UpdateGrid(this);

    }
}
 
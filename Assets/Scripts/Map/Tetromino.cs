using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{

    public bool allowRotation = true;

    public bool isFalling = false;
    public bool isSliding = false;

    public Gravity gravity;

    public Vector3Int gridPosition;
    public Vector3Int fallDestination;
    public Map map;

    public Vector3Int PositionBeforeRotation;

    void Start()
    {
        isSliding = false;
        gravity = map.tetrominoGravity;
    }

    public void MoveBy(Vector3Int offset)
    {
        gridPosition += offset;
        transform.position = map.basePosition + map.scaleFactor * gridPosition;
    }

    public void SlideBy(int amount)
    {

        Vector3Int shift = amount * Vector3Int.down;
        fallDestination = gridPosition + shift;
        gridPosition = fallDestination;

        foreach (Transform child in transform)
        {
            var mino = child.gameObject.GetComponent<Mino>();
            var pos = mino.GetGridPosition();
            mino.slideDestination = pos.y;
        }

        isSliding = true;
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
    }

   
    void Update()
    {
        if (isFalling)
        {
            Fall();
            //ImmediateFallForDebug();
            return;
        }

        if (isSliding)
        {
            Slide();
            return;
        }

        if (!map.inputLock)
        {
            PlayerInput();
        }
    }

    public void Slide()
    {

        transform.position += gravity.Shift(Time.deltaTime);

        var actualPosition = map.basePosition + map.scaleFactor * fallDestination;

        if (transform.position.y < actualPosition.y)
        {
            var pos = transform.position;
            pos.y = actualPosition.y;
            transform.position = pos;
            isSliding = false;
            gravity.Reset();
        }
    }

    void Fall()
    {

        int finishCount = 0;

        var shift = gravity.Shift(Time.deltaTime);

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
            gravity.Reset();
            isFalling = false;
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
        gridPosition = fallDestination;
        map.UpdateGrid(this);

    }

}
 
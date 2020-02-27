using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //For test
    public Room nextRoom;
    //For test

    public bool roomCleared = false;
    public Transform enemy = null;
    public static float roomMoveTime = 0.2f;

    public Dictionary<Vector2, Door> doors;

    public IEnumerator MovePlayerToRoom(Vector2 dir)
    {
        Vector2 destination = (Vector2)PlayerController.Instance.transform.position + dir * 2;
        StartCoroutine(CameraController.Instance.MoveCamera(nextRoom.transform.position));
        StartCoroutine(PlayerController.Instance.MovePlayer(destination));
        PlayerController.Instance.transform.parent = nextRoom.transform;
        GameManager.Instance.aStarPath.transform.position = nextRoom.transform.position;

        if (!nextRoom.roomCleared)
        {
            yield return new WaitForSeconds(roomMoveTime);
            nextRoom.ControlDoor(-dir, false);
        }
    }

    public void ControlDoor(Vector2 dir, bool open)
    {
        doors[dir].Open(open);
    }

    public void ClearRoom()
    {
        roomCleared = true;


        foreach(Vector2 _dir in doors.Keys)
        {
            //Open door only door is connected
            //Need to work later
            ControlDoor(_dir, true);
            nextRoom.ControlDoor(-_dir, true);
        }


        Debug.Log("Room Cleared");
    }

    private void Start()
    {
        doors = new Dictionary<Vector2, Door>();
        doors.Add(Vector2.left, transform.Find("DoorLeft").GetComponent<Door>());
        doors.Add(Vector2.right, transform.Find("DoorRight").GetComponent<Door>());
        doors.Add(Vector2.up, transform.Find("DoorUp").GetComponent<Door>());
        doors.Add(Vector2.down, transform.Find("DoorDown").GetComponent<Door>());
    }

    // Update is called once per frame
    void Update()
    {
        if(!roomCleared && enemy.childCount == 0)
        {
            ClearRoom();
        }
    }
}

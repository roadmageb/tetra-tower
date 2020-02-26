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
    public Door[] doors;

    public void MovePlayerToRoom(Vector2 dir)
    {
        Vector2 destination = (Vector2)PlayerController.Instance.transform.position + dir * 2;
        StartCoroutine(CameraController.Instance.MoveCamera(nextRoom.transform.position));
        StartCoroutine(PlayerController.Instance.MovePlayer(destination));
        PlayerController.Instance.transform.parent = nextRoom.transform;

        GameManager.Instance.aStarPath.transform.position = nextRoom.transform.position;
    }

    public void ClearRoom()
    {
        roomCleared = true;
        Debug.Log("Room Cleared");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public static float roomMoveTime = 0.2f;
    //For test
    public Room nextRoom;
    //For test

    public Door[] doors;

    public void MovePlayerToRoom(Vector2 dir)
    {
        Vector2 destination = (Vector2)PlayerController.Instance.transform.position + dir * 2;
        StartCoroutine(CameraController.Instance.MoveCamera(nextRoom.transform.position));
        StartCoroutine(PlayerController.Instance.MovePlayer(destination));

        GameManager.Instance.aStarPath.transform.position = nextRoom.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

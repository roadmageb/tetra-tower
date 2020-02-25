using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //For test
    public Room nextRoom;
    //For test

    public void MovePlayerToRoom(Vector2 dir)
    {
        Vector2 destination = (Vector2)PlayerController.Instance.transform.position + dir * 2;
        PlayerController.Instance.transform.position = destination;
        StartCoroutine(CameraController.Instance.MoveCamera(nextRoom.transform.position));
        GameManager.Instance.aStarPath.UpdateGraphs(new Bounds(nextRoom.transform.position, new Vector3(160, 160)));
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

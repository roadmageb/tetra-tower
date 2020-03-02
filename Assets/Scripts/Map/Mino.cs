using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour
{
    public bool toBeDestroyed;


    public int originalPosition;
    public int slideDestination;

    public Vector3Int localPosition;

    public GameObject RoomPrefab;
    public Room room;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (room)
        {
            room.transform.position = transform.position;
        }
    }

    public Vector3Int GetGridPosition()
    {
        return GetComponentInParent<Tetromino>().gridPosition + localPosition;
    }

    public void MakeRoom()
    {
        var gameObj = Instantiate(RoomPrefab);
        room = gameObj.GetComponent<Room>();
        room.transform.position = transform.position;
    }

}

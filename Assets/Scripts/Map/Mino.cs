using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour
{
    public bool toBeDestroyed;


    public int originalPosition;
    public int slideDestination;

    public Vector3Int localPosition;

    public Room room;

    // Start is called before the first frame update
    void Start()
    {
        var renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.sortingOrder = -1;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Vector3Int GetGridPosition()
    {
        return GetComponentInParent<Tetromino>().gridPosition + localPosition;
    }

    public void ConnectRoom(Room room)
    {
        this.room = room;
        room.transform.position = transform.position;
        room.transform.parent = transform;
    }

    public void SpawnPlayer(GameObject playerPrefab)
    {
        var playerObj = Instantiate(playerPrefab);
        playerObj.transform.position = transform.position;
    }

}

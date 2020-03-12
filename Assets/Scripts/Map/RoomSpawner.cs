using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public GameObject[] roomPrefabs;

    public Shuffler roomShuffler;

    public void Init()
    {
        roomShuffler = new Shuffler(4);
    }

    public Room spawnShuffled()
    {
        int n = roomShuffler.retrieve();
        Debug.Log(n);
        return spawnNth(n);
    }

    Room spawnNth(int n)
    {
        GameObject roomObj = Instantiate(roomPrefabs[n]) as GameObject;

        Room room = roomObj.GetComponent<Room>();

        return room;
    }

}

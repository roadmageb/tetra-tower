using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Room room;
    [SerializeField] private Vector2 doorDir;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            StartCoroutine(room.MovePlayerToRoom(doorDir));
        }
    }

    public void Open(bool open)
    {
        GetComponent<Collider2D>().isTrigger = open;
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

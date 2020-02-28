using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    //For test
    public Room nextRoom;
    //For test

    public bool roomCleared = false;
    public Transform enemy = null;
    public static float roomMoveTime = 0.2f;
    public Door[] doors;
    public Tilemap wallTileMap;
    bool[,] wallCheckArray;
    int[,] adjIndex;

    private void Start()
    {
        adjIndex = new int[,]
        {
            {1, 1},
            {0, 1},
            {-1, 1},
            {1, 0},
            {0, 0},
            {-1, 0},
            {1, -1},
            {0, -1},
            {-1, -1}
        };
        Test();
    }

    private void Update()
    {        
        if(!roomCleared && enemy.childCount == 0)
        {
            ClearRoom();
        }
    }

    public void Test()
    {
        ReplaceWallTile(0, 0);
    }
    public void CheckTileMap()
    {
        wallCheckArray = new bool[wallTileMap.size.x, wallTileMap.size.y];
        for(int i = 0; i < wallTileMap.size.x; i++)
        {
            for(int j = 0; j < wallTileMap.size.y; j++)
            {
                if(wallTileMap.GetTile(new Vector3Int(i, j, 0)))
                {
                    wallCheckArray[i, j] = true;
                }
            }
        }
    }

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

    private bool IsInRange(int min, int val, int max)
    {
        return min <= val && val < max;
    }

    public void ReplaceWallTile(int stage, int theme)
    {
        for (int i = wallTileMap.origin.x; i < wallTileMap.origin.x + wallTileMap.size.x; i++)
        {
            for (int j = wallTileMap.origin.y; j < wallTileMap.origin.y + wallTileMap.size.y; j++)
            {
                if (wallTileMap.HasTile(new Vector3Int(i, j, 0)))
                {
                    string s = "";
                    for(int k = 0; k < 9; k++)
                    {
                        s += wallTileMap.HasTile(new Vector3Int(i + adjIndex[k, 0], j + adjIndex[k, 1], 0)) ? "O" : "X";
                    }
                    Tile a = ScriptableObject.CreateInstance<Tile>();
                    a.sprite = TileCombiner.Instance.GetCombinedTile(stage, theme, s);
                    wallTileMap.SetTile(new Vector3Int(i, j, 0), a);
                }
            }
        }
    }
}


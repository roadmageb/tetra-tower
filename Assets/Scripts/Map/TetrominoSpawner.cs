using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoSpawner : MonoBehaviour
{
    public GameObject[] prefab_tetrominos;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnNth(int i)
    {
        Debug.Log(prefab_tetrominos[i]);
        GameObject nextTetromino = Instantiate(prefab_tetrominos[i], new Vector3(5.0f, 20.0f, 0.0f), Quaternion.identity) as GameObject;
    }
}

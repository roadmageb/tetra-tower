using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoSpawner : MonoBehaviour
{
    public GameObject[] prefab_tetrominos;
    public Shuffler shapeShuffler = new Shuffler(7);
    public Shuffler colorShuffler = new Shuffler(4);

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnShuffled()
    {
        var shapeIdx = shapeShuffler.retrieve();
        // var colorIdx = colorShuffler.retrieve(); // TODO
        spawnNth(shapeIdx);

    }

    public void spawnNth(int i)
    {
        GameObject nextTetromino = Instantiate(prefab_tetrominos[i], new Vector3(5.0f, 16.0f, 0.0f), Quaternion.identity) as GameObject;
    }
}

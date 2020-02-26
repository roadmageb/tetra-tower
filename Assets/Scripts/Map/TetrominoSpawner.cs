using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoSpawner : MonoBehaviour
{
    public Map map;
    public GameObject[] prefab_tetrominos;
    public Shuffler shapeShuffler = new Shuffler(7);
    public Shuffler colorShuffler = new Shuffler(4);

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Initialize(Map map)
    {
        this.map = map;
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
        var initialGridPosition = new Vector3Int(5, 18, 2);
        GameObject nextTetrominoObj = Instantiate(prefab_tetrominos[i]) as GameObject;

        Tetromino nextTetromino = nextTetrominoObj.GetComponent<Tetromino>();
        nextTetromino.transform.localScale += map.scaleVector;

        if (i == 4) // square
        {
            nextTetromino.allowRotation = false;
        }
        nextTetromino.Initialize(map, initialGridPosition);

        map.currentTetromino = nextTetromino;

        //nextTetromino.transform.parent = map.transform;
    }
}

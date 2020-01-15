using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Start is called before the first frame update
    public static int count = 0;
    public static string[] tetrominoNames = {"Tetromino_T",
        "Tetromino_Long",
        "Tetromino_Square",
        "Tetromino_J",
        "Tetromino_L",
        "Tetromino_S",
        "Tetromino_Z"};

    public static int[] indices = { 0, 1, 2, 3, 4, 5, 6 };

    public static int gridWidth = 10;
    public static int gridHeight = 20;
    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    void shuffleIndices() {
        var result = indices;
        for (var i = 0; i < result.Length; ++i) {
            var j = Random.Range(0, result.Length);

            var tmp = result[i];
            result[i] = result[j];
            result[j] = tmp;
        }
    }

    int GetRandomIntFromShuffle() {
        if (count == 0) {
            shuffleIndices();
        }
        var idx = count;

        count++;
        if (count == 7) {
            count = 0;
        }

        return indices[idx];
    }

    // Start is called before the first frame update
    void Start() {
        SpawnNextTetromino();
    }

    public bool isFullRowAt(int y) {
        for (int x = 0; x < gridWidth; ++x) {
            if (grid[x, y] == null) {
                return false;
            }
        }

        return true;
    }

    public void DeleteMinoAt(int y) {
        for (int x = 0; x < gridWidth; ++x) {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public void MoveRowDown(int y) {
        for (int x = 0; x < gridWidth; ++x) {
            if (grid[x, y] != null) {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void MoveAllRowsDown(int y) {
        for (int i = y; i < gridHeight; ++i) {
            MoveRowDown(i);
        }
    }

    public void DeleteRow() {
        for (int y = 0; y < gridHeight; ++y) {
            if (isFullRowAt(y)) {
                DeleteMinoAt(y);

                MoveAllRowsDown(y + 1);

                --y;
            } else {
                //Debug.Log($"{y} is not full\n");
            }
        }
    }

    public void UpdateGrid(Tetromino tetromino) {
        for (int y = 0; y < gridHeight; ++y) {
            for (int x = 0; x < gridWidth; ++x) {
                if (grid[x, y] != null) {
                    if (grid[x, y].parent == tetromino.transform) {
                        grid[x, y] = null;
                    }
                }
            }
        }

        foreach (Transform mino in tetromino.transform) {
            Vector2 pos = Round(mino.position);

            if (pos.y < gridHeight) {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }

    public Transform GetTransformAtGridPosition(Vector3 pos) {

        if (pos.y > gridHeight - 1) {
            return null;
        } else {
            return grid[(int)pos.x, (int)pos.y];
        }
    }

    public void SpawnNextTetromino() {
        var randInt = GetRandomIntFromShuffle();
        GameObject tetSpawn = GameObject.Find("TetrominoSpawnerObj");
        tetSpawn.GetComponent<TetrominoSpawner>().SpawnNth(randInt);
        /*
        GameObject nextTetromino = (GameObject)Instantiate(
            Resources.Load(
                TetrominoPath,
                typeof(GameObject)),
            new Vector2(5.0f, 20.0f),
            Quaternion.identity);
        */
        //var nextTetrominoPrefab = Resources.Load(TetrominoPath);

        /*
        GameObject nextTetrominoPrefab = tetSpawn.prefab_J;
        Debug.Log(nextTetrominoPrefab);
        GameObject nextTetromino = Instantiate(nextTetrominoPrefab, new Vector2(5.0f, 20.0f), Quaternion.identity) as GameObject;
        */
    }

    public bool CheckIsInsideGrid(Vector3 pos) {
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }

    public Vector3 Round(Vector3 pos) {
        return new Vector3((int)Mathf.Round(pos.x), (int)Mathf.Round(pos.y), 0);
    }
}

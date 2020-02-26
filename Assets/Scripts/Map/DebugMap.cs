using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMap : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject minoPrefab;
    Map map;

    GameObject[,] grid;

    void Start()
    {
        map = GameObject.Find("Map").GetComponent<Map>();
        grid = new GameObject[Map.gridWidth, Map.gridHeight];
    }

    public void Initialize(Map map)
    {
        this.map = map;
    }

    // Update is called once per frame
    void Update()
    {
        for (int x = 0; x < Map.gridWidth; ++x)
        {
            for (int y = 0; y < Map.gridHeight; ++y)
            {
                UpdateDebugGrid(x, y);
            }

        }
    }

    void UpdateDebugGrid(int x, int y)
    {
        if (map.grid[x, y] != null && grid[x, y] == null)
        {
            grid[x, y] = GameObject.Instantiate(minoPrefab, basePosition + new Vector3((float)x, (float)y, 0.0f), Quaternion.identity);
            grid[x, y].transform.parent = transform;
        }
        else if (map.grid[x, y] == null && grid[x, y] != null)
        {
            Destroy(grid[x, y].gameObject);
        }
    }

    public Vector3 basePosition
    { 
        get {
            return transform.position + new Vector3(1, 1, 0);
        } 
    }

    public Vector3 relativePosition(Vector3 vec)
    {
        return vec - basePosition;
    }
}

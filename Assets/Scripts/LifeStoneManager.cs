using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStoneManager : Singleton<LifeStoneManager>
{
    public GameObject lifeStoneTop, lifeStoneMiddle, lifeStoneBottom;

    private Transform lifeStoneUI;
    private int rowSize, columnSize;
    private LifeStoneType[,] lifeStone = new LifeStoneType[20, 3];
    [SerializeField] private float lifeStoneFrameOffset = 75;
    [SerializeField] private float lifeStoneEdgeOffset = 44.64285f;
    [SerializeField] private Vector2 lifeStoneFrameInitialPos = new Vector2(250, 150);

    private void GetLifeStone(int amount, LifeStoneType type)
    {

    }

    /// <summary>
    /// Create randomized shape of life stone.
    /// </summary>
    /// <param name="amount">Total amount of life stone</param>
    /// <param name="goldAmount">Amount of gold life stone</param>
    /// <returns>Encoded string of shape of created life stone</returns>
    private string CreateLifeStoneShape(int amount, int goldAmount = 0)
    {
        int xSize = Mathf.Min(3, amount), ySize = amount;
        bool[,] lifeStonePos = new bool[ySize, xSize];
        lifeStonePos[Random.Range(0, ySize), Random.Range(0, xSize)] = true;

        List<Vector2Int> cands = new List<Vector2Int>();
        for (int i = 1; i < amount; i++)
        {
            for (int y = 0; y < ySize; y++)
            {
                for(int x = 0; x < xSize; x++)
                {
                    if(!lifeStonePos[y, x] && ((x > 0 && lifeStonePos[y, x - 1]) || (x < xSize - 1 && lifeStonePos[y, x + 1]) || (y > 0 && lifeStonePos[y - 1, x]) || (y < ySize - 1 && lifeStonePos[y + 1, x])))
                    {
                        cands.Add(new Vector2Int(x, y));
                    }
                }
            }
            if (cands.Count == 0) break;
            int randomIndex = Random.Range(0, cands.Count);
            lifeStonePos[cands[randomIndex].y, cands[randomIndex].x] = true;
            cands.Clear();
        }

        string result = "";
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                if(lifeStonePos[y, x])
                {
                    result += ((int)LifeStoneType.Normal).ToString();
                }
                else
                {
                    result += ((int)LifeStoneType.NULL).ToString();
                }
            }
        }

        return result;
    }

    public void ExpandRow(int row)
    {
        int previousRow = rowSize;
        rowSize += row;
        GameObject frameTop = lifeStoneUI.Find("LifeStoneFrameTop(Clone)").gameObject;
        for(int i = previousRow; i < rowSize; i++)
        {
            Instantiate(lifeStoneMiddle, lifeStoneFrameInitialPos + new Vector2(0, i * lifeStoneFrameOffset), Quaternion.identity, lifeStoneUI);
        }
        frameTop.GetComponent<RectTransform>().position = lifeStoneFrameInitialPos + new Vector2(0, (rowSize - 1) * lifeStoneFrameOffset + lifeStoneEdgeOffset);
    }

    public void InitiateLifeStone(int row, int column)
    {
        rowSize = row;
        columnSize = column;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                lifeStone[i, j] = LifeStoneType.NULL;
            }
        }
        Instantiate(lifeStoneBottom, lifeStoneFrameInitialPos + new Vector2(0, -lifeStoneEdgeOffset), Quaternion.identity, lifeStoneUI);
        for (int i = 0; i < row; i++)
        {
            Instantiate(lifeStoneMiddle, lifeStoneFrameInitialPos + new Vector2(0, i * lifeStoneFrameOffset), Quaternion.identity, lifeStoneUI);
        }
        Instantiate(lifeStoneTop, lifeStoneFrameInitialPos + new Vector2(0, (row - 1) * lifeStoneFrameOffset + lifeStoneEdgeOffset), Quaternion.identity, lifeStoneUI);
    }

    private void Awake()
    {
        lifeStoneUI = GameObject.Find("LifeStoneUI").transform;
    }


    // Start is called before the first frame update
    void Start()
    {
        InitiateLifeStone(6, 3);
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            string temp = CreateLifeStoneShape(3, 0);
            Debug.Log(temp[0] + "" + temp[1] + "" + temp[2]);
            Debug.Log(temp[3] + "" + temp[4] + "" + temp[5]);
            Debug.Log(temp[6] + "" + temp[7] + "" + temp[8]);
        }
    }
}

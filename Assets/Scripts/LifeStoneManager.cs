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

    private string CreateLifeStoneShape(int amount, int goldAmount = 0)
    {
        return null;
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

    }
}

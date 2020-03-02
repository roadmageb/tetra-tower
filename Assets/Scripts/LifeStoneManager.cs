using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStoneInfo
{
    public string lifeStonePos;
    public Vector2Int size;
    public LifeStoneInfo(string _lifeStonePos, Vector2Int _size)
    {
        lifeStonePos = _lifeStonePos;
        size = _size;
    }
}

public class LifeStoneManager : MonoBehaviour
{
    public GameObject lifeStoneTop, lifeStoneMiddle, lifeStoneBottom;
    public GameObject lifeStoneNormal;
    private Transform lifeStoneUI;
    private int rowSize, columnSize;
    private LifeStone[,] lifeStoneGrid = new LifeStone[20, 3];

    [SerializeField] private float lifeStoneFrameOffset = 75;
    [SerializeField] private float lifeStoneEdgeOffset = 44.64285f;
    [SerializeField] private Vector2 lifeStoneInitialPos = new Vector2(250, 150);

    private void GetLifeStone(LifeStoneInfo lifeStoneInfo)
    {
        //Check possible && lowest position of new life stone
        int minY = rowSize;
        List<Vector2Int> minPosCands = new List<Vector2Int>();

        for (int i = 0; i < 4 - lifeStoneInfo.size.x; i++)
        {
            //Make initial matrix without new life stone
            LifeStoneType[,] lifeStoneDownTest = new LifeStoneType[rowSize + lifeStoneInfo.size.y, columnSize];
            for (int y = 0; y < rowSize; y++)
            {
                for (int x = 0; x < columnSize; x++)
                {
                    lifeStoneDownTest[y, x] = lifeStoneGrid[y, x] != null ? lifeStoneGrid[y, x].type : LifeStoneType.NULL;
                }
            }

            //Push new life stone to the top of the initial matrix
            for (int y = 0; y < lifeStoneInfo.size.y; y++)
            {
                for (int x = 0; x < lifeStoneInfo.size.x; x++)
                {
                    lifeStoneDownTest[y + rowSize, x + i] = (LifeStoneType)int.Parse(lifeStoneInfo.lifeStonePos[y * lifeStoneInfo.size.x + x].ToString());
                }
            }

            //Find the lowest position of the new life stone with current x offset
            bool moveDown = true;
            for (int j = rowSize - 1; moveDown && j >= 0; j--)
            {
                for (int y = 0; moveDown && y < lifeStoneInfo.size.y; y++)
                {
                    for (int x = 0; moveDown && x < lifeStoneInfo.size.x; x++)
                    {
                        if((LifeStoneType)int.Parse(lifeStoneInfo.lifeStonePos[y * lifeStoneInfo.size.x + x].ToString()) != LifeStoneType.NULL)
                        {
                            //Debug.Log("j : " + j + " i : " + i + " x : " + (x + i) + " y : " + (y + j) + " " + lifeStoneDownTest[y + j, x + i]);
                            if (lifeStoneDownTest[y + j, x + i] != LifeStoneType.NULL)
                            {
                                //Debug.Log("j : " + j + " i : " + i + " x : " + (x + i) + " y : " + (y + j) + " False");
                                //Mark lowest position
                                moveDown = false;
                                if (minY > j + 1)
                                {
                                    minY = j + 1;
                                    minPosCands.Clear();
                                    //Debug.Log(new Vector2Int(i, j + 1) + "accept");
                                    minPosCands.Add(new Vector2Int(i, j + 1));
                                }
                                else if (minY == j + 1)
                                {
                                    //Debug.Log(new Vector2Int(i, j + 1) + "accept");
                                    minPosCands.Add(new Vector2Int(i, j + 1));
                                }
                                break;
                            }
                            lifeStoneDownTest[y + j, x + i] = lifeStoneDownTest[y + j + 1, x + i];
                            lifeStoneDownTest[y + j + 1, x + i] = LifeStoneType.NULL;
                        }
                    }
                }
            }

            //If the lowest pos is floor, mark floor
            if (moveDown)
            {
                if (minY > 0)
                {
                    minY = 0;
                    minPosCands.Clear();
                }
                //Debug.Log(new Vector2Int(i, 0) + "accept floor");
                minPosCands.Add(new Vector2Int(i, 0));
            }
        }

        //If current height is not enough to take the whole new life stone
        if (minY + lifeStoneInfo.size.y - 1 >= rowSize)
        {
            Debug.Log("Height exceeded");

            int cutSize = rowSize - minY;
            string remainingLifeStoneInfo = "";

            for (int y = cutSize; y < lifeStoneInfo.size.y; y++)
            {
                for (int x = 0; x < lifeStoneInfo.size.x; x++)
                {
                    remainingLifeStoneInfo += int.Parse(lifeStoneInfo.lifeStonePos[y * lifeStoneInfo.size.x + x].ToString());
                }
            }

            //Need to work later
            //Make cut off life stone to be eaten
            Debug.Log(remainingLifeStoneInfo);

            lifeStoneInfo.size.y = cutSize;
        }

        //Randomize the lowest pos of the new life stone
        int randomizer = Random.Range(0, minPosCands.Count);
        for (int y = 0; y < lifeStoneInfo.size.y; y++)
        {
            for (int x = 0; x < lifeStoneInfo.size.x; x++)
            {
                if ((LifeStoneType)int.Parse(lifeStoneInfo.lifeStonePos[y * lifeStoneInfo.size.x + x].ToString()) != LifeStoneType.NULL)
                {
                    Vector2Int newPos = new Vector2Int(x + minPosCands[randomizer].x, y + minPosCands[randomizer].y);
                    CreateLifeStone(y + rowSize, newPos, (LifeStoneType)int.Parse(lifeStoneInfo.lifeStonePos[y * lifeStoneInfo.size.x + x].ToString()));
                    PlayerController.Instance.hp++;
                }
            }
        }

        //For test
        //Debug current life stone grid
        /*for (int y = rowSize - 1; y >= 0; y--)
        {
            string temp = "";
            for (int x = 0; x < columnSize; x++)
            {
                temp += lifeStoneGrid[y, x] != null ? lifeStoneGrid[y, x].type : LifeStoneType.NULL;
            }
            Debug.Log(temp);
        }*/
    }

    private void CreateLifeStone(float initialPos, Vector2Int pos, LifeStoneType type)
    {
        var temp = Instantiate(lifeStoneNormal, lifeStoneInitialPos + new Vector2(pos.x - 1, initialPos) * lifeStoneFrameOffset, 
            Quaternion.identity, lifeStoneUI).GetComponent<LifeStone>();
        temp.type = type;
        temp.pos = pos;
        lifeStoneGrid[pos.y, pos.x] = temp;
        StartCoroutine(LifeStoneDropCoroutine(temp, lifeStoneInitialPos + new Vector2(pos.x - 1, pos.y) * lifeStoneFrameOffset));
    }

    private IEnumerator LifeStoneDropCoroutine(LifeStone lifeStone, Vector2 dest)
    {
        float velocity = 0, accel = 0.1f;
        while(lifeStone.transform.position.y > dest.y)
        {
            yield return null;
            velocity -= accel;
            lifeStone.transform.position += new Vector3(0, velocity);
        }
        lifeStone.transform.position = dest;
    }

    /// <summary>
    /// Create randomized shape of life stone.
    /// </summary>
    /// <param name="amount">Total amount of life stone</param>
    /// <param name="goldAmount">Amount of gold life stone</param>
    /// <returns>Encoded string of shape of created life stone</returns>
    public LifeStoneInfo CreateLifeStoneShape(int amount, int goldAmount = 0)
    {
        int xSize = Mathf.Min(columnSize, amount), ySize = amount;
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

        int maxX = 0, minX = 2;
        int maxY = 0, minY = 19;

        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                if(lifeStonePos[y, x])
                {
                    if (minX > x) minX = x;
                    if (maxX < x) maxX = x;
                    if (minY > y) minY = y;
                    if (maxY < y) maxY = y;
                }
            }
        }

        string result = "";
        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
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

        return new LifeStoneInfo(result, new Vector2Int(maxX - minX + 1, maxY - minY + 1));
    }

    public void ExpandRow(int row)
    {
        int previousRow = rowSize;
        rowSize += row;
        GameObject frameTop = lifeStoneUI.Find("LifeStoneFrameTop(Clone)").gameObject;
        for(int i = previousRow; i < rowSize; i++)
        {
            Instantiate(lifeStoneMiddle, lifeStoneInitialPos + new Vector2(0, i * lifeStoneFrameOffset), Quaternion.identity, lifeStoneUI);
        }
        frameTop.GetComponent<RectTransform>().position = lifeStoneInitialPos + new Vector2(0, (rowSize - 1) * lifeStoneFrameOffset + lifeStoneEdgeOffset);
    }

    public void InitiateLifeStone(int row, int column)
    {
        rowSize = row; columnSize = column;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                lifeStoneGrid[i, j] = null;
            }
        }
        Instantiate(lifeStoneBottom, lifeStoneInitialPos + new Vector2(0, -lifeStoneEdgeOffset), Quaternion.identity, lifeStoneUI);
        for (int i = 0; i < row; i++)
        {
            Instantiate(lifeStoneMiddle, lifeStoneInitialPos + new Vector2(0, i * lifeStoneFrameOffset), Quaternion.identity, lifeStoneUI);
        }
        Instantiate(lifeStoneTop, lifeStoneInitialPos + new Vector2(0, (row - 1) * lifeStoneFrameOffset + lifeStoneEdgeOffset), Quaternion.identity, lifeStoneUI);
    }

    public void DestroyLifeStone(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            //If there is no more life stone to destroy, game over
            if (PlayerController.Instance.hp == 0)
            {
                //Make it later
                Debug.Log("Game Over");
                return;
            }

            List<LifeStone> cands = new List<LifeStone>();
            for (int y = rowSize - 1; y >= 0; y--)
            {
                for (int x = 0; x < columnSize; x++)
                {
                    if (lifeStoneGrid[y, x] != null)
                    {
                        cands.Add(lifeStoneGrid[y, x]);
                    }
                }
                if(cands.Count != 0)
                {
                    break;
                }
            }

            int randomIndex = Random.Range(0, cands.Count);
            lifeStoneGrid[(int)cands[randomIndex].pos.y, (int)cands[randomIndex].pos.x] = null;
            Destroy(cands[randomIndex].gameObject);
            PlayerController.Instance.hp--;
        }
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
            LifeStoneInfo temp = CreateLifeStoneShape(3, 0);
            /*for(int i = temp.lifeStonePos.Length / temp.size.x - 1; i >= 0; i--) 
            {
                string oneLine = "";
                for(int j = 0; j < temp.size.x; j++)
                {
                    oneLine += temp.lifeStonePos[i * (int)temp.size.x + j] + "";
                }
                Debug.Log(oneLine);
            }*/
            GetLifeStone(temp);


            /*CreateLifeStone(new Vector2Int(2, 0), LifeStoneType.Normal);
            CreateLifeStone(new Vector2Int(1, 1), LifeStoneType.Normal);
            CreateLifeStone(new Vector2Int(2, 1), LifeStoneType.Normal);



            GetLifeStone(new LifeStoneInfo("1011", new Vector2Int(2, 2)));*/
        }

    }
}

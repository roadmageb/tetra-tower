using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCombiner : Singleton<TileCombiner>
{
    public const int tileSize = 32;
    public int halfSize
    {
        get => tileSize / 2;
    }
    public Dictionary<string, Sprite>[,] combinedTiles;
    public int stageNum = 1;
    public int[] themeNum;
    public const int tileNumPerPart = 5;
    //"OOO" "OOX" "OXO" "XOO" "XXO"
    public Texture2D[] tileTexture;
    //W
    //[1][0]
    //[3][2]
    //
    //[D][V][D]
    //[H][*][H]
    //[D][V][D]
    //"W_HVD"
    private Dictionary<string, Texture2D>[,] seperatedTiles;

    public int[,] mergeIndex, mergeCoordIndex;
    
    public void Test()
    {
        GameObject.Find("TestSprite").GetComponent<SpriteRenderer>().sprite = GetCombinedTile(0, 0, "XXXOOXXXX");
    }
    private void Start()
    {
        mergeIndex = new int[,]
        {
            {3, 1, 0},
            {5, 1, 2},
            {3, 7, 6},
            {5, 7, 8}
        };
        mergeCoordIndex = new int[,]
        {
            {1, 1},
            {0, 1},
            {1, 0},
            {0, 0}
        };

        Pair<string, int>[] cutIndex = new Pair<string, int>[]
        {
            new Pair<string, int>("OOO", 0),
            new Pair<string, int>("OOX", 1),
            new Pair<string, int>("OXO", 2),
            new Pair<string, int>("OXX", 2),
            new Pair<string, int>("XOO", 3),
            new Pair<string, int>("XOX", 3),
            new Pair<string, int>("XXO", 4),
            new Pair<string, int>("XXX", 4)
        };

        combinedTiles = new Dictionary<string, Sprite>[stageNum, Mathf.Max(themeNum)];
        seperatedTiles = new Dictionary<string, Texture2D>[stageNum, Mathf.Max(themeNum)];
        for(int i = 0; i < stageNum; i++)
        {
            for(int j = 0; j < themeNum[i]; j++)
            {
                combinedTiles[i, j] = new Dictionary<string, Sprite>();
                seperatedTiles[i, j] = new Dictionary<string, Texture2D>();
            }
        }
        for(int i = 0; i < stageNum; i++)
        {
            for (int j = 0; j < themeNum[i]; j++)
            {
                for(int k = 0; k < 4; k++)
                foreach(Pair<string, int> p in cutIndex)
                {
                    Texture2D tx = new Texture2D(halfSize, halfSize);
                    tx.SetPixels(0, 0, halfSize, halfSize, tileTexture[i].GetPixels((k * tileNumPerPart + p.k) * halfSize, halfSize * j, halfSize, halfSize));
                    tx.Apply();
                    seperatedTiles[i, j].Add(k + "_" + p.t, tx);
                }
            }
        }

        Test();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stage"></param>
    /// <param name="theme"></param>
    /// <param name="index">
    /// [2][1][0]
    /// [5][O][3]
    /// [8][7][6]
    /// </param>
    /// <returns></returns>
    public Sprite GetCombinedTile(int stage, int theme, string index)
    {
        if(!combinedTiles[stage, theme].ContainsKey(index))
        {
            Texture2D tx = new Texture2D(tileSize, tileSize);
            for(int i = 0; i < 4; i++)
            {
                tx.SetPixels(
                    halfSize * mergeCoordIndex[i, 0],
                    halfSize * mergeCoordIndex[i, 1],
                    halfSize,
                    halfSize,
                    seperatedTiles[stage, theme][i + "_" + index[mergeIndex[i, 0]] + index[mergeIndex[i, 1]] + index[mergeIndex[i, 2]]].GetPixels());
                tx.Apply();
            }
            combinedTiles[stage, theme].Add(index, Sprite.Create(tx, new Rect(0, 0, tileSize, tileSize), new Vector2(0.5f, 0.5f), 32));
        }
        return combinedTiles[stage, theme][index];
    }
}

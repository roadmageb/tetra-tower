using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGridUtils : MonoBehaviour
{

    GridUtils gu;

    Transform[,] emptyGrid;
    // Start is called before the first frame update
    void Start()
    {
        emptyGrid = new Transform[10,9];
        gu = new GridUtils();
        gu.Initialize(emptyGrid);
        gu.Test();

    }
}

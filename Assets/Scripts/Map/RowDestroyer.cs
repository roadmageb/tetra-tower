using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowDestroyer : MonoBehaviour
{
    public Map map;
    public bool destroyFinished;
    public bool requested;
    public int requestCount;

    void Start()
    {
        destroyFinished = false;
        requested = false;
        requestCount = 0;
    }

    public void Initialize(Map map)
    {
        this.map = map;
    }

    public void Reset()
    {
        destroyFinished = false;
    }

    // Update is called once per frame  
    void Update()   
    {
        if (requested)
        {
            if (Piston.pistonCount > 0)
            {
                return;
            }
            else
            {
                map.inputLock = true;

                map.gridUtils.isFullUpdate();
                map.gridUtils.shiftAmountUpdate();
                map.DestroyRowsIfFull(map.gridUtils.isFull);
                map.MoveAllRowsDown(map.gridUtils.isFull, map.gridUtils.shiftDown);

                destroyFinished = true;
                requested = false;
                return;
            }
        }
    }

    public void RequestDestroyRows()
    {
        requested = true;
        requestCount++;
    }
}

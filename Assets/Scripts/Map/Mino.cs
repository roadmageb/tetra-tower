using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour
{
    public bool toBeDestroyed;

    [SerializeField]
    public int fallDestination;

    public Vector3Int localPosition;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localPosition = localPosition;
    }

    public Vector3Int GetGridPosition()
    {
        return GetComponentInParent<Tetromino>().gridPosition + localPosition;
    }

}

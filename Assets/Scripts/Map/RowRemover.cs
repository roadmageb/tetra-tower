using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowRemover : MonoBehaviour
{
    // Start is called before the first frame update
    public Map map;
    void Start()
    {
        map = GameObject.FindGameObjectWithTag("MapTag").GetComponent<Map>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void RemoveRow(int row)
    {
        StartCoroutine(RemoveRowCoroutine(row));
    }

    IEnumerator RemoveRowCoroutine(int row)
    {
        yield return new WaitForSeconds(3);
    }
} 
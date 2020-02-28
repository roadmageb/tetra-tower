using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate float FuncToMap(float x);

public static class Vector3Utils
{
    public static Vector3 Map(Vector3 vec, FuncToMap func)
    {
        return new Vector3(func(vec.x), func(vec.y), func(vec.z));
    }

    public static Vector3Int ToVector3Int(Vector3 vec)
    {
        int x = (int)Mathf.Round(vec.x);
        int y = (int)Mathf.Round(vec.y);
        int z = (int)Mathf.Round(vec.z);
        return new Vector3Int(x, y, z);
    }
}

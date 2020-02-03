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

    public static Vector3 newVector3From(Vector3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }

}

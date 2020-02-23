using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pair<T, K>
{
    public T t;
    public K k;
    public Pair (T t, K k)
    {
        this.t = t;
        this.k = k;
    }
}

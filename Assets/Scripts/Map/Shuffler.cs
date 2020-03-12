using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuffler
{
    int count = 0;

    int[] buffer;

    public Shuffler(int length)
    {
        buffer = new int[length];
        for( int i = 0; i < length; ++i)
        {
            buffer[i] = i;
        }
    }

    public int retrieve()
    {
        if (count == 0)
        {
            Shuffle(buffer);
        }

        var element = buffer[count++];
        if (count == buffer.Length)
        {
            count = 0;
        }

        return element;
    }

    void Shuffle(int[] arr)
    {

        for (var i = 0; i < arr.Length; ++i)
        {
            var j = Random.Range(0, arr.Length);

            var tmp = arr[i];
            arr[i] = arr[j];
            arr[j] = tmp;
        }
    }
}

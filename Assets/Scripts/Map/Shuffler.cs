using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuffler : MonoBehaviour
{
    int count = 0;
    public int retrieve(int[] shuffled)
    {
        if (count == 0)
        {
            Shuffler.Shuffle(shuffled);
        }

        var element = shuffled[count++];
        if (count == shuffled.Length)
        {
            count = 0;
        }

        return element;
    }

    public static void Shuffle(int[] arr)
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

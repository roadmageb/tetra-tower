using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    public IEnumerator MoveCamera(Vector3 _dest)
    {
        Vector3 from = transform.position;
        Vector3 to = new Vector3(_dest.x, _dest.y, transform.position.z);
        for (float timer = 0; timer < 0.2f; timer += Time.deltaTime)
        {
            yield return null;
            transform.position = Vector3.Lerp(from, to, timer / 0.2f);
        }
        transform.position = to;
    }
}

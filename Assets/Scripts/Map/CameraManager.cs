using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mapCamera;
    public Camera roomCamera;
    public Camera zoomCamera;
    public Camera currentCamera;

    public int zValue;

    Map map;

    [SerializeField]
    public bool isZooming { get; private set; }

    public CameraManager Init(Map map)
    {
        this.map = map;

        mapCamera = GameObject.Find("Map Camera").GetComponent<Camera>();
        mapCamera.orthographicSize = 12 * map.scaleFactor;
        mapCamera.enabled = false;

        roomCamera = GameObject.Find("Room Camera").GetComponent<Camera>();
        roomCamera.orthographicSize = 8; // exactly fits the room
        Debug.Assert(map.currentMino != null);
        roomCamera.transform.position = map.currentMino.transform.position + new Vector3(0, 0, -10);

        zoomCamera = GameObject.Find("Zoom Camera").GetComponent<Camera>();
        zoomCamera.enabled = false;

        currentCamera = roomCamera;

        isZooming = false;

        zValue = -10;

        return this;
    }
    public IEnumerator Zoom(bool zoomIn)
    {
        isZooming = true;

        var fromCamera = mapCamera;
        var toCamera = roomCamera;

        if (!zoomIn)
        {
            var swap = fromCamera;
            fromCamera = toCamera;
            toCamera = swap;
        }

        zoomCamera.transform.position = fromCamera.transform.position;
        zoomCamera.orthographicSize = fromCamera.orthographicSize;

        zoomCamera.enabled = true;
        fromCamera.enabled = false;
        Debug.Assert(toCamera.enabled == false);


        var step = 100;
        var positionStep = toCamera.transform.position - fromCamera.transform.position;
        positionStep /= step;

        var sizeStep = toCamera.orthographicSize - fromCamera.orthographicSize;
        sizeStep /= step;

        var transitionTime = 2;
        for (int i = 0; i < step; ++i)
        {
            zoomCamera.transform.position += positionStep;
            zoomCamera.orthographicSize += sizeStep;
            yield return new WaitForSeconds(transitionTime / step);
        }

        zoomCamera.enabled = false;
        toCamera.enabled = true;
        Debug.Assert(fromCamera.enabled == false);

        isZooming = false;
    }

    public void MoveTo(Vector3 pos)
    {
        pos.z = -10;
        roomCamera.transform.position = pos;
    }
    public void MoveBy(Vector3 pos)
    {
        roomCamera.transform.position += pos;
    }

    public IEnumerator MoveCamera(Vector3 _dest)
    {
        Vector3 from = roomCamera.transform.position;
        Vector3 to = new Vector3(_dest.x, _dest.y, from.z);
        for (float timer = 0; timer < Room.roomMoveTime; timer += Time.deltaTime)
        {
            yield return null;
            roomCamera.transform.position = Vector3.Lerp(from, to, timer / Room.roomMoveTime);
        }
        roomCamera.transform.position = to;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDraggable : MonoBehaviour
{
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    ///////////////////////////////////////
    // MTouch : Combined mouse and touch //
    ///////////////////////////////////////

    private void OnMouseDown()
    {
        OnMTouchDown(cam.ScreenToWorldPoint(Input.mousePosition));
    }

    private void OnMouseDrag()
    {
        OnMTouchDrag(cam.ScreenToWorldPoint(Input.mousePosition));
    }

    /////////
    // ... //
    /////////

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = cam.ScreenToWorldPoint(touch.position);
            transform.position = touchPosition;
        }
    }

    ///////////////////////////////////////
    // MTouch : Combined mouse and touch //
    ///////////////////////////////////////

    // Vector from transform.position to mouse position
    Vector2 offset = Vector2.zero;

    private void OnMTouchDown(Vector2 pos)
    {
        offset = pos - (Vector2)transform.position;
    }

    private void OnMTouchDrag(Vector2 pos)
    {
        transform.position = pos - offset;
    }
}

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

    // This function works under the assumption that the camera has no rotation, and all interactable objects are at z = 0
    Vector3 MouseToWorldPoint(Vector3 mousePos)
    {
        mousePos.z = -cam.transform.position.z;

        return cam.ScreenToWorldPoint(mousePos);
    }

    private void OnMouseDown()
    {
        OnMTouchDown(MouseToWorldPoint(Input.mousePosition));
    }

    private void OnMouseDrag()
    {
        OnMTouchDrag(MouseToWorldPoint(Input.mousePosition));
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

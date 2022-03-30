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

    Vector2 difference = Vector2.zero;

    private void OnMouseDown()
    {
        difference = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    private void OnMouseDrag()
    {
        transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition) - difference;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = cam.ScreenToWorldPoint(touch.position);
            transform.position = touchPosition;
        }
    }

}

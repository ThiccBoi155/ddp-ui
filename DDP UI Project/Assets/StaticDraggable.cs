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
}

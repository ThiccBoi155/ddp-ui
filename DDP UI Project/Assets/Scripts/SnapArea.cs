using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapArea : MonoBehaviour
{
    public Collider2D col;
    public Collider2D trigger;

    public DragAndClick item = null;

    private void OnMouseDown()
    {
        Debug.Log("Down");
    }

    private void OnMouseDrag()
    {
        Debug.Log("Drag");
    }

    private void OnMouseUp()
    {
        Debug.Log("Up");
    }
}

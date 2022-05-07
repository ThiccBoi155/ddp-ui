using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMTouchable : MTouchable
{
    public Collider2D col;

    public override Collider2D MTouchCollider { get { return col; } }

    public override void OnMTouchDown(MTouch mt)
    {
        Debug.Log("Debug down");
    }

    public override void OnMTouchDrag(MTouch mt)
    {
        Debug.Log("Debug drag");
    }

    public override void OnMTouchUp(MTouch mt)
    {
        Debug.Log("Debug up");
    }
}

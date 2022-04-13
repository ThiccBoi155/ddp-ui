using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MTouchable
{
    Collider2D MTouchCollider { get; }

    bool Grapped { get; set; }

    void AddThisToMTouchController();
    //void RemoveThisWhenDestroyed();

    void OnMTouchDown(Vector2 pos);

    void OnMTouchDrag(Vector2 pos);

    void OnMTouchUp(Vector2 pos);
}

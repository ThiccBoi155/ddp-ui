using UnityEngine;

public abstract class MTouchable: MonoBehaviour
{
    public abstract Collider2D MTouchCollider { get; }

    [HideInInspector]
    public bool grapped;



    protected void Awake()
    {
        AddThisToMTouchController();
    }

    protected void AddThisToMTouchController()
    {
        MTouchController.AddToMTouchables(this);
    }



    public abstract void OnMTouchDown(MTouch mt);

    public abstract void OnMTouchDrag(MTouch mt);

    public abstract void OnMTouchUp(MTouch mt);



    protected void OnDestroy()
    {
        MTouchController.RemoveFromMTouchables(this);
    }
}

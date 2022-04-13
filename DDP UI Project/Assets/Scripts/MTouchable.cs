using UnityEngine;

public abstract class MTouchable: MonoBehaviour
{
    public abstract Collider2D MTouchCollider { get; }

    public abstract bool Grapped { get; set; }



    protected void Awake()
    {
        AddThisToMTouchController();
    }

    protected void AddThisToMTouchController()
    {
        MTouchController.AddToMTouchables(this);
    }



    public abstract void OnMTouchDown(Vector2 pos);

    public abstract void OnMTouchDrag(Vector2 pos);

    public abstract void OnMTouchUp(Vector2 pos);



    protected void OnDestroy()
    {
        MTouchController.RemoveFromMTouchables(this);
    }
}

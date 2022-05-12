using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SnapArea : MonoBehaviour
{
    public Collider2D Area { get { return trigger; } }

    [Header("References")]
    public Collider2D trigger;

    public Disc currentDisc = null;

    [Header("Settings")]
    public Vector3 offset;

    protected bool notHolding = false;

    protected void Awake()
    {
        if (Area == null)
            Debug.Log("The area for SnapArea has not been assigned");

        MTouchController.snapAreas.Add(this);
    }

    protected void Update()
    {
        if (currentDisc != null)
            currentDisc.transform.position = transform.position + offset;
    }

    public virtual void Enter(MTouch mt)
    {
        //Debug.Log("Enter");

        Disc disc = mt.currentMTble as Disc;

        currentDisc = disc;

        disc.rid.velocity = Vector2.zero;

        disc.rid.constraints = RigidbodyConstraints2D.FreezePosition;

        disc.SetLayerOrderToBack();

        notHolding = true;
    }

    public virtual void Holding()
    {
        //Debug.Log("Holding");
    }

    public virtual void Stay()
    {
        Debug.Log("Stay");

        notHolding = false;
    }

    public virtual void HoldAgain()
    {
        //Debug.Log("Hold again");

        notHolding = true;
    }

    public virtual void Leave()
    {
        //Debug.Log("Leave");

        currentDisc.rid.constraints = RigidbodyConstraints2D.None;

        currentDisc.UpdateLayerOrder();

        currentDisc = null;
    }
}

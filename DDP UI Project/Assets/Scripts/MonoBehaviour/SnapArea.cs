using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapArea : MonoBehaviour
{
    public Collider2D Area { get { return trigger; } }

    [Header("References")]
    public Collider2D col;
    public Collider2D trigger;

    public Disc currentDisc = null;

    [Header("Settings")]
    public Vector3 offset;
    public float angularVelocity;

    private bool notHolding = false;

    private void Awake()
    {
        if (Area == null)
            Debug.Log("The area for SnapArea has not been assigned");

        MTouchController.snapAreas.Add(this);
    }

    private void Update()
    {
        SetCurrentDiscValues();
    }

    void SetCurrentDiscValues()
    {
        if (currentDisc != null)
        {
            currentDisc.transform.position = transform.position + offset;

            currentDisc.rid.angularVelocity = !notHolding ? angularVelocity : 0f;
        }
    }

    public void Enter(MTouch mt)
    {
        Debug.Log("Enter");

        Disc disc = mt.currentMTble as Disc;

        currentDisc = disc;

        disc.rid.velocity = Vector2.zero;

        disc.rid.constraints = RigidbodyConstraints2D.FreezePosition;

        disc.SetLayerOrderToBack();

        notHolding = true;
    }

    public void Holding()
    {
        Debug.Log("Holding");
    }

    public void Stay()
    {
        Debug.Log("Stay");

        notHolding = false;
    }

    public void HoldAgain()
    {
        Debug.Log("Hold again");

        notHolding = true;
    }

    public void Leave()
    {
        Debug.Log("Leave");

        currentDisc.rid.constraints = RigidbodyConstraints2D.None;

        currentDisc.UpdateLayerOrder();

        currentDisc = null;
    }
}

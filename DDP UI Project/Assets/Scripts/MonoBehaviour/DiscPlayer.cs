using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscPlayer : SnapArea
{
    public float angularVelocity;

    protected new void Update()
    {
        SetCurrentDiscValues();
        base.Update();
    }

    void SetCurrentDiscValues()
    {
        if (currentDisc != null)
        {
            currentDisc.rid.angularVelocity = !notHolding ? angularVelocity : 0f;
        }
    }

    public new void Enter(MTouch mt)
    {
        base.Enter(mt);
    }

    public new void Holding()
    {
        base.Holding();
    }

    public new void Stay()
    {
        base.Stay();
    }

    public new void HoldAgain()
    {
        base.HoldAgain();
    }

    public new void Leave()
    {
        base.Leave();
    }
}

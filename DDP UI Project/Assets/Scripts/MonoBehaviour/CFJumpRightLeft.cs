using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFJumpRightLeft : SimpleButton
{
    public bool right = true;

    [Header("Refrences")]
    public CoverFlow cf;
    

    protected new void Awake()
    {
        base.Awake();
    }

    protected override void ClickAction()
    {
        if (cf != null)
        {
            if (right)
                cf.jumpRight = true;
            else
                cf.jumpLeft = true;
        }
    }
}

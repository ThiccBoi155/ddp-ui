using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMoveCoverButton : SimpleButton
{
    [Header("Refrences")]
    public CoverFlow cf;

    protected new void Awake()
    {
        base.Awake();
    }

    protected override void ClickAction()
    {
        if (cf != null)
            cf.ToggleMoveCover();
    }
}

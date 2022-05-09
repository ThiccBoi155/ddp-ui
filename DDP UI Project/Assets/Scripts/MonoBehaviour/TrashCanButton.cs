using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanButton : SimpleButton
{
    [Header("Refrences")]
    public DiscTrashCan dtc;

    public override Collider2D MTouchCollider { get { return dtc.dragTrigger; } }

    protected override void ClickAction()
    {
        if (dtc != null)
            dtc.ToggleOpen();
    }
}

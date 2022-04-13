using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : DragAndClick
{
    public SpriteRenderer coverArt;

    [HideInInspector]
    public CoverFlow cf;

    public void SetCoverArt(Sprite sprite)
    {
        coverArt.sprite = sprite;
    }

    protected override void ClickAction()
    {
        // Nothing
    }

    // It still works without "new"-keyword, since the base function is also called
    private new void OnDestroy()
    {
        base.OnDestroy();

        if (cf != null)
            cf.RemoveDiscFromList(this);
    }
}

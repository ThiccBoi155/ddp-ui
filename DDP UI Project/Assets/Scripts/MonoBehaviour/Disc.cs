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

    private void OnDestroy()
    {
        if (cf != null)
            cf.RemoveDiscFromList(this);
    }
}

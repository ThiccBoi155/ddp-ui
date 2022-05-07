using UnityEngine;

public class Disc : DragAndClick
{
    [Header("References")]
    public SpriteRenderer coverArt;
    public ShowDiscInfo showDiscInfo;

    [HideInInspector]
    public CoverFlow cf;
    //[HideInInspector]
    //public Cover cov;

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
        
        /*/
        if (cov != null)
            cov.RemoveDiscFromList(this);
        //*/
    }
}

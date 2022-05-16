using UnityEngine;
using System.Collections;

public class SkipButton : SimpleButton
{
    [Header("Refrences")]
    public DiscPlayer discPlayer;

    protected override void ClickAction()
    {
        if (discPlayer != null)
            discPlayer.RemoveDiscNow();
    }
}

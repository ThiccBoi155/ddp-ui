using UnityEngine;

public class Disc : DragAndClick
{
    [Header("References")]
    public SpriteRenderer coverArt;
    public ShowDiscInfo showDiscInfo;
    public AudioSource audioSource;
    public DiscValues dv;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log($"Mag: {rid.velocity.magnitude}");

        float speed = rid.velocity.magnitude;

        // Value between 0 and 1...
        float impact = (speed - dv.minSpeedRange) / (dv.maxSpeedRange - dv.minSpeedRange);
        impact = Mathf.Clamp01(impact);

        if (!dv.invertPitchRange)
            audioSource.pitch = Mathf.Lerp(dv.minPitch, dv.maxPitch, impact);
        else
            audioSource.pitch = Mathf.Lerp(dv.maxPitch, dv.minPitch, impact);

        audioSource.volume = Mathf.Lerp(dv.minVolume, dv.maxVolume, impact);

        if (dv.minSpeedRange < speed)
            audioSource.Play();
    }
}

using UnityEngine;
using System;

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

    private void Update()
    {
        CapAudio();
    }

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

    private void CapAudio()
    {
        if (dv.maxAudioClipLength < audioSource.time)
            audioSource.Stop();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float speed = rid.velocity.magnitude;

        // Value between 0 and 1...
        float impact = (speed - dv.minSpeedRange) / (dv.maxSpeedRange - dv.minSpeedRange);
        impact = Mathf.Clamp01(impact);

        bool dontPlaySoundHere = false;

        Disc discCollision = collision.gameObject.GetComponent<Disc>();
        DiscTrashCan trashCollision = collision.gameObject.GetComponent<DiscTrashCan>();
        if (discCollision != null)
        {
            Debug.Log("Disc collision");
        }
        else if (trashCollision != null)
        {
            Debug.Log("Trash can collision");
            trashCollision.PlayCollisionSound();

            if (!dv.playHereWhenTrashCanIsHit)
                dontPlaySoundHere = true;
        }
        else
        {
            Debug.Log("Other collision");
        }

        if (!dv.invertPitchRange)
            audioSource.pitch = Mathf.Lerp(dv.minPitch, dv.maxPitch, impact);
        else
            audioSource.pitch = Mathf.Lerp(dv.maxPitch, dv.minPitch, impact);

        audioSource.volume = Mathf.Lerp(dv.minVolume, dv.maxVolume, impact);

        audioSource.clip = dv.audioClip;

        if (dv.minSpeedRange < speed && !dontPlaySoundHere)
            audioSource.Play();
    }

    void MakeSound(float impact, float speed)
    {
        // ...
    }
}

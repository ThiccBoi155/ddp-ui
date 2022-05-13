using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscPlayer : SnapArea
{
    public float angularVelocity;

    [Header("References")]
    public AudioSource audioSource;

    [Header("Music")]
    public List<AudioClip> defaultSongs;

    //public AudioClip currentSong;

    protected new void Update()
    {
        SetCurrentDiscValues();

        SetTimer();

        base.Update();
    }

    void SetCurrentDiscValues()
    {
        if (currentDisc != null)
        {
            currentDisc.rid.angularVelocity = !notHolding ? angularVelocity : 0f;
        }
    }

    void SetTimer()
    {

    }

    public override void Enter(MTouch mt)
    {
        base.Enter(mt);

        SetCurrentSong();
    }

    public override void Holding()
    {
        base.Holding();
    }

    public override void Stay()
    {
        base.Stay();

        audioSource.Play();
    }

    public override void HoldAgain()
    {
        base.HoldAgain();

        audioSource.Pause();
    }

    public override void Leave()
    {
        base.Leave();

        SetCurrentSong();
    }

    private void SetCurrentSong()
    {
        if (currentDisc != null)
        {
            if (currentDisc.songFile != null)
                audioSource.clip = currentDisc.songFile;
            else
            {
                audioSource.clip = defaultSongs[Random.Range(0, defaultSongs.Count)];
            }
        }
        else
            audioSource.clip = null;
    }

    private void PlaySong()
    {
        audioSource.Play();
    }

    private void PauseSong()
    {
        audioSource.Pause();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscPlayer : SnapArea
{
    public float angularVelocity;

    [Header("References")]
    public AudioSource audioSource;
    public DisplaySongTime dst;
    public SpriteRenderer skipButtonRenderer;

    [Header("Music")]
    public List<AudioClip> defaultSongs;

    [Header("Settings")]
    public float maxClickToPause = .8f;
    public Vector3 removeOffset;
    public Vector2 removeVelocity;

    private bool setPaused;
    private float timeSinceHoldAgain = 0f;

    protected new void Awake()
    {
        base.Awake();

        if (defaultSongs.Count < 1)
            Debug.Log("The disc player needs at least one default song");
        if (audioSource == null)
            Debug.Log("No audio source found");
    }

    protected new void Update()
    {
        SetCurrentDiscValues();

        SetTimer();

        if (timeSinceHoldAgain != float.MaxValue)
            timeSinceHoldAgain += Time.deltaTime;

        base.Update();

        RemoveDiscWhenSongIsOver();

        HideSkipButton();
    }

    void SetCurrentDiscValues()
    {
        if (currentDisc != null)
        {
            currentDisc.rid.angularVelocity = !holding && audioSource.isPlaying ? angularVelocity : 0f;
        }
    }

    void SetTimer()
    {
        if (currentDisc != null && audioSource.clip != null)
            dst.Display(audioSource.time, audioSource.clip.length);
    }

    void HideSkipButton()
    {
        if (skipButtonRenderer != null)
            skipButtonRenderer.enabled = (currentDisc != null);
    }

    void RemoveDiscWhenSongIsOver()
    {
        if (audioSource.clip != null && audioSource.time == audioSource.clip.length)
        {
            RemoveDiscNow();
        }
    }

    public void RemoveDiscNow()
    {
        if (currentDisc != null)
        {
            currentDisc.transform.position = transform.position + removeOffset;

            currentDisc.gameObject.layer = LayerMask.NameToLayer("Physics Disc");

            currentDisc.rid.gravityScale = 1f;
            currentDisc.rid.velocity = removeVelocity;

            Leave();
        }
    }

    public override void Enter(Disc disc)
    {
        base.Enter(disc);

        SetCurrentSong();

        setPaused = false;
        timeSinceHoldAgain = float.MaxValue;
    }

    public override void Holding()
    {
        base.Holding();
    }

    public override void Stay()
    {
        base.Stay();

        if (timeSinceHoldAgain <= maxClickToPause && setPaused == false)
        {
            setPaused = true;
        }
        else
        {
            setPaused = false;
            audioSource.Play();
        }
    }

    public override void HoldAgain()
    {
        base.HoldAgain();

        audioSource.Pause();

        timeSinceHoldAgain = 0f;
    }

    public override void Leave()
    {
        base.Leave();

        SetCurrentSong();

        dst.Clear();
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

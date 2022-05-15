using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscTrashCan : SnapArea
{
    public bool Open { get; private set; } = false;

    public float holdScale = 1f;

    [Header("References")]
    public Collider2D dragTrigger;
    public Collider2D throwTrigger;
    public Collider2D col;
    public SpriteRenderer sr;
    public AudioSource audioSource;
    public GameObject trashLid;

    [Header("Sprites")]
    public Sprite closedTrash;
    public Sprite openTrash;
    public Sprite trashNoLid;

    [Header("...")]
    public AudioClip collisionSound;

    [Header("Impact pitch settings")]
    public float minPitch = 1f;
    public float maxPitch = 1f;

    [Header("Toss out (with drag) settings")]
    public float startTossOutDragPitch = .7f;
    public float tossOutDragPitchMultiplier = .1f;
    public int pitchesNum = 4;
    public float resetPitchPosTime = -1f;

    [SerializeField]
    private int pitchPosition = 0;

    private float timeSinceLastToss = 0f;

    public new void Awake()
    {
        base.Awake();

        pitchPosition = 0;
    }

    public new void Update()
    {
        SetDiscScale(holdScale);

        ResetPitchPositionAfterTime();

        base.Update();
    }

    public override void Enter(Disc disc)
    {
        base.Enter(disc);

        HoldDisc();
    }

    public override void Stay()
    {
        Destroy(currentDisc.gameObject);

        CloseNow();

        PlayTossOutSound();

        base.Stay();
    }

    public override void Leave()
    {
        CloseNow();

        SetDiscScale(1f);

        base.Leave();
    }

    private void SetDiscScale(float scale)
    {
        if (currentDisc != null)
            currentDisc.transform.localScale = Vector3.one * scale;
    }

    public void ToggleOpen()
    {
        if (Open)
            CloseNow();
        else
            OpenNow();

        PlayClickSound();
    }

    private void OpenNow()
    {
        Open = true;
        sr.sprite = openTrash;

        if (col != null)
            col.enabled = false;

        if (throwTrigger != null)
            throwTrigger.enabled = true;
    }

    private void CloseNow()
    {
        Open = false;
        sr.sprite = closedTrash;

        if (col != null)
            col.enabled = true;

        if (throwTrigger != null)
            throwTrigger.enabled = false;

        trashLid.SetActive(false);
    }

    private void HoldDisc()
    {
        sr.sprite = trashNoLid;

        trashLid.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Disc>() != null)
            Destroy(collision.gameObject);
    }

    public void PlayClickSound()
    {
        if (audioSource != null)
            audioSource.Play();
    }

    public void PlayCollisionSound(float impact = -1f)
    {
        if (impact != -1f)
            audioSource.pitch = Mathf.Lerp(maxPitch, minPitch, impact);
        else
            audioSource.pitch = 1f;

        if (audioSource != null && collisionSound != null)
            audioSource.PlayOneShot(collisionSound, audioSource.volume);
    }

    private void PlayTossOutSound()
    {
        audioSource.pitch = startTossOutDragPitch + tossOutDragPitchMultiplier * pitchPosition;
        
        pitchPosition = (pitchPosition + 1) % pitchesNum;

        if (audioSource != null && collisionSound != null)
            audioSource.PlayOneShot(collisionSound, audioSource.volume);
    }

    private void ResetPitchPositionAfterTime()
    {
        if (resetPitchPosTime != -1f && pitchPosition != 0)
        {
            timeSinceLastToss += Time.deltaTime;

            if (resetPitchPosTime <= timeSinceLastToss)
                pitchPosition = 0;
        }
        else
            timeSinceLastToss = 0;
    }
}

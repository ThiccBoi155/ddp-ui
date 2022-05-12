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

    public new void Update()
    {
        SetDiscScale(holdScale);
        base.Update();
    }

    public override void Enter(MTouch mt)
    {
        base.Enter(mt);

        HoldDisc();
    }

    public override void Stay()
    {
        Destroy(currentDisc.gameObject);

        CloseNow();

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

    public void PlayCollisionSound()
    {
        // Implement: Change pitch and volume

        if (audioSource != null && collisionSound != null)
            audioSource.PlayOneShot(collisionSound, audioSource.volume);
    }
}

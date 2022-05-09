using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscTrashCan : MonoBehaviour
{
    public Collider2D Area { get { return dragTrigger; } }

    public bool Open { get; private set; } = false;

    [Header("References")]
    public Collider2D dragTrigger;
    public Collider2D throwTrigger;
    public Collider2D col;
    public SpriteRenderer sr;
    public AudioSource audioSource;

    [Header("Sprites")]
    public Sprite closedTrash;
    public Sprite openTrash;

    [Header("...")]
    public AudioClip collisionSound;

    private void Awake()
    {
        MTouchController.discTrashCans.Add(this);
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

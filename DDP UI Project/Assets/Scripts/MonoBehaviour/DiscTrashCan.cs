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

    [Header("Sprites")]
    public Sprite closedTrash;
    public Sprite openTrash;

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
    }

    private void OpenNow()
    {
        Open = true;
        sr.sprite = openTrash;

        if (col != null)
            col.enabled = false;
    }

    private void CloseNow()
    {
        Open = false;
        sr.sprite = closedTrash;

        if (col != null)
            col.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Disc>() != null)
            Destroy(collision.gameObject);
    }
}

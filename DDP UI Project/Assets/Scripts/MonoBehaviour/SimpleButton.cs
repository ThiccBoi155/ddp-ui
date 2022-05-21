using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleButton : MTouchable
{
    [Header("Sound")]
    public AudioSource audioSource;

    public override Collider2D MTouchCollider { get { return trigger; } }

    private Camera cam;
    private Collider2D trigger;

    protected new void Awake()
    {
        base.Awake();

        cam = Camera.main;

        trigger = GetComponent<Collider2D>();

        if (MTouchCollider == null || !MTouchCollider.isTrigger)
            Debug.Log($"{name}: No trigger for simple button found");
    }

    float timeAtMTouchClick;

    [Header("Settings")]
    // This value is measured in (world) units rather than screen pixel units or percentage
    public float maxClickDistance = .1f;
    // Theese values are measured in seconds
    public float maxClickDelay = .5f;

    public override void OnMTouchDown(MTouch mt)
    {
        Vector2 wPos = Funcs.MouseToWorldPoint(mt.pos, cam);

        timeAtMTouchClick = Time.time;
    }

    public override void OnMTouchDrag(MTouch mt)
    {
        Vector2 wPos = Funcs.MouseToWorldPoint(mt.pos, cam);
    }

    public override void OnMTouchUp(MTouch mt)
    {
        Vector2 wPos = Funcs.MouseToWorldPoint(mt.pos, cam);
        Vector2 startWPos = Funcs.MouseToWorldPoint(mt.startPos, cam);

        if ((wPos - startWPos).magnitude <= maxClickDistance && Time.time - timeAtMTouchClick <= maxClickDelay)
        {
            ClickAction();
            PlayClickSound();
        }
    }

    protected virtual void ClickAction()
    {
        Debug.Log("simple stuff");
    }

    protected void PlayClickSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}

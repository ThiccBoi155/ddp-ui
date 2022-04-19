using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverFlowMTouch : MTouchable
{
    [Header("References")]
    public Camera cam;
    public Collider2D dragArea;
    public CoverFlow cf;

    [Header("Settings")]
    public float multiplier = 1f;

    protected new void Awake()
    {
        base.Awake();

        cam = Camera.main;

        if (cf == null)
            cf = GetComponent<CoverFlow>();
    }

    public override Collider2D MTouchCollider { get { return dragArea; } }

    private bool holding = false;
    private float startCFPosition;

    private void Update()
    {
        SetCFPosition();
    }

    void SetCFPosition()
    {

    }

    bool dragging = false;

    float timeAtMTouchClick;
    // This value is measured in (world) units rather than screen pixel units or percentage
    [Header("MTouch settings")]
    public float maxClickDistance = .1f;
    // Theese values are measured in seconds
    public float maxClickDelay = .5f;
    public float delayBeforeDrag = .1f;

    public override void OnMTouchDown(MTouch mt)
    {
        Vector2 wPos = Funcs.MouseToWorldPoint(mt.pos, cam);

        timeAtMTouchClick = Time.time;

        startCFPosition = cf.cFPosition;

        holding = true;

        cf.countFixTimeNow = false;
    }

    public override void OnMTouchDrag(MTouch mt)
    {
        Vector2 wPos = Funcs.MouseToWorldPoint(mt.pos, cam);
        Vector2 startWPos = Funcs.MouseToWorldPoint(mt.startPos, cam);

        Vector2 delta = mt.pos - mt.startPos;
        Vector2 wDelta = wPos - startWPos;

        if (Time.time - timeAtMTouchClick > delayBeforeDrag || maxClickDistance <= wDelta.magnitude)
            dragging = true;

        if (dragging)
            cf.CFPosition = startCFPosition - wDelta.x * multiplier;
    }

    public override void OnMTouchUp(MTouch mt)
    {
        Vector2 wPos = Funcs.MouseToWorldPoint(mt.pos, cam);
        Vector2 startWPos = Funcs.MouseToWorldPoint(mt.startPos, cam);

        if ((wPos - startWPos).magnitude <= maxClickDistance && Time.time - timeAtMTouchClick <= maxClickDelay)
            ClickAction();

        holding = false;
        cf.countFixTimeNow = true;
        dragging = false;
    }

    void ClickAction()
    {
        if (Funcs.IsInteger(cf.cFPosition))
            cf.ejectDiscNow = true;
    }
}

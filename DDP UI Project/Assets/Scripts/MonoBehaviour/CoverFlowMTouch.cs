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

    public override void OnMTouchDown(MTouch mt)
    {
        Vector2 wPos = Funcs.MouseToWorldPoint(mt.pos, cam);

        startCFPosition = cf.cFPosition;

        holding = true;

        cf.countFixTimeNow = false;
    }

    public override void OnMTouchDrag(MTouch mt)
    {
        //Vector2 wPos = Funcs.MouseToWorldPoint(mt.pos, cam);
        //Vector2 startWPos = Funcs.MouseToWorldPoint(mt.startPos, cam);

        Vector2 delta = mt.pos - mt.startPos;
        //Vector2 wDelta = Funcs.MouseToWorldPoint(delta, cam);

        cf.CFPosition = startCFPosition - delta.x * multiplier;
    }

    public override void OnMTouchUp(MTouch mt)
    {
        holding = false;
        cf.countFixTimeNow = true;
    }
}

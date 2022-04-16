using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndClick : MTouchable
{
    Camera cam;
    [HideInInspector]
    public Rigidbody2D rid;
    [HideInInspector]
    public Collider2D col;

    public override Collider2D MTouchCollider { get { return col; } }

    public string logMessage = "Default message";

    [Header("Physics settings")]
    public float smallForceIndex = 0f;
    public float maxDragVelocity = float.MaxValue;
    public float velocityMultiplier = 1f;
    public bool accelerate = false;
    public float maxAcceleration = float.MaxValue;

    private new void Awake()
    {
        Setup();
    }

    bool setupRan = false;

    public void Setup()
    {
        if (!setupRan)
        {
            cam = Camera.main;
            target = transform.position;

            rid = GetComponent<Rigidbody2D>();
            if (rid == null)
                Debug.Log($"\"{name}\" did not contain a ridgidbody");

            col = GetComponent<Collider2D>();
            if (rid == null)
                Debug.Log($"\"{name}\" did not contain a collider");

            AddThisToMTouchController();

            setupRan = true;
        }

    }

    private void FixedUpdate()
    {
        SmallConstantForce();

        if (followTarget && rid != null)
        {
            if (!accelerate)
                FollowTargetSetVelocity();
            else
                FollowTargetAccelerate();
        }
    }

    void FollowTargetSetVelocity()
    {
        // This is the distance to the target. Also represented in units / second
        Vector2 toTarget = target - (Vector2)transform.position;

        Vector2 newVelocity = toTarget / Time.fixedDeltaTime * velocityMultiplier;

        Funcs.cabVector2Magnitude(ref newVelocity, maxDragVelocity);

        Vector2 addForceAmmount = (newVelocity - rid.velocity) * rid.mass;

        rid.AddForce(addForceAmmount, ForceMode2D.Impulse);
    }
    
    void FollowTargetAccelerate()
    {
        Vector2 toTarget = target - (Vector2)transform.position;

        Vector2 targetVelocity = toTarget / Time.fixedDeltaTime * velocityMultiplier;

        //Funcs.cabVector2Magnitude(ref targetVelocity, maxDragVelocity);

        Vector2 velocityDelta = targetVelocity - rid.velocity;

        Funcs.cabVector2Magnitude(ref velocityDelta, maxAcceleration);

        Vector2 addForceAmmount = (velocityDelta) * rid.mass;

        rid.AddForce(addForceAmmount, ForceMode2D.Impulse);
    }

    void SmallConstantForce()
    {
        Vector2 smallForce = -transform.position * smallForceIndex;

        rid.AddForce(smallForce, ForceMode2D.Force);
    }

    ////////////
    // MTouch //
    ////////////

    // Vector from transform.position to mouse position
    Vector2 offset = Vector2.zero;

    Vector2 target;
    bool followTarget = false;
    
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

        offset = wPos - (Vector2)transform.position;

        timeAtMTouchClick = Time.time;
    }

    public override void OnMTouchDrag(MTouch mt)
    {
        Vector2 wPos = Funcs.MouseToWorldPoint(mt.pos, cam);

        target = wPos - offset;

        if (Time.time - timeAtMTouchClick > delayBeforeDrag)
            StartFollowTarget();
    }

    public override void OnMTouchUp(MTouch mt)
    {
        Vector2 wPos = Funcs.MouseToWorldPoint(mt.pos, cam);
        Vector2 startWPos = Funcs.MouseToWorldPoint(mt.startPos, cam);

        if ((wPos - startWPos).magnitude <= maxClickDistance && Time.time - timeAtMTouchClick <= maxClickDelay)
            ClickAction();

        followTarget = false;
    }

    private void StartFollowTarget()
    {
        rid.angularVelocity = 0;
        followTarget = true;
    }

    //////////////////
    // Click action //
    //////////////////

    protected virtual void ClickAction()
    {
        Debug.Log(logMessage);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverFlowMTouch : MTouchable
{
    [Header("References")]
    public Camera cam;
    public Collider2D dragArea;
    public CoverFlow cf;
    public MTouchable dragOut;

    [Header("Drag settings")]
    public float dragMultiplier = 1f;
    public float moveDragMultiplier = 1f;

    [Header("Physics settings")]
    public float resistanceMultiplier = 1f;

    public float maxVelocity = .7f;
    public float minVelocity = .01f;

    public float bounceMultiplier = .5f;

    public int maxStoredCFPos = 5;

    [Header("Private fields (Don't edit this)")]
    [SerializeField]
    private float velocity = 0f;
    [SerializeField]
    private List<float> storedCFPositions = new List<float>();
    [SerializeField]
    private List<float> storedVelocities = new List<float>();

    protected new void Awake()
    {
        base.Awake();

        cam = Camera.main;

        if (cf == null)
            cf = GetComponent<CoverFlow>();
    }

    public override Collider2D MTouchCollider { get { return dragArea; } }

    //private bool holding = false;
    private float startCFPosition;

    private void Update()
    {
        CalculatePhysics();
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

        //holding = true;

        cf.countFixTimeNow = false;

        storedCFPositions.Clear();
        storedVelocities.Clear();

        storedCFPositions.Add(startCFPosition);
    }

    public override void OnMTouchDrag(MTouch mt)
    {
        Vector2 wPos = Funcs.MouseToWorldPoint(mt.pos, cam);
        Vector2 startWPos = Funcs.MouseToWorldPoint(mt.startPos, cam);

        Vector2 delta = mt.pos - mt.startPos;
        Vector2 wDelta = wPos - startWPos;

        if (Time.time - timeAtMTouchClick > delayBeforeDrag || maxClickDistance <= wDelta.magnitude)
            dragging = true;

        SetCFPosition(wDelta);

        //Vector3 oldDOPos = dragOut.transform.position;
        //dragOut.transform.position = new Vector3(oldDOPos.x, wPos.y, oldDOPos.z);
    }

    public override void OnMTouchUp(MTouch mt)
    {
        Vector2 wPos = Funcs.MouseToWorldPoint(mt.pos, cam);
        Vector2 startWPos = Funcs.MouseToWorldPoint(mt.startPos, cam);

        Vector2 wDelta = wPos - startWPos;

        SetCFPosition(wDelta);

        if ((wPos - startWPos).magnitude <= maxClickDistance && Time.time - timeAtMTouchClick <= maxClickDelay)
            ClickAction();

        //holding = false;
        cf.countFixTimeNow = true;
        dragging = false;

        CalculateCFThrowVelocity();

        if (minVelocity <= Mathf.Abs(velocity))
        {
            cf.readyForRoundNowSound = true;
            cf.readyForSnapSound = true;
        }

        
    }

    void ClickAction()
    {
        //if (Funcs.IsInteger(cf.cFPosition))
            cf.ejectDiscNow = true;
    }

    // Better name
    void SetCFPosition(Vector2 wDelta)
    {
        float currentMultiplier;

        if (cf.CFMoveGap)
            currentMultiplier = moveDragMultiplier;
        else
            currentMultiplier = dragMultiplier;

        if (dragging)
            cf.CFPosition = startCFPosition - wDelta.x * currentMultiplier;

        storedCFPositions.Add(cf.CFPosition);

        int count = storedCFPositions.Count;

        if (2 <= count)
        {
            float deltaPos = storedCFPositions[count - 1] - storedCFPositions[count - 2];

            // This works under the assumption that this function is run through Update()
            float newVelocity = deltaPos / Time.deltaTime;

            storedVelocities.Add(newVelocity);
        }

        if (maxStoredCFPos < count)
            storedCFPositions.RemoveAt(0);

        if (maxStoredCFPos - 1 < storedVelocities.Count)
        {
            storedVelocities.RemoveAt(0);
        }
    }

    void CalculateCFThrowVelocity()
    {
        if (1 < storedVelocities.Count)
        {
            float largestVelocity = 0;

            foreach (float vel in storedVelocities)
            {
                if (Mathf.Abs(largestVelocity) < Mathf.Abs(vel))
                {
                    largestVelocity = vel;
                }
            }

            velocity = largestVelocity;
        }
    }

    void CalculatePhysics()
    {
        if (Mathf.Abs(velocity) < minVelocity)
            velocity = 0;

        if (!dragging && velocity != 0)
        {
            velocity += -velocity * resistanceMultiplier * Time.deltaTime;

            Funcs.cabFloatAbs(ref velocity, maxVelocity);

            // This works under the assumption that this function is run through Update()
            cf.CFPosition += velocity * Time.deltaTime;

            if (cf.MaxCFPos == cf.CFPosition || cf.CFPosition == cf.MinCFPos)
                // Add bounce? It didn't work so well last time
                velocity = 0;
        }
    }

    /*/
    void CheckIfTouchLeft(Vector2 wPos)
    {
        if (!MTouchCollider.bounds.Contains(wPos))
        {
            
        }
    }
    //*/
}

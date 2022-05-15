using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CoverFlow : MonoBehaviour
{
    private const int space = 30;

    private int currentSetLayer = 0;

    ////////////////////
    //// Pulic fields
    ////////////////////

    [Header("References")]
    public Camera cam;
    public BorderCollider bc;
    public Transform coverHolder;

    [Header("Prefabs")]
    public GameObject discObj;
    
    [Space(space)]

    [Header("Temporary input")]
    public bool toggleMoveCurrentCover = false;

    [Space(space)]

    [Header("Eject speed settings")]
    public float ejectSpeed = 5f;
    public float ejectMass = 1f;
    [Header("Eject angle settings")]
    public float ejectAngle = 0f;
    public bool randomEjectAngle = true;
    public float randomEjectAngleRange = 25f;
    [Header("Eject torque settings")]
    public float ejectTorque = 0f;
    public bool randomEjectTorque = true;
    public float randomEjectTorqueRange = .5f;

    [Space(space)]

    [Header("Position, rotation and scale settings")]
    public int startPosition = 0;
    public float angle = 88; // 59.92
    public float selectGap = 2.6f; // 3.43
    public float stackGap = 0.6f;

    [Header("Max/min CF position")]
    [Range(0f, .499999f)]
    public float maxMinPositoin = .5f;

    [Header("Fix position settings (Disable: fixPositionDelay = -1)")]
    public float fixPositionDelay = .5f;
    public float fixPositionLerpVal = .1f;
    public float roundWithinRangeVal = .01f;
    public bool countFixTimeNow = true;

    [Space(space)]

    // Come up with better names
    [Header("Move cover settings")]
    public Vector3 moveCoverDeltaPos = new Vector3(0f, 0f, .4f);
    public float learpMoveCoverStuff = .5f;

    ////////////////////
    //// Old temporary input
    ////////////////////

    [System.NonSerialized]
    public bool jumpRight = false;
    [System.NonSerialized]
    public bool jumpLeft = false;
    [System.NonSerialized]
    public bool roundPosition = false;
    [System.NonSerialized]
    public bool ejectDiscNow = false;

    ////////////////////
    //// CFPosition
    ////////////////////

    [System.NonSerialized]
    public float cFPosition;
    public float CFPosition
    {
        get { return cFPosition; }
        set
        {
            float minPos = -maxMinPositoin;
            float maxPos = maxMinPositoin + coverCount - 1f;

            if (CFMoveGap)
                maxPos += 1f;

            if (value <= minPos)
                cFPosition = minPos;

            else if (maxPos <= value)
                cFPosition = maxPos;

            else
                cFPosition = value;
        }
    }

    ////////////////////
    //// Private fields
    ////////////////////

    private int coverCount = 1;

    public float MinCFPos { get { return -maxMinPositoin; } }
    public float MaxCFPos {
        get
        {
            float maxPos = maxMinPositoin + coverCount - 1f;

            if (CFMoveGap)
                maxPos += 1f;

            return maxPos;
        }
    }

    ////////////////////
    //// Setup
    ////////////////////

    private void Awake()
    {
        cam = Camera.main;

        discList = new List<Disc>();

        UpdateCoverCount();

        SetRandomPosition();
    }

    void UpdateCoverCount()
    {
        coverCount = coverHolder.transform.childCount;
    }

    void SetRandomPosition()
    {
        CFPosition = Random.Range(3, coverCount - 3);
    }

    ////////////////////
    //// Update
    ////////////////////

    private void Update()
    {
        BooleanButtons();

        UpdatePositions();

        FixPositionAfterTime();
    }

    private void BooleanButtons()
    {
        if (jumpRight)
        {
            jumpRight = false;
            cFPosition++;
        }
        if (jumpLeft)
        {
            jumpLeft = false;
            cFPosition--;
        }
        if (roundPosition)
        {
            roundPosition = false;
            cFPosition = Mathf.Round(cFPosition);
        }
        if (CFMoveGap)
            ejectDiscNow = false;

        if (ejectDiscNow)
        {
            ejectDiscNow = false;
            EjectDisc();
        }

        if (toggleMoveCurrentCover)
        {
            toggleMoveCurrentCover = false;

            ToggleMoveCover();
        }
    }

    private void UpdatePositions()
    {
        float i = 0;
        foreach (Transform cover in coverHolder.transform)
        {
            float coverPos = i - cFPosition;

            if (CFMoveGap)
                Funcs.MakeCFMoveGap(ref coverPos);

            //*
            Quaternion qm1 = Quaternion.Euler(90, -90, 90 + angle);
            Quaternion q0 = Quaternion.Euler(90, -90, 90);
            Quaternion q1 = Quaternion.Euler(90, -90, 90 - angle);
            /*/

            Quaternion q0 = Quaternion.Euler(90, -90, 90);

            Quaternion qm1 = q0 + Quaternion.AngleAxis(angle, Vector3.up);
            
            Quaternion q1 = q0; //Quaternion.AngleAxis(-angle, Vector3.up);

            //*/

            cover.localRotation = Quaternion.Lerp(qm1, q1, Funcs.Rangem1to1Range0to1(coverPos));

            float newX;

            if (-1 <= coverPos && coverPos <= 1)
            {
                newX = Mathf.Lerp(-selectGap, selectGap, Funcs.Rangem1to1Range0to1(coverPos));
            }
            else
            {
                float prefix = 1;
                if (coverPos < 0)
                    prefix = -1;

                newX = ((coverPos * prefix - 1) * stackGap + selectGap) * prefix;
            }
            
            cover.localPosition = new Vector3(newX, cover.localPosition.y, cover.localPosition.z);

            i++;
        }
    }

    private float lastCFPos = 0f;
    private float notIntegerPosTime = 0f;

    private void FixPositionAfterTime()
    {
        if (fixPositionDelay != -1 )
        {
            if (!Funcs.IsInteger(cFPosition) && cFPosition == lastCFPos && countFixTimeNow)
            {
                notIntegerPosTime += Time.deltaTime;
            }
            else
            {
                lastCFPos = cFPosition;
                notIntegerPosTime = 0f;
            }

            if (fixPositionDelay <= notIntegerPosTime)
            {
                float target = Mathf.Round(cFPosition);

                cFPosition = Mathf.Lerp(cFPosition, target, fixPositionLerpVal);

                cFPosition = Funcs.RoundWithinRange(cFPosition, roundWithinRangeVal);

                lastCFPos = cFPosition;
            }
        }
    }

    public void ToggleMoveCover()
    {
        if (detachedChild == null)
            DetachFromParent();
        else
            SetBackToParent();
    }

    ////////////////////
    //// Disc
    ////////////////////

    public List<Disc> discList;

    public void EjectDisc()
    {
        // Error check

        if (discObj == null)
        {
            Debug.Log("Disc prefab was not found");
            return;
        }

        if (discObj.GetComponent<DragAndClick>() == null)
        {
            Debug.Log("Disc did not contain DragAndClick");
            return;
        }

        Cover cover = GetCurrentCover();

        if (cover == null)
            return;

        int songIndex = cover.GetNextSongIndex();

        if (songIndex == -1)
            return;

        // Instantiation and setup

        GameObject newDisc = Instantiate(discObj);

        Disc disc = newDisc.GetComponent<Disc>();

        discList.Add(disc);

        disc.cf = this;

        cover.songDiscs[songIndex] = disc;

        // Spawn position

        float newPosY = GetTopOfThePanel().y - newDisc.transform.lossyScale.y / 2 + ejectSpeed * Time.fixedDeltaTime;

        Vector2 newPos = new Vector3(transform.position.x, newPosY);

        newDisc.transform.position = newPos;

        // Rigid body related

        float currentEjectAngle;

        if (!randomEjectAngle)
            currentEjectAngle = ejectAngle;
        else
            currentEjectAngle = Mathf.Lerp(-randomEjectAngleRange, randomEjectAngleRange, Random.value);

        Quaternion q = Quaternion.AngleAxis(currentEjectAngle, Vector3.forward);

        disc.rid.AddForce(q * Vector3.up * ejectSpeed, ForceMode2D.Impulse);

        float currentEjectTorque;

        if (!randomEjectTorque)
            currentEjectTorque = ejectTorque;
        else
            currentEjectTorque = Mathf.Lerp(-randomEjectTorqueRange, randomEjectTorqueRange, Random.value);

        disc.rid.AddTorque(currentEjectTorque, ForceMode2D.Impulse);

        Physics2D.IgnoreCollision(bc.col, disc.col);

        disc.rid.mass = ejectMass;

        // Other disc values

        disc.SetCoverArt(cover.GetDiscSprite());

        disc.discNum = songIndex + 1;

        disc.CurrentOrder = currentSetLayer; // here disc num is also shown
        currentSetLayer++;

        //disc.showDiscInfo.SetDiscNum(discList.Count);
        //disc.showDiscInfo.SetDiscNum(songIndex + 1);
    }

    public void RemoveDiscFromList(Disc dac)
    {
        discList.Remove(dac);
    }

    ////////////////////
    //// Get cover values
    ////////////////////

    public Vector3 GetTopOfThePanel()
    {
        Transform panel1 = transform;

        foreach (Transform cover in coverHolder.transform)
        {
            panel1 = cover;
            break;
        }

        Vector3 upMiddle = transform.position + Vector3.up * panel1.lossyScale.x * 5;

        return Funcs.projectPointToXYPlane(cam, upMiddle);
    }
    
    int GetCurrentCoverIndex()
    {
        return Mathf.RoundToInt(CFPosition);
    }

    Cover GetCurrentCover()
    {
        int currentIndex = GetCurrentCoverIndex();

        if (0 <= currentIndex && currentIndex < coverHolder.transform.childCount)
        {
            // (Index 0 is the transform of this)
            Transform t = coverHolder.GetComponentsInChildren<Transform>()[currentIndex + 1];

            if (t == null)
            {
                Debug.Log("Cound not find child");
                return null;
            }

            Cover c = t.GetComponent<Cover>();

            if (c == null)
            {
                Debug.Log("Cound not find Cover component");
                return null;
            }

            return c;
        }
        else
        {
            Debug.Log($"Index out of range. (Index: {currentIndex})");

            return null;
        }
    }

    ////////////////////
    //// Move cover
    ////////////////////

    public bool CFMoveGap { get; private set; }
    private Transform detachedChild = null;

    void DetachFromParent()
    {
        if (detachedChild == null)
        {
            if (!Funcs.IsInteger(cFPosition))
            {
                cFPosition = Mathf.Round(cFPosition);
                UpdatePositions();
                Debug.Log("Position was instantly rounded");
            }

            detachedChild = GetCurrentCover().transform;
            detachedChild.parent = transform; // null?

            CFMoveGap = true;
            UpdateCoverCount();

            Cover cov = detachedChild.GetComponent<Cover>();
            if (cov != null)
            {
                cov.cl = CoverLearp.LearpAway;
                cov.cf = this;
            }
        }
        else
            Debug.Log("Child is already detached");
    }

    void SetBackToParent()
    {
        if (detachedChild != null)
        {
            Cover cov = detachedChild.GetComponent<Cover>();
            if (cov != null)
            {
                cov.cl = CoverLearp.LearpToCF;
                cov.cf = this;
            }

            detachedChild.parent = coverHolder.transform;
            detachedChild.SetSiblingIndex(GetCurrentCoverIndex());
            detachedChild = null;
            CFMoveGap = false;
            UpdateCoverCount();
        }
        else
            Debug.Log("No child is detached");
    }

    ////////////////////
    //// Gizmos
    ////////////////////

    private void OnDrawGizmos()
    {
        //BooleanButtons();

        UpdatePositions();

        DrawProjectedPanelPoints();

        CFPosition = startPosition;
    }

    void DrawProjectedPanelPoints()
    {
        Transform panel1 = transform;

        foreach (Transform cover in coverHolder.transform)
        {
            panel1 = cover;
            break;
        }

        // fix this shit

        Gizmos.color = Color.green;

        Gizmos.DrawSphere(panel1.position, .1f);

        Vector3 upRight = Vector3.up + Vector3.right;
        Vector3 upLeft = Vector3.up + Vector3.left;

        Vector3 upRightCorner = transform.position + upRight * panel1.lossyScale.x * 5;
        Vector3 upLeftCorner = transform.position + upLeft * panel1.lossyScale.x * 5;
        Vector3 upMiddle = transform.position + Vector3.up * panel1.lossyScale.x * 5;

        Gizmos.DrawSphere(upRightCorner, .1f);
        Gizmos.DrawSphere(upLeftCorner, .1f);
        Gizmos.DrawSphere(upMiddle, .1f);

        Gizmos.color = Color.blue;

        Gizmos.DrawSphere(Funcs.projectPointToXYPlane(cam, upRightCorner), .1f);
        Gizmos.DrawSphere(Funcs.projectPointToXYPlane(cam, upLeftCorner), .1f);
        Gizmos.DrawSphere(Funcs.projectPointToXYPlane(cam, upMiddle), .1f);
    }
}

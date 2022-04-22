using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverFlow : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    public BorderCollider bc;

    [Header("Prefabs")]
    public GameObject discObj;

    [Header("Temporary input")]
    public float cFPosition;
    public float CFPosition
    {
        get { return cFPosition; }
        set
        {
            float minPos = -maxMinPositoin;
            float maxPos = maxMinPositoin + coverCount - 1f;

            if (cFMoveGap)
                maxPos += 1f;

            if (value <= minPos)
                cFPosition = minPos;

            else if (maxPos <= value)
                cFPosition = maxPos;

            else
                cFPosition = value;
        }
    }

    public bool jumpRight = false;
    public bool jumpLeft = false;
    public bool roundPosition = false;

    public bool ejectDiscNow = false;

    public bool toggleMoveCurrentCover = false;

    [Header("Position, rotation and scale settings")]
    public float angle = 88; // 59.92
    public float selectGap = 2.6f; // 3.43
    public float stackGap = 0.6f;

    public float scale = 1f; // .67

    [Header("Eject speed settings")]
    public float ejectSpeed = 5f;
    [Header("Eject angle settings")]
    public float ejectAngle = 0f;
    public bool randomEjectAngle = true;
    public float randomEjectAngleRange = 25f;
    [Header("Eject torque settings")]
    public float ejectTorque = 0f;
    public bool randomEjectTorque = true;
    public float randomEjectTorqueRange = .5f;

    [Header("MaxMinPositoin (range: 0 - 0.499999)")]
    public float maxMinPositoin = .5f;

    [Header("Fix position settings (Disable: fixPositionDelay = -1)")]
    public float fixPositionDelay = .5f;
    public float fixPositionLerpVal = .1f;
    public float roundWithinRangeVal = .01f;
    public bool countFixTimeNow = true;

    [Header("Private fields (Don't edit this)")]
    [SerializeField]
    private List<Disc> discList;

    private int coverCount = 1;

    private bool cFMoveGap = false;

    private Transform detachedChild = null;

    private void Awake()
    {
        cam = Camera.main;

        discList = new List<Disc>();

        UpdateCoverCount();
    }

    void UpdateCoverCount()
    {
        coverCount = transform.childCount;
    }

    private void Update()
    {
        BooleanButtons();

        UpdatePositions();

        SetScale();

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
        if (cFMoveGap)
            ejectDiscNow = false;

        if (ejectDiscNow)
        {
            ejectDiscNow = false;
            EjectDisc();
        }

        if (toggleMoveCurrentCover)
        {
            toggleMoveCurrentCover = false;

            if (detachedChild == null)
                DetachFromParent();
            else
                SetBackToParent();
        }
    }

    private void UpdatePositions()
    {
        float i = 0;
        foreach (Transform cover in transform)
        {
            float coverPos = i - cFPosition;

            if (cFMoveGap)
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

            cover.localRotation = Quaternion.Lerp(qm1, q1, Rangem1to1Range0to1(coverPos));

            float newX;

            if (-1 <= coverPos && coverPos <= 1)
            {
                newX = Mathf.Lerp(-selectGap, selectGap, Rangem1to1Range0to1(coverPos));
            }
            else
            {
                float prefix = 1;
                if (coverPos < 0)
                    prefix = -1;

                newX = ((coverPos * prefix - 1) * stackGap + selectGap) * prefix;
            }
            
            //cover.localPosition = new Vector3(newX, cover.localPosition.y, cover.localPosition.z);
            cover.localPosition = new Vector3(newX, 0, 0);

            i++;
        }
    }

    private void SetScale()
    {
        transform.localScale = new Vector3(scale, scale, scale);
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

                cFPosition = RoundWithinRange(cFPosition, roundWithinRangeVal);

                lastCFPos = cFPosition;
            }
        }
    }

    private float RoundWithinRange(float f, float roundRange)
    {
        float roundedF = Mathf.Round(f);

        float fRange = Mathf.Abs(f - roundedF);

        if (fRange <= roundRange)
            return roundedF;
        else
            return f;
    }

    // Range(-1 to 1) to range(0 to 1)
    float Rangem1to1Range0to1(float f)
    {
        return (f + 1) / 2;
    }

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

        // Instantiation and setup

        GameObject newDisc = Instantiate(discObj);

        Disc disc = newDisc.GetComponent<Disc>();

        discList.Add(disc);

        disc.cf = this;

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

        // Other disc values

        /*
        int i = 0;
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            Debug.Log($"{t.name} - {i}");

            i++;
        }

        Debug.Log(GetCurrentPanel().name);
        */
        Cover c = GetCurrentCover();

        if (c == null)
            Debug.Log("hmm");

        disc.SetCoverArt(c.GetDiscSprite());
    }

    public void RemoveDiscFromList(Disc dac)
    {
        discList.Remove(dac);
    }

    private void OnDrawGizmos()
    {
        BooleanButtons();

        UpdatePositions();

        SetScale();

        DrawProjectedPanelPoints();
    }

    void DrawProjectedPanelPoints()
    {
        Transform panel1 = transform;

        foreach (Transform cover in transform)
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

    public Vector3 GetTopOfThePanel()
    {
        Transform panel1 = transform;

        foreach (Transform cover in transform)
        {
            panel1 = cover;
            break;
        }

        Vector3 upMiddle = transform.position + Vector3.up * panel1.lossyScale.x * 5;

        return Funcs.projectPointToXYPlane(cam, upMiddle);
    }

    /*/
    // I don't know if this works
    Transform GetCurrentPanel()
    {
        int currentIndex = Mathf.RoundToInt(cFPosition);

        if (0 <= currentIndex && currentIndex < transform.childCount)
            return GetComponentsInChildren<Transform>()[currentIndex];
        else
        {
            Debug.Log("Index out of range");

            return null;
        }
    }
    //*/

    int GetCurrentCoverIndex()
    {
        return Mathf.RoundToInt(CFPosition);
    }

    Cover GetCurrentCover()
    {
        int currentIndex = GetCurrentCoverIndex();

        if (0 <= currentIndex && currentIndex < transform.childCount)
        {
            // (Index 0 is the transform of this)
            Transform t = GetComponentsInChildren<Transform>()[currentIndex + 1];

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
            detachedChild.parent = null;

            detachedChild.position += new Vector3(0f, 0f, -.4f);

            cFMoveGap = true;
            UpdateCoverCount();
        }
        else
            Debug.Log("Child is already detached");
    }

    void SetBackToParent()
    {
        if (detachedChild != null)
        {
            detachedChild.parent = transform;
            detachedChild.SetSiblingIndex(GetCurrentCoverIndex());
            detachedChild = null;
            cFMoveGap = false;
            UpdateCoverCount();
        }
        else
            Debug.Log("No child is detached");
    }
}

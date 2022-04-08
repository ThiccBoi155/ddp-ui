using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverFlow : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    public BorderCollider bc;

    [Header("Prefabs")]
    public GameObject disc;

    [Header("Temporary input")]
    public float CFPosition;

    public bool jumpRight = false;
    public bool jumpLeft = false;
    public bool roundPosition = false;

    public bool ejectDiscNow = false;

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

    [Header("Fix position settings (Disable: fixPositionDelay = -1)")]
    public float fixPositionDelay = .5f;
    public float fixPositionLerpVal = .1f;
    public float roundWithinRangeVal = .01f;

    [Header("Other settings")]
    public bool lookAtCamera = true;

    private void Awake()
    {
        cam = Camera.main;
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
            CFPosition++;
        }
        if (jumpLeft)
        {
            jumpLeft = false;
            CFPosition--;
        }
        if (roundPosition)
        {
            roundPosition = false;
            CFPosition = Mathf.Round(CFPosition);
        }
        if (ejectDiscNow)
        {
            ejectDiscNow = false;
            EjectDisc();
        }
    }

    private void UpdatePositions()
    {
        float i = 0;
        foreach (Transform cover in transform)
        {
            float coverPos = i - CFPosition;

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
        if (fixPositionDelay != -1)
        {
            if (!IsInteger(CFPosition) && CFPosition == lastCFPos)
            {
                notIntegerPosTime += Time.deltaTime;
            }
            else
            {
                lastCFPos = CFPosition;
                notIntegerPosTime = 0f;
            }

            if (fixPositionDelay <= notIntegerPosTime)
            {
                float target = Mathf.Round(CFPosition);

                CFPosition = Mathf.Lerp(CFPosition, target, fixPositionLerpVal);

                CFPosition = RoundWithinRange(CFPosition, roundWithinRangeVal);

                lastCFPos = CFPosition;
            }
        }
    }

    private bool IsInteger(float f)
    {
        return f % 1f == 0;
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

    private void EjectDisc()
    {
        if (disc == null)
        {
            Debug.Log("Disc prefab was not found");
            return;
        }

        if (disc.GetComponent<DragAndClick>() == null)
        {
            Debug.Log("Disc did not contain DragAndClick");
            return;
        }

        GameObject newDisc = Instantiate(disc);

        //newDisc.transform.position = (Vector2)transform.position;
        //newDisc.transform.position = (Vector2)(GetTopOfThePanel() - newDisc.transform.lossyScale / 2);

        float newPosY = GetTopOfThePanel().y - newDisc.transform.lossyScale.y / 2 + ejectSpeed * Time.fixedDeltaTime;

        Vector2 newPos = new Vector3(transform.position.x, newPosY);

        newDisc.transform.position = newPos;

        DragAndClick discDAC = newDisc.GetComponent<DragAndClick>();

        float currentEjectAngle;

        if (!randomEjectAngle)
            currentEjectAngle = ejectAngle;
        else
            currentEjectAngle = Mathf.Lerp(-randomEjectAngleRange, randomEjectAngleRange, Random.value);

        Quaternion q = Quaternion.AngleAxis(currentEjectAngle, Vector3.forward);

        discDAC.rid.AddForce(q * Vector3.up * ejectSpeed, ForceMode2D.Impulse);

        float currentEjectTorque;

        if (!randomEjectTorque)
            currentEjectTorque = ejectTorque;
        else
            currentEjectTorque = Mathf.Lerp(-randomEjectTorqueRange, randomEjectTorqueRange, Random.value);

        discDAC.rid.AddTorque(currentEjectTorque, ForceMode2D.Impulse);

        Physics2D.IgnoreCollision(bc.col, discDAC.col);

        Debug.Log($"Vec vel: {discDAC.rid.velocity}, float vel: {discDAC.rid.velocity.magnitude}");
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

        Gizmos.DrawSphere(projectPointToXYPlane(cam, upRightCorner), .1f);
        Gizmos.DrawSphere(projectPointToXYPlane(cam, upLeftCorner), .1f);
        Gizmos.DrawSphere(projectPointToXYPlane(cam, upMiddle), .1f);
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

        return projectPointToXYPlane(cam, upMiddle);
    }

    // Creates ray from _cam to point and finds the intersection with the xy-plane (z = 0)
    Vector3 projectPointToXYPlane(Camera _cam, Vector3 point)
    {
        // If point is in front of the plane.
        Ray camToPointRay = new Ray(point, point - _cam.transform.position);

        // If point is behind the plane. (This one might be unnecesarry.)
        Ray pointToCamRay = new Ray(point, _cam.transform.position - point);
        
        Plane xyPlane = new Plane(Vector3.forward, Vector3.zero);

        float distance = 0;

        if (xyPlane.Raycast(camToPointRay, out distance))

            return camToPointRay.GetPoint(distance);

        else if (xyPlane.Raycast(pointToCamRay, out distance))

            return pointToCamRay.GetPoint(distance);

        else

            return -Vector3.one;
    }
}

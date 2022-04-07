using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverFlow : MonoBehaviour
{
    [Header("References")]
    public Camera cam;

    [Header("Temporary input")]
    public float CFPosition;

    public bool jumpRight = false;
    public bool jumpLeft = false;
    public bool roundPosition = false;

    [Header("Settings")]
    public float angle = 88;
    public float selectGap = 2.6f;
    public float stackGap = 0.6f;

    public bool lookAtCamera = true;

    public float scale = 1f;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        BooleanButtons();

        UpdatePositions();

        SetScale();

        LookAtCamera();
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

    private void LookAtCamera()
    {
        //transform.LookAt(cam.transform.position);
        //transform.localRotation = Quaternion.AngleAxis(180f, Vector3.up);
    }

    private void SetScale()
    {
        transform.localScale = new Vector3(scale, scale, scale);
    }

    // Range(-1 to 1) to range(0 to 1)
    float Rangem1to1Range0to1(float f)
    {
        return (f + 1) / 2;
    }

    private void OnDrawGizmos()
    {
        BooleanButtons();

        UpdatePositions();

        SetScale();
    }
}

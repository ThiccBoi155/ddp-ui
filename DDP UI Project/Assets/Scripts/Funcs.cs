using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Funcs
{
    // This function works under the assumption that the camera has no rotation, and all interactable objects are at z = 0
    public static Vector3 MouseToWorldPoint(Vector3 mousePos, Camera _cam)
    {
        mousePos.z = _cam.transform.position.z * -1f;

        return _cam.ScreenToWorldPoint(mousePos);
    }

    // Creates ray from _cam to point and finds the intersection with the xy-plane (z = 0)
    public static Vector3 projectPointToXYPlane(Camera _cam, Vector3 point)
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

    // Range(-1 to 1) to range(0 to 1)
    public static float Rangem1to1Range0to1(float f)
    {
        return (f + 1) / 2;
    }

    public static float RoundWithinRange(float f, float roundRange)
    {
        float roundedF = Mathf.Round(f);

        float fRange = Mathf.Abs(f - roundedF);

        if (fRange <= roundRange)
            return roundedF;
        else
            return f;
    }

    public static bool IsInteger(float f)
    {
        return f % 1f == 0;
    }

    // Remember, magnitude cannot be less than 0
    public static void capVector2Magnitude(ref Vector2 v, float maxMag)
    {
        if (maxMag < v.magnitude)
        {
            v.Normalize();

            v *= maxMag;
        }
    }

    public static void cabFloat(ref float f, float max)
    {
        if (max < f)
            f = max;
    }

    public static void cabFloatAbs(ref float f, float max)
    {
        if (max < 0)
            Debug.Log("Max value should not be less than 0");

        if (max < Mathf.Abs(f))
        {
            if (f < 0)
                f = -max;
            else
                f = max;
        }
    }

    // (-2 -> -2) (-1 -> -1) (-0.5 -> 0) (0 -> 1) (1 -> 2)
    public static void MakeCFMoveGap(ref float val)
    {
        if (-1f < val)
        {
            if (val <= 0f)
                val = Mathf.Lerp(-1f, 1f, val + 1f);
            else
                val = val + 1f;
        }
    }

    public static void minMaxValueCorrectionMove(ref float min, ref float max, ref float previousMin)
    {
        if (max < min)
        {
            if (min == previousMin)
                min = max;
            else
                max = min;
        }

        previousMin = min;
    }

    public static void minMaxValueCorrectionCut(ref float min, ref float max, ref float previousMin)
    {
        if (max < min)
        {
            if (min == previousMin)
                max = min;
            else
                min = max;
        }

        previousMin = min;
    }
}

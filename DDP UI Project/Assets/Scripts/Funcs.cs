using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Funcs
{
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

    public static bool IsInteger(float f)
    {
        return f % 1f == 0;
    }
}

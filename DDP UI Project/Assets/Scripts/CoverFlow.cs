using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverFlow : MonoBehaviour
{
    public float position;

    public float angle = 56;
    public float selectGap = 2.7f;
    public float stackGap = 1.1f;

    public bool jumpRight = false;
    public bool jumpLeft = false;
    public bool roundPosition = false;

    private void Update()
    {
        BooleanButtons();

        UpdatePositions();
    }

    private void BooleanButtons()
    {
        if (jumpRight)
        {
            jumpRight = false;
            position++;
        }
        if (jumpLeft)
        {
            jumpLeft = false;
            position--;
        }
        if (roundPosition)
        {
            roundPosition = false;
            position = Mathf.Round(position);
        }
    }

    private void UpdatePositions()
    {
        float i = 0;
        foreach (Transform cover in transform)
        {
            float coverPos = i - position;

            Quaternion qm1 = Quaternion.Euler(90, -90, 90 + angle);
            Quaternion q0 = Quaternion.Euler(90, -90, 90);
            Quaternion q1 = Quaternion.Euler(90, -90, 90 - angle);
            
            cover.rotation = Quaternion.Lerp(qm1, q1, Rangem1to1Range0to1(coverPos));

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
                 

            cover.position = new Vector3(newX, cover.position.y, cover.position.z);

            i++;
        }
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
    }
}

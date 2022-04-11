using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTouch : MonoBehaviour
{
    Camera cam;

    int mouseIndex = -1;

    // This might not be needed outside of gizmos
    public List<Vector2> startMTouches = new List<Vector2>();

    public List<Vector2> mTouches = new List<Vector2>();

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        //DetectMouseInput();

        DetectTouchInput();
    }

    private void DetectMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startMTouches.Add(Input.mousePosition);
            mTouches.Add(new Vector2());

            mouseIndex = mTouches.Count - 1;
        }

        if (Input.GetMouseButton(0))
        {
            mTouches[mouseIndex] = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            startMTouches.RemoveAt(mouseIndex);
            mTouches.RemoveAt(mouseIndex);

            mouseIndex = -1;
        }
    }

    // Doesn't work next to DetectMouseInput()
    private void DetectTouchInput()
    {
        int i = 0;
        foreach (Touch t in Input.touches)
        {
            Vector2 temp = t.position;

            Debug.Log($"{i}: {t.phase}");

            //if (t.phase == TouchPhase.Began) ;

            //*/
            switch (t.phase)
            {
                case TouchPhase.Began:
                    startMTouches.Add(t.position);
                    mTouches.Add(t.position);
                    break;
                case TouchPhase.Stationary:
                case TouchPhase.Moved:
                    mTouches[i] = t.position;
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    startMTouches.RemoveAt(i);
                    mTouches.RemoveAt(i);
                    break;
            }
            //*/

            i++;
        }
    }

    void deletelater()
    {
        // touch down, drag, up?

        /*/
        for (int i = 0; i < Input.touchCount; i++)
        {
            //Vector2 temp = Input.touches[i].position;
            Vector2 temp = Input.GetTouch(i).position;
        }
        //*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        int i = 0;
        foreach (Vector2 mt in mTouches)
        {
            Vector2 startMTWorld = Funcs.MouseToWorldPoint(mt, cam);
            Vector2 mtWorld = Funcs.MouseToWorldPoint(startMTouches[i], cam);

            Gizmos.DrawSphere(mtWorld, .15f);

            Gizmos.DrawLine(startMTWorld, mtWorld);

            i++;
        }
    }
}

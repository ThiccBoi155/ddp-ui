using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Needs to be renaming for MTouch and MMTouch
public class MTouch : MonoBehaviour
{
    Camera cam;

    public List<MMTouch> mMTouches = new List<MMTouch>();

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
            mMTouches.Add(new MMTouch(Input.mousePosition, -1));
        }

        if (Input.GetMouseButton(0))
        {
            mMTouches.Find(mm => mm.fingerId == -1).pos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            MMTouch m = mMTouches.Find(mm => mm.fingerId == -1);
            mMTouches.Remove(m);
        }
    }

    // Doesn't work next to DetectMouseInput()
    private void DetectTouchInput()
    {
        int i = 0;
        foreach (Touch touch in Input.touches)
        {
            Debug.Log($"{i}: {touch.phase}");
            
            // Do better naming
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    mMTouches.Add(new MMTouch(touch.position, touch.fingerId));
                    break;
                case TouchPhase.Stationary:
                case TouchPhase.Moved:
                    mMTouches.Find(mm => mm.fingerId == touch.fingerId).pos = touch.position;
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    MMTouch m = mMTouches.Find(mm => mm.fingerId == touch.fingerId);
                    mMTouches.Remove(m);
                    break;
            }

            i++;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        int i = 0;
        foreach (MMTouch mm in mMTouches)
        {
            Vector2 startMTWorld = Funcs.MouseToWorldPoint(mm.startPos, cam);
            Vector2 mtWorld = Funcs.MouseToWorldPoint(mm.pos, cam);

            Gizmos.DrawSphere(mtWorld, .15f);

            Gizmos.DrawLine(startMTWorld, mtWorld);

            i++;
        }
    }
}

public class MMTouch
{
    public Vector2 startPos;
    public Vector2 pos;
    public int fingerId;

    public MMTouch(Vector2 _startPos, Vector2 _pos, int _fingerId)
    {
        startPos = _startPos;
        pos = _pos;
        fingerId = _fingerId;
    }

    public MMTouch(Vector2 _samePos, int _fingerId)
    {
        startPos = pos = _samePos;
        fingerId = _fingerId;
    }
}

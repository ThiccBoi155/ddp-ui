using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Needs to be renaming for MTouch and MMTouch
public class MTouchController : MonoBehaviour
{
    Camera cam;

    public List<MTouch> mMTouches = new List<MTouch>();
    public Dictionary<int, MTouch> mTouches = new Dictionary<int, MTouch>();

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
            mTouches.Add(-1, new MTouch(Input.mousePosition));
        }

        if (Input.GetMouseButton(0))
        {
            mTouches[-1].pos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            mTouches.Remove(-1);
        }
    }

    // Doesn't work next to DetectMouseInput()
    private void DetectTouchInput()
    {
        int i = 0;
        foreach (Touch touch in Input.touches)
        {
            //Debug.Log($"{i}: {touch.phase}");
            
            // Do better naming
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    mTouches.Add(touch.fingerId, new MTouch(Input.mousePosition));
                    break;

                case TouchPhase.Stationary:
                case TouchPhase.Moved:
                    mTouches[touch.fingerId].pos = Input.mousePosition;
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    mTouches.Remove(touch.fingerId);
                    break;
            }

            i++;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        int i = 0;
        foreach (KeyValuePair<int, MTouch> kvp in mTouches)
        {
            Vector2 startMTWorld = Funcs.MouseToWorldPoint(kvp.Value.startPos, cam);
            Vector2 mtWorld = Funcs.MouseToWorldPoint(kvp.Value.pos, cam);

            Gizmos.DrawSphere(mtWorld, .15f);

            Gizmos.DrawLine(startMTWorld, mtWorld);

            Debug.Log($"{kvp.Key}: {startMTWorld} --- {mtWorld}");

            i++;
        }
    }
}

public class MTouch
{
    public Vector2 startPos;
    public Vector2 pos;

    public MTouch(Vector2 _startPos, Vector2 _pos)
    {
        startPos = _startPos;
        pos = _pos;
    }

    public MTouch(Vector2 _samePos)
    {
        startPos = pos = _samePos;
    }

    /*/
    public MTouch(Vector2 _startPos, Vector2 _pos, int _fingerId)
    {
        startPos = _startPos;
        pos = _pos;
        fingerId = _fingerId;
    }

    public MTouch(Vector2 _samePos, int _fingerId)
    {
        startPos = pos = _samePos;
        fingerId = _fingerId;
    }
    //*/
}

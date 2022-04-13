using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTouchController : MonoBehaviour
{
    private static List<MTouchable> mTouchables = new List<MTouchable>();

    public static void AddToMTouchables(MTouchable mt)
    {
        if (mTouchables == null)
            mTouchables = new List<MTouchable>();

        mTouchables.Add(mt);
    }

    public static void RemoveFromMTouchables(MTouchable mt)
    {
        if (mTouchables != null)
            mTouchables.Remove(mt);
    }

    Camera cam;

    [Header("When multiTouch is true, the mouse doesn't work")]
    public bool multiTouch = true;
    public Dictionary<int, MTouch> mTouches = new Dictionary<int, MTouch>();

    private bool previousMultiTouch = true;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        UpdateMultiTouchBool();

        if (multiTouch)
            DetectTouchInput();
        else
            DetectMouseInput();
    }

    private void DetectMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MTouchDown(Input.mousePosition, -1);
        }

        if (Input.GetMouseButton(0))
        {
            MTouchDrag(Input.mousePosition, -1);
        }

        if (Input.GetMouseButtonUp(0))
        {
            MTouchUp(-1);
        }
    }

    // Doesn't work next to DetectMouseInput()
    private void DetectTouchInput()
    {
        int i = 0;
        foreach (Touch touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    MTouchDown(touch.position, touch.fingerId);
                    break;

                case TouchPhase.Stationary:
                case TouchPhase.Moved:
                    MTouchDrag(touch.position, touch.fingerId);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    MTouchUp(touch.fingerId);
                    break;
            }

            i++;
        }
    }

    private void UpdateMultiTouchBool()
    {
        if (multiTouch != previousMultiTouch)
        {
            mTouches.Clear();

            previousMultiTouch = multiTouch;
        }
    }



    private void MTouchDown(Vector2 pos, int fingerId)
    {
        mTouches.Add(fingerId, new MTouch(pos));

        foreach (MTouchable mt in mTouchables)
        {
            Vector2 v = Funcs.MouseToWorldPoint(mTouches[fingerId].pos, cam);

            if (mt.MTouchCollider.bounds.Contains(v) && !mt.Grapped)
            {
                mTouches[fingerId].currentMT = mt;
                mt.Grapped = true;
                mt.OnMTouchDown(v);
            }
        }
    }

    private void MTouchDrag(Vector2 pos, int fingerId)
    {
        if (mTouches.ContainsKey(fingerId))
        {
            mTouches[fingerId].pos = pos;

            if (mTouches[fingerId].currentMT != null)
            {
                Vector2 v = Funcs.MouseToWorldPoint(mTouches[fingerId].pos, cam);

                mTouches[fingerId].currentMT.OnMTouchDrag(v);
            }
        }
    }

    private void MTouchUp(int fingerId)
    {
        if (mTouches.ContainsKey(fingerId))
            if (mTouches[fingerId].currentMT != null)
            {
                Vector2 v = Funcs.MouseToWorldPoint(mTouches[fingerId].pos, cam);

                mTouches[fingerId].currentMT.Grapped = false;
                mTouches[fingerId].currentMT.OnMTouchUp(v);
            }

        mTouches.Remove(fingerId);
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

            i++;
        }
    }
}

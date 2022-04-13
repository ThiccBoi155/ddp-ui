using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTouchController : MonoBehaviour
{
    // There might need to be a hiearchy structure for different MTouchables, if their colliders ever overlap in a meaningful way
    private static List<MTouchable> mTouchables = new List<MTouchable>();

    public static void AddToMTouchables(MTouchable mtble)
    {
        if (mTouchables == null)
            mTouchables = new List<MTouchable>();

        mTouchables.Add(mtble);
    }

    public static void RemoveFromMTouchables(MTouchable mtble)
    {
        if (mTouchables != null)
            mTouchables.Remove(mtble);
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

        MTouch mt = mTouches[fingerId];

        foreach (MTouchable mtble in mTouchables)
        {
            Vector2 v = Funcs.MouseToWorldPoint(mt.pos, cam);

            if (mtble.MTouchCollider.bounds.Contains(v) && !mtble.grapped)
            {
                mt.currentMTble = mtble;
                mtble.grapped = true;
                mtble.OnMTouchDown(mt);
            }
        }
    }

    private void MTouchDrag(Vector2 pos, int fingerId)
    {
        if (mTouches.ContainsKey(fingerId))
        {
            MTouch mt = mTouches[fingerId];

            mt.pos = pos;

            if (mt.currentMTble != null)
            {
                mt.currentMTble.OnMTouchDrag(mt);
            }
        }
    }

    private void MTouchUp(int fingerId)
    {
        if (mTouches.ContainsKey(fingerId))
        {
            MTouch mt = mTouches[fingerId];

            if (mt.currentMTble != null)
            {
                mt.currentMTble.grapped = false;
                mt.currentMTble.OnMTouchUp(mt);
            }
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

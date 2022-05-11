using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTouchController : MonoBehaviour
{
    // There might need to be a hiearchy structure for different MTouchables, if their colliders ever overlap in a meaningful way
    private static List<MTouchable> mTouchables = new List<MTouchable>();

    private static List<MonoBehaviour> dragIntos = new List<MonoBehaviour>();
    public static List<DiscTrashCan> discTrashCans = new List<DiscTrashCan>();
    public static List<SnapArea> snapAreas = new List<SnapArea>();

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

                //IgnoreDiscDiscAreaCollision(mt);

                if (mt.currentMTble.GetType() == typeof(Disc))
                    mt.currentMTble.gameObject.layer = LayerMask.NameToLayer("Held Disc");
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
                if (mt.currentSnapArea == null)
                {
                    mt.currentMTble.OnMTouchDrag(mt);

                    CheckDragInto(mt);
                }
                else
                    CheckDraggingInto(mt);

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

                if (mt.currentSnapArea != null)
                {
                    mt.currentSnapArea.Stay();
                    mt.currentSnapArea = null;
                }
                else
                {
                    if (mt.currentMTble.GetType() == typeof(Disc))
                        mt.currentMTble.gameObject.layer = LayerMask.NameToLayer("Physics Disc");

                    //IgnoreDiscDiscAreaCollision(mt, false);
                }
            }
        }

        mTouches.Remove(fingerId);
    }

    private void CheckDragInto(MTouch mt)
    {
        if (mt.currentMTble.GetType() == typeof(Disc) && mt.currentSnapArea == null)
        {
            Vector2 wPos = Funcs.MouseToWorldPoint(mt.pos, cam);

            foreach (SnapArea snapArea in snapAreas)
            {
                if (snapArea.Area.bounds.Contains(wPos))
                {
                    if (snapArea.currentDisc == null)
                    {
                        mt.currentSnapArea = snapArea;

                        mt.currentSnapArea.Enter(mt);

                        break;
                    }
                    else if (snapArea.currentDisc == mt.currentMTble as Disc)
                    {
                        mt.currentSnapArea = snapArea;

                        mt.currentSnapArea.HoldAgain();

                        break;
                    }
                }
            }

            foreach (DiscTrashCan can in discTrashCans)
                if (can.Area.bounds.Contains(wPos))
                    Destroy(mt.currentMTble.gameObject);
        }
    }

    private void CheckDraggingInto(MTouch mt)
    {
        Vector2 wPos = Funcs.MouseToWorldPoint(mt.pos, cam);

        if (mt.currentSnapArea.Area.bounds.Contains(wPos))
        {
            mt.currentSnapArea.Holding();
        }
        else
        {
            mt.currentSnapArea.Leave();
            mt.currentSnapArea = null;
        }
    }

    void IgnoreDiscDiscAreaCollision(MTouch mt, bool ignore = true)
    {
        Disc disc = mt.currentMTble as Disc;

        if (disc != null)
        {
            foreach (SnapArea snapArea in snapAreas)
                Physics2D.IgnoreCollision(disc.col, snapArea.col, ignore);

            foreach (DiscTrashCan dtc in discTrashCans)
                Physics2D.IgnoreCollision(disc.col, dtc.col, ignore);
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

            i++;
        }
    }
}

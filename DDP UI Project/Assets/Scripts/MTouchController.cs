using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Needs to be renaming for MTouch and MMTouch
public class MTouchController : MonoBehaviour
{
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
            mTouches.Add(-1, new MTouch(Input.mousePosition));
        }

        if (Input.GetMouseButton(0))
        {
            if (mTouches.ContainsKey(-1))
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
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    mTouches.Add(touch.fingerId, new MTouch(touch.position));
                    break;

                case TouchPhase.Stationary:
                case TouchPhase.Moved:
                    if (mTouches.ContainsKey(touch.fingerId))
                        mTouches[touch.fingerId].pos = touch.position;
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    mTouches.Remove(touch.fingerId);
                    break;
            }

            i++;
        }
    }

    private void UpdateMultiTouchBool()
    {
        if (multiTouch != previousMultiTouch)
        {
            Debug.Log("Clear!");

            mTouches.Clear();

            previousMultiTouch = multiTouch;
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
}

// Currently not in use. Might be useful eventually.
public class MTouchDict : IEnumerable
{
    public Dictionary<int, MTouch> mTouches;

    public MTouchDict()
    {
        mTouches = new Dictionary<int, MTouch>();
    }

    public void Add(Vector2 _samePos, int fingerId)
    {
        mTouches.Add(fingerId, new MTouch(_samePos));
    }

    // Currently not in use
    public void Add(Vector2 _startPos, Vector2 _pos, int fingerId)
    {
        mTouches.Add(fingerId, new MTouch(_startPos, _pos));
    }

    public void SetPos(Vector2 _pos, int fingerId)
    {
        if (mTouches.ContainsKey(fingerId))
            mTouches[fingerId].pos = _pos;
    }

    public void Remove(int fingerId)
    {
        mTouches.Remove(fingerId);
    }

    public IEnumerator GetEnumerator()
    {
        return mTouches.GetEnumerator();
    }
}

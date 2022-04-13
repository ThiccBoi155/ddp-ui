using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTouch
{
    public Vector2 startPos;
    public Vector2 pos;
    public MTouchable currentMT;

    public MTouch(Vector2 _startPos, Vector2 _pos)
    {
        startPos = _startPos;
        pos = _pos;
        currentMT = null;
    }

    public MTouch(Vector2 _samePos)
    {
        startPos = pos = _samePos;
        currentMT = null;
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

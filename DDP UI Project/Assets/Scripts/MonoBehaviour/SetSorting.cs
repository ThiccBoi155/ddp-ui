using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSorting : MonoBehaviour
{
    public Renderer rend;

    public bool setNow = false;
    public int setSortingLayerIDAs = 0;

    private void Update()
    {
        SetIfTrue();
    }

    void SetIfTrue()
    {
        if (setNow)
        {
            setNow = false;
            rend.sortingLayerID = setSortingLayerIDAs;
        }
    }

    private void OnDrawGizmos()
    {
        SetIfTrue();
    }
}

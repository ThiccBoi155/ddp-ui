using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiscQueueParent : MonoBehaviour
{
    [Header("References")]
    public DiscPlayer discPlayer;

    [Header("Settings")]
    public float passDiscDelay;

    private void Awake()
    {
        SetChildrenReferences();
    }

    void SetChildrenReferences()
    {
        DiscQueue[] children = GetComponentsInChildren<DiscQueue>();

        SnapArea next = discPlayer;

        foreach (DiscQueue discQueue in children)
        {
            discQueue.nextQueue = next;
            next = discQueue;
        }
    }
}

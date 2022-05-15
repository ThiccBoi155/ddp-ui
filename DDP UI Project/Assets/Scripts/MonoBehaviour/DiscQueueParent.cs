using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiscQueueParent : MonoBehaviour
{
    [Header("References")]
    public DiscPlayer discPlayer;

    [Header("Settings")]
    public float passDiscDelay;
    public int startHideIndex = -1;

    private DiscQueue[] children;

    private void Awake()
    {
        SetChildrenReferences();

        HideFrom(startHideIndex);
    }

    private void Update()
    {
        Stuff();
    }

    void SetChildrenReferences()
    {
        children = GetComponentsInChildren<DiscQueue>();
    
        SnapArea next = discPlayer;

        foreach (DiscQueue discQueue in children)
        {
            discQueue.nextQueue = next;
            next = discQueue;
        }
    }

    void HideFrom(int index)
    {
        for (int i = 0; i < children.Length; i++)
        {
            bool currentActive = i < index || startHideIndex == -1;

            children[i].gameObject.SetActive(currentActive);
        }
    }

    void Stuff()
    {
        bool filled = true;

        int hideFromIndex = -1;

        int i = 0;
        foreach (DiscQueue discQueue in children)
        {
            //*/
            bool currentActive = i < startHideIndex || filled || discQueue.currentDisc != null || startHideIndex == -1;

            //children[i].gameObject.SetActive(currentActive);

            if (currentActive)
                hideFromIndex = i + 1;

            /*/

            if (i < startHideIndex || filled || discQueue.currentDisc != null)
                hideFromIndex = i;

            //*/

            if (discQueue.currentDisc == null)
                filled = false;

            i++;
        }

        HideFrom(hideFromIndex);
    }
}

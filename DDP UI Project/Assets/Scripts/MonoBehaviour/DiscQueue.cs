using UnityEngine;
using System.Collections;

public class DiscQueue : SnapArea
{
    [Header("References")]
    public SnapArea nextQueue;

    private DiscQueueParent dqp;
    private float countTime = 0f;

    protected new void Awake()
    {
        base.Awake();

        dqp = GetComponentInParent<DiscQueueParent>();
    }

    protected new void Update()
    {
        base.Update();

        PassDisc();
    }

    void PassDisc()
    {
        if (nextQueue != null)
            if (nextQueue.currentDisc == null && currentDisc != null)
            {
                countTime += Time.deltaTime;

                if (dqp.passDiscDelay <= countTime)
                {
                    nextQueue.currentDisc = currentDisc;
                    currentDisc = null;
                }
            }
            else
                countTime = 0f;
    }
}

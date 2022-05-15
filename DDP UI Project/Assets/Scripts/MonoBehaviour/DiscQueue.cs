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
            if (nextQueue.currentDisc == null && currentDisc != null && !holding)
            {
                countTime += Time.deltaTime;

                if (dqp.passDiscDelay <= countTime)
                {
                    PassDiscNow();
                }
            }
            else
                countTime = 0f;
    }

    void PassDiscNow()
    {
        //nextQueue.currentDisc = currentDisc;
        nextQueue.Enter(currentDisc);
        nextQueue.Stay();

        Leave();
    }
}

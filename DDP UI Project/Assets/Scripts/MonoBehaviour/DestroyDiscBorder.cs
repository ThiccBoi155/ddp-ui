using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDiscBorder : MonoBehaviour
{
    [Header("References")]
    public EdgeCollider2D trigger;
    public CoverFlow cf;

    [Header("Value settings")]
    public Vector2 buttomLeft;
    public Vector2 topRight;

    [Header("Set settings")]
    public bool attachToCam;
    public bool constantAttachToCam;
    public bool attachButtomToPanel;

    private void FixedUpdate()
    {
        SetEdgeCollider();
    }

    void SetEdgeCollider()
    {
        List<Vector2> points = new List<Vector2>()
        {
            buttomLeft,
            new Vector2(buttomLeft.x, topRight.y),
            topRight,
            new Vector2(topRight.x, buttomLeft.y),
            buttomLeft
        };

        trigger.SetPoints(points);
    }

    private void OnDrawGizmos()
    {
        SetEdgeCollider();
    }

    private void Update()
    {
        foreach (Disc disc in cf.discList)
            if (!trigger.bounds.Contains(disc.transform.position))
                Destroy(disc.gameObject);
    }

    /*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Disc disc = collision.GetComponent<Disc>();

        if (disc != null)
            Destroy(collision.gameObject);
    }
    //*/
}

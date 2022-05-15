using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note: Ridgid bodies will fly out of the border collider if they go too fast

public class BorderCollider : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    public EdgeCollider2D col;
    public EdgeCollider2D trigger;
    public CoverFlow cf;

    [Header("Value settings")]
    public Vector2 buttomLeft;
    public Vector2 topRight;

    [Header("Set settings")]
    public bool attachToCam;
    public bool constantAttachToCam;
    public bool attachButtomToPanel;

    private void Awake()
    {
        SetReferences();
    }

    private void FixedUpdate()
    {
        SetBorderColliderToCameraBorder();
        SetEdgeCollider();
        SetButtomToPanel();
    }

    void SetReferences()
    {
        cam = Camera.main;
        //col = GetComponent<EdgeCollider2D>();

        EdgeCollider2D[] edgeColliders = GetComponents<EdgeCollider2D>();

        if (edgeColliders.Length == 2)
        {
            col = edgeColliders[0];
            trigger = edgeColliders[1];

            col.isTrigger = false;
            trigger.isTrigger = true;
        }
        else if (edgeColliders.Length == 1)
        {
            col = edgeColliders[0];
            col.isTrigger = false;

            Debug.Log("Only one edge collider found");
        }
        else
        {
            Debug.Log("No edge collider found");
        }
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

        col.SetPoints(points);
        trigger.SetPoints(points);
        
    }

    void SetBorderColliderToCameraBorder()
    {
        if (constantAttachToCam)
            attachToCam = true;

        if (attachToCam)
        {
            transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 0);

            buttomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, -cam.transform.position.z));
            topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -cam.transform.position.z));

            if (!constantAttachToCam)
                attachToCam = false;
        }
    }

    void SetButtomToPanel()
    {
        if (attachButtomToPanel)
        {
            attachButtomToPanel = false;
            buttomLeft.y = cf.GetTopOfThePanel().y;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Physics2D.IgnoreCollision(col, collision, false);

        Disc disc = collision.GetComponent<Disc>();

        if (disc != null)
            disc.rid.mass = 1f;
    }

    private void OnDrawGizmos()
    {
        SetReferences();
        SetBorderColliderToCameraBorder();
        SetEdgeCollider();
        SetButtomToPanel();
    }
}

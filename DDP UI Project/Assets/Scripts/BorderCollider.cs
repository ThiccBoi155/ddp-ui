using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note: Ridgid bodies will fly out of the border collider if they go too fast

public class BorderCollider : MonoBehaviour
{
    public Camera cam;
    public EdgeCollider2D col;

    public bool attachToCam;
    public bool constantAttackToCam;

    public Vector2 buttomLeft;
    public Vector2 topRight;

    private void Awake()
    {
        SetReferences();
    }

    private void FixedUpdate()
    {
        SetBorderColliderToCameraBorder();
        SetEdgeCollider();
    }

    void SetReferences()
    {
        cam = Camera.main;
        col = GetComponent<EdgeCollider2D>();
    }

    void SetEdgeCollider()
    {
        col.SetPoints(new List<Vector2>() {
            buttomLeft,
            new Vector2(buttomLeft.x, topRight.y),
            topRight,
            new Vector2(topRight.x, buttomLeft.y),
            buttomLeft
        });
    }

    void SetBorderColliderToCameraBorder()
    {
        if (constantAttackToCam)
            attachToCam = true;

        if (attachToCam)
        {
            transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 0);

            buttomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, -cam.transform.position.z));
            topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -cam.transform.position.z));

            if (!constantAttackToCam)
                attachToCam = false;
        }
    }

    private void OnDrawGizmos()
    {
        SetReferences();
        SetBorderColliderToCameraBorder();
        SetEdgeCollider();
    }
}

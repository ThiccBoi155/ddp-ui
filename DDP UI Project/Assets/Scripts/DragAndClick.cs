using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndClick : MonoBehaviour
{
    Camera cam;
    Rigidbody2D rid;

    public string logMessage = "Default message";

    private void Awake()
    {
        cam = Camera.main;
        target = transform.position;

        rid = GetComponent<Rigidbody2D>();
        if (rid == null)
            Debug.Log($"\"{name}\" did not contain a ridgidbody");

        //Debug.Log($"Time stuff: {Time.fixedDeltaTime}");
    }

    private void FixedUpdate()
    {
        if (followTarget && rid != null)
        {
            //rid.AddForce(-rid.velocity, ForceMode2D.Impulse);
            

            Vector2 toTarget = target - (Vector2)transform.position - offset;

            Vector2 temp = toTarget / Time.fixedDeltaTime;

            //rid.AddForce(toTarget - rid.velocity, ForceMode2D.Impulse);
            //rid.AddForce(toTarget, ForceMode2D.Impulse);
            rid.AddForce(temp - rid.velocity, ForceMode2D.Impulse);
            //rid.AddForce(temp, ForceMode2D.Impulse);
        }
    }

    /////////////
    // OnMouse //
    /////////////

    private void OnMouseDown()
    {
        OnMTouchDown(cam.ScreenToWorldPoint(Input.mousePosition));
    }

    private void OnMouseDrag()
    {
        OnMTouchDrag(cam.ScreenToWorldPoint(Input.mousePosition));
    }

    private void OnMouseUp()
    {
        OnMTouchUp(cam.ScreenToWorldPoint(Input.mousePosition));
    }

    ///////////////////////////////////////
    // MTouch : Combined mouse and touch //
    ///////////////////////////////////////

    // Vector from transform.position to mouse position
    Vector2 offset = Vector2.zero;

    Vector2 target;
    bool followTarget = false;
    
    float timeAtMTouchClick;
    Vector2 startMTouchPos;
    // This value is measured in (world) units rather than screen pixel units or percentage
    public float maxClickDistance = .1f;
    public float maxClickDelay = .5f;

    private void OnMTouchDown(Vector2 pos)
    {
        offset = pos - (Vector2)transform.position;

        //StartFollowTarget();

        startMTouchPos = pos;
        timeAtMTouchClick = Time.time;
    }

    private void OnMTouchDrag(Vector2 pos)
    {
        target = pos - offset;

        if (Time.time - timeAtMTouchClick > maxClickDelay)
            StartFollowTarget();
    }

    private void OnMTouchUp(Vector2 pos)
    {

        if ((pos - startMTouchPos).magnitude <= maxClickDistance && Time.time - timeAtMTouchClick <= maxClickDelay)
            ClickAction();

        followTarget = false;
    }

    //////////////////
    // Click action //
    //////////////////
    
    private void StartFollowTarget()
    {
        rid.angularVelocity = 0;
        followTarget = true;
    }

    private void ClickAction()
    {
        Debug.Log(logMessage);
    }
}

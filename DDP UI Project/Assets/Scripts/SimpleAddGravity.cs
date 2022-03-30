using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAddGravity : MonoBehaviour
{
    Rigidbody2D rid;

    public Vector2 gravityVector = new Vector2(0f, -9.81f);

    private void Awake()
    {
        rid = GetComponent<Rigidbody2D>();
        if (rid == null)
            Debug.Log($"\"{name}\" did not contain a ridgidbody");
    }

    private void FixedUpdate()
    {
        if (rid != null)
            rid.AddForce(gravityVector);
    }
}

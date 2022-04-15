using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintUpdate : MonoBehaviour
{
    public bool printOnUpdate = false;
    public bool printOnFixedUpdate = false;

    private void Update()
    {
        if (printOnUpdate)
            Debug.Log("Update");
    }

    private void FixedUpdate()
    {
        if (printOnFixedUpdate)
            Debug.Log("FixedUpdate");
    }
}

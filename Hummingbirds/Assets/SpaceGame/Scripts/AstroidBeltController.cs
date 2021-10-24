using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidBeltController : MonoBehaviour
{
    public Vector3 desiredTorque = new Vector3(0,0.2f,0);

    private void FixedUpdate()
    {
        transform.localRotation = Quaternion.Euler(Vector3.Lerp(transform.localRotation.eulerAngles, transform.localRotation.eulerAngles + desiredTorque, .1f));
    }
}

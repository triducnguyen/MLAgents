using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform targetLook;   //target to look at
    public Transform targetPos;    //target position to move to

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //look at target
        transform.LookAt(targetLook);

        //if camera is not near target location
        if (Vector3.Distance(transform.position, targetPos.position) > 0.1f)
        {
            //lerp towards target position
            var newPos = Vector3.Lerp(transform.position, targetPos.position, .2f);
            transform.position = newPos;
        }
        
    }
}

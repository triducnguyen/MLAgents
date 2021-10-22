using System;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    //if the ship is being piloted by an ai or a player (default is false)
    public bool ai_pilot = false;

    //the teamID of this ship
    //team 0 is neutral
    public int teamID = 0;
    //whether or not the spacecraft is landed
    public bool landed = true;

    //whether or not the spacraft can be controlled
    public bool controllable = false;

    //reference to ship animator
    public Animator animator;

    //reference to ship rigidbody
    public Rigidbody shipBody;

    //list of forces added to ship
    public List<KeyControl> pressedKeys = new List<KeyControl>();

    //ray position
    public Transform rayOrigin;
    //distance sensor on bottom of ship. For landing
    public FloorSensor floorSensor;
    public bool canLand
    {
        get
        {
            return !landed && controllable && floorSensor.distance < 5;
        }
    }

    //list of bound keys
    List<KeyControl> boundKeys = new List<KeyControl>();

    private void Awake()
    {
        //create floor sensor
        floorSensor = new FloorSensor(new Ray(rayOrigin.position, Vector3.down), 5f);
        //set bound keys
        boundKeys = new List<KeyControl> {
            new KeyControl(KeyCode.W,            ()=>AddForce(Vector3.forward),     ()=>AddForce(-Vector3.forward)),
            new KeyControl(KeyCode.S,            ()=>AddForce(Vector3.back),        ()=>AddForce(-Vector3.back)),
            new KeyControl(KeyCode.A,            ()=>AddForce(Vector3.left),        ()=>AddForce(-Vector3.left)),
            new KeyControl(KeyCode.D,            ()=>AddForce(Vector3.right),       ()=>AddForce(-Vector3.right)),
            new KeyControl(KeyCode.LeftControl,  ()=>AddForce(Vector3.down),        ()=>AddForce(-Vector3.down)),
            new KeyControl(KeyCode.Space,        ()=>AddForce(Vector3.up),          ()=>AddForce(-Vector3.up)),
            new KeyControl(KeyCode.DownArrow,    ()=>AddForce(Vector3.up,true),     ()=>AddForce(-Vector3.up,true)),
            new KeyControl(KeyCode.UpArrow,      ()=>AddForce(Vector3.down,true),   ()=>AddForce(-Vector3.down,true)),
            new KeyControl(KeyCode.Q,            ()=>AddForce(Vector3.left,true),   ()=>AddForce(-Vector3.left,true)),
            new KeyControl(KeyCode.E,            ()=>AddForce(Vector3.right,true),  ()=>AddForce(-Vector3.right,true)),
            new KeyControl(KeyCode.LeftArrow,    ()=>AddForce(Vector3.left,true),   ()=>AddForce(-Vector3.left,true)),
            new KeyControl(KeyCode.RightArrow,   ()=>AddForce(Vector3.right,true),  ()=>AddForce(-Vector3.right,true)),
            new KeyControl(KeyCode.LeftAlt,      ()=>Breaks(),                      ()=>{ }),
            new KeyControl(KeyCode.F,            ()=>ToggleTakeOff(),               ()=>{ })
        };
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //check if there is a player and ship is not landed and controllable
        if (!ai_pilot && !landed && controllable)
        {
            //get player input
            foreach (var key in boundKeys)
            {
                //if key is pressed and is not already pressed, add force/torque
                if (Input.GetKeyDown(key.key) && !pressedKeys.Contains(key))
                {
                    //execute key down action
                    key.keyDown.Invoke();
                    //add key to list of pressed keys
                    pressedKeys.Add(key);
                }

                if (Input.GetKeyUp(key.key) && pressedKeys.Contains(key))
                {
                    //execute key up action
                    key.keyUp.Invoke();
                    //remove key from list of pressed keys
                    pressedKeys.Remove(key);
                }
            }

        }
    }

    void AddForce(Vector3 force, bool rotation = false)
    {
        if (!landed && controllable)
        {
            if (!rotation)
            {
                shipBody.AddForce(force);
            }
            else
            {
                shipBody.AddTorque(force);
            }
        }
    }

    void Breaks()
    {
        while (pressedKeys.Contains(boundKeys[13]))
        {
            //lerp ship velocity to 0
            shipBody.velocity = Vector3.Lerp(shipBody.velocity, Vector3.zero, .1f);
            //lerp ship angular velocity to 0
            shipBody.angularVelocity = Vector3.Lerp(shipBody.angularVelocity, Vector3.zero, .1f);
        }
    }

    void ToggleTakeOff()
    {
        //check if not landed and can land
            //remove control of the ship
            //lock ship position
            //play landing animation
       //else if landed and can taleoff
            //unlock ship position
            //play takeoff animation
            //give control of the ship
    }

    

    void PlayAnimation(string animName, int layer = -1, float normalizedTime = 0)
    {
        animator.Play(animName, layer, normalizedTime);
    }

    void SetAnimationSpeed(float speed)
    {
        //change animation speed
        animator.speed = speed;
    }

    public void ResetShip()
    {
        //remove all forces
        shipBody.velocity = Vector3.zero;
        //make ship landed
        PlayAnimation("landed");
        landed = true;
        controllable = false;
    }
}
[System.Serializable]
public class KeyControl
{
    public KeyCode key;
    public Action keyDown;
    public Action keyUp;

    public KeyControl(KeyCode key, Action keyDown, Action keyUp)
    {
        this.key = key;
        this.keyDown = keyDown;
        this.keyUp = keyUp;
    }
}
[System.Serializable]
public class FloorSensor
{
    public Ray ray;
    public float maxDist;
    public float distance
    {
        get
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDist))
            {
                return hit.distance;
            }
            else
            {
                return maxDist;
            }
        }
    }

    public FloorSensor(Ray ray, float maxDist)
    {
        this.ray = ray;
        this.maxDist = maxDist;
    }
}
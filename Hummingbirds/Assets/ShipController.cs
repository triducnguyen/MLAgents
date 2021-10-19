using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{

    //whether or not the spacecraft is landed
    public bool landed = true;

    //whether or not the spacraft can be controlled
    public bool controllable = false;

    //reference to ship animator
    public Animator animator;

    //reference to ship rigidbody
    public Rigidbody shipBody;

    //list of forces added to ship
    List<Vector3> currentForces = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //check if ship is controllable and not landed
        if (!landed && controllable)
        {
            //ship can be controlled
        }
    }

    void Land()
    {
        //check if landed

        //play landing animation

        //remove control of the ship
    }

    void Takeoff()
    {
        //check if landed

        //play takeoff animation

        //give control of the ship
    }

    void AddVehicleForce(Vector3 force)
    {
        //add force to vehicle based on direction pressed
    }

    void RemoveAllVehicleForces()
    {
        for (var i = currentForces.Count; i > 0; i--)
        {
            shipBody.AddForce(-currentForces[i]);
            currentForces.RemoveAt(i);
        }
    }

    void RemoveVehicleForce(Vector3 force)
    {
        //remove force to vehicle based on direction pressed
    }

    void Breaks()
    {
        //slowly remove velocity
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.Linq;

public class AlienAgent : Agent
{
    //what the AI can see
    public Camera aiCamera;
    //AI rigidbody
    public Rigidbody rBody;
    //target planet to claim
    public Transform targetFlag;
    //AI reset position
    public Transform resetPosition;
    //ai ship
    public ShipController shipController;

    //whether ai is out of bounds or not
    bool outOfBounds = false;

    //list of flags
    List<FlagpoleController> flags = new List<FlagpoleController>();

    // Start is called before the first frame update
    void Start()
    {
        //get list of flags
        //get flags
        flags = GameObject.FindGameObjectsWithTag("space_flag").ToList().Select(x => x.GetComponent<FlagpoleController>()).ToList();
    }

    //executed at the beginning of training
    public override void OnEpisodeBegin()
    {
        //base.OnEpisodeBegin();

        //move ai back to reset pos
        transform.position = resetPosition.position;
        transform.rotation = resetPosition.rotation;
        //reset ship
        shipController.ResetShip();
        
    }

    //tells the ai to get information about the world
    public override void CollectObservations(VectorSensor sensor)
    {

        //choose a planet if one is not yet selected
        if (targetFlag == null) SetRandomFlagRange(50f);
        //position of target planet
        sensor.AddObservation(targetFlag.position);   //3
        //position of target planet flag

        //position of alien ship
        sensor.AddObservation(transform.position);      //3
        //rotation of alien ship
        sensor.AddObservation(transform.rotation);      //3
        //velecity of alien ship
        sensor.AddObservation(rBody.velocity);          //3
        //whether ship is landed or not
        sensor.AddObservation(shipController.landed);   //1
        //whether ship can land or not
        sensor.AddObservation(shipController.canLand);  //1
        //total: 14

    }

    //when the ai has decided to do something
    public override void OnActionReceived(float[] vectorAction)
    {
        Vector3 direction = Vector3.zero;
        Vector3 rotation = Vector3.zero;
        bool toggleTakeoff = false;
        bool nitro = false;

        //get directions

        //forward
        direction += vectorAction[0] == 1 ? Vector3.forward : Vector3.zero;
        //backward
        direction += vectorAction[0] == 1 ? Vector3.forward : Vector3.zero;
        //left
        direction += vectorAction[0] == 1 ? Vector3.forward : Vector3.zero;
        //right
        direction += vectorAction[0] == 1 ? Vector3.forward : Vector3.zero;
        //up
        direction += vectorAction[0] == 1 ? Vector3.forward : Vector3.zero;
        //down
        direction += vectorAction[0] == 1 ? Vector3.forward : Vector3.zero;


        //get rotations
        //roll+
        rotation += vectorAction[0] == 1 ? Vector3.forward : Vector3.zero;
        //roll-
        rotation += vectorAction[0] == 1 ? Vector3.forward : Vector3.zero;
        //pitch+
        rotation += vectorAction[0] == 1 ? Vector3.forward : Vector3.zero;
        //pitch-
        rotation += vectorAction[0] == 1 ? Vector3.forward : Vector3.zero;
        //yaw+
        rotation += vectorAction[0] == 1 ? Vector3.forward : Vector3.zero;
        //yaw-
        rotation += vectorAction[0] == 1 ? Vector3.forward : Vector3.zero;

        //speed boost
        nitro =  vectorAction[0] == 1;

        //toggle takeoff
        toggleTakeoff = vectorAction[0] == 1;

        //apply ai actions to ship
        shipController.desiredForce = direction;
        shipController.desiredTorque = rotation;

        if (nitro)
        {
            shipController.speed = shipController.baseSpeed + 10;
        }
        else
        {
            shipController.speed = shipController.baseSpeed;
        }

        if (toggleTakeoff)
        {
            shipController.ToggleTakeOff();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "space_flag")
        {
            //attatch to capture event
            FlagpoleController flag = other.GetComponent<FlagpoleController>();
            flag.flagCapture += OnFlagCapture;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "boundary")
        {
            SetReward(-0.1f);
        }
        if (other.tag == "space_flag")
        {
            //detatch from capture event
        }

    }


    public void OnFlagCapture(object flag, FlagCaptureArgs e)
    {
        if (e.claimingTeam == shipController.teamID)
        {
            //ai captured a flag!
            AddReward(1f);

            if (((FlagpoleController)flag).transform == targetFlag)
            {
                //select new target flag
                SetRandomFlagRange(50f);
            }
        }
    }

    //sets AI's target planet to a random planet within a given range.
    //if there are no planets in range, will use closest planet
    public void SetRandomFlagRange(float range)
    {
        List<FlagpoleController> candidates = new List<FlagpoleController>();
        FlagpoleController nearest = null;
        float nearestFlag = 100;
        foreach (var flag in flags)
        {
            var dist = Vector3.Distance(transform.position, flag.transform.position);
            if (dist < range)
            {
                
                candidates.Add(flag);
            }
            if (dist < nearestFlag)
            {
                nearest = flag;
                nearestFlag = dist;
            }
        }

        
        if (candidates.Count > 0)
        {
            targetFlag = candidates[Random.Range(0, candidates.Count)].transform;
        }
        else if (candidates.Count < 1 && nearest != null)
        {
            targetFlag = nearest.transform;
        }
        else
        {
            //no target flags found
            targetFlag = transform;
        }
    }

}

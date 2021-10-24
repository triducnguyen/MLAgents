using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

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


    // Start is called before the first frame update
    void Start()
    {

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
        //sensor.AddObservation(targetFlag.position);   //3
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
        shipController.ControlShip(vectorAction);
    }


    //sets AI's target planet to a random planet within a given range.
    //if there are no planets in range, will use closest planet
    public void SetRandomFlagRange(float range)
    {

    }

}

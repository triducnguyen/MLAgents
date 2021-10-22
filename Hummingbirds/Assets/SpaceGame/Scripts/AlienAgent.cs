using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class AlienAgent : Agent
{
    public Rigidbody rBody;
    public Transform targetPlanet;
    public Transform resetPosition;
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

        //choose a planet
        SetRandomPlanetRange();
        //position of target planet
        sensor.AddObservation(targetPlanet.position);
        //position of target planet flag

        //position of alien ship
        sensor.AddObservation(transform.position);
        //rotation of alien ship
        sensor.AddObservation(transform.rotation);
        //velecity of alien ship
        sensor.AddObservation(rBody.velocity);
        //whether ship is landed or not
        sensor.AddObservation(shipController.landed);
        //whether ship is controllable or not
        sensor.AddObservation(shipController.controllable);
        //whether ship can land or not
        sensor.AddObservation(shipController.canLand);

    }

    //when the ai has decided to do something
    public override void OnActionReceived(float[] vectorAction)
    {
        //base.OnActionReceived(vectorAction);
    }


    //sets AI's target planet to a random planet within a given range.
    //if there are no planets in range, will return closest planet
    public void SetRandomPlanetRange()
    {

    }

}

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

    public override void OnEpisodeBegin()
    {
        //base.OnEpisodeBegin();

        //move ai back to reset pos
        transform.position = resetPosition.position;
        transform.rotation = resetPosition.rotation;
        shipController.ResetShip();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        base.OnActionReceived(vectorAction);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagpoleController : MonoBehaviour
{
    //which team has claimed the flag (this is also used as an index to retrieve the team flag material)
    public int team = 0; //0 is neutral

    //whether the flag is completely hoisted or not
    public bool hoisted = true;

    //flag renderer
    public MeshRenderer renderer;


    //

    //if there is more than one team inside the capture zone
    public bool contested = false;


    // Start is called before the first frame update
    void Start()
    {
        //set flag to team
        renderer.materials = new Material[1] { TeamMaterials.instance.mats[team] };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSpaceshipEnter()
    {
        //a new spaceship has entered this zone!

        //check to see if the zone is contested
        if (contested)
        {

        }
        else
        {

        }
    }

    public void OnSpaceshipExit()
    {

    }
}

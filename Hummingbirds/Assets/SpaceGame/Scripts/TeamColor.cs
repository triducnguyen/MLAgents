using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamColor : MonoBehaviour
{
    public int teamID = 0;
    public int lastID = 0;

    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        SetTeamMat();
    }

    // Update is called once per frame
    void Update()
    {
        if (teamID != lastID)
        {
            SetTeamMat();
            lastID = teamID;
        }
    }

    void SetTeamMat()
    {
        material.SetColor("TeamColor",TeamController.instance.teams[teamID]);
    }
}

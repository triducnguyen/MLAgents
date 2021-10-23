using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : Singleton<TeamController>
{
    public Color[] teams;

    //public Dictionary<int, Color> id2color = new Dictionary<int, Color>();

    protected override void Awake()
    {
        base.Awake();
        //add colors to dictionary
        //for(int i=0; i<teams.Length; i++)
        //{
        //    var color = teams[i];
        //    id2color.Add(i, color);
        //}
    }
}

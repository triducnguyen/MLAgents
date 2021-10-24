using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagpoleController : MonoBehaviour
{
    //which team has claimed the flag (this is also used as an index to retrieve the team flag material)
    public int claimedBy = 0; //0 is neutral

    //whether the flag is being taken
    public bool beingCaptured = false;

    //flag progress
    public float progress
    {
        get
        {
            return _progress;
        }
        set
        {
            if (beingCaptured)
            {
                if (value < 0f)
                {
                    value = 0f;
                }
                _progress = value;
            }
            else
            {
                if (value > 1f)
                {
                    value = 1f;
                }
                _progress = value;
            }
        }
    }
    public float _progress = 1f;

    public float desiredProgress
    {
        get
        {
            return _desiredProgress;
        }
        set
        {
            if (beingCaptured)
            {
                if (value < 0f)
                {
                    value = 0f;
                    //set flag to neutral
                    SetFlagDisplay(0);
                    SetFlagTeam(0);
                }
                _desiredProgress = value;
            }
            else
            {
                if (value > 1f)
                {
                    value = 1f;
                }
                _desiredProgress = value;
            }
        }
    }
    public float _desiredProgress = 1f;

    //flag renderer
    public MeshRenderer renderer;

    //list of ships that are in range of the flagpole
    public List<ShipController> inRange = new List<ShipController>();

    //flagpole animator
    public Animator animator;

    public int uniqueTeams
    {
        get
        {
            HashSet<int> teams = new HashSet<int>();
            foreach (var ship in inRange)
            {
                teams.Add(ship.teamID);
            }
            //Debug.Log("Team Count: " + teams.Count);

            return teams.Count;
        }
    }

    //which team is currently winning the flag
    public int winningTeam
    {
        get
        {
            if (uniqueTeams == 1)
            {
                return inRange[0].teamID;
            }
            //create list of team counts
            Dictionary<int, int> teamCount = new Dictionary<int, int>();
            foreach (var ship in inRange)
            {
                //check if team is in the list or not
                int members = 0;
                if(teamCount.TryGetValue(ship.teamID, out members)){
                    //team was in list. Add one to their count
                    teamCount[ship.teamID] = members + 1;
                }
                else
                {
                    //team was not in list. add team with count of 1
                    teamCount[ship.teamID] = 1;
                }
            }
            
            //get team with max size
            int count = 0;
            List<int> winners = new List<int>();
            foreach (var key in teamCount.Keys)
            {
                var tCount = teamCount[key];
                if (tCount > count)
                {
                    winners.Clear();
                    winners.Add(key);
                    count = teamCount[key];
                }
                else if (tCount == count)
                {
                    winners.Add(key);
                }
            }
            if (winners.Count == 0)
            {
                //no winners
                return 0;
            }
            else if (winners.Count == 1)
            {
                //only one team with highest count
                return winners[0];
            }
            else
            {
                return 0;
            }
            
        }
    }

    public float flagInterval = 1f;
    public float flagSpeed = 0.1f;

    //coroutine
    Coroutine updateFlag;

    // Start is called before the first frame update
    void Start()
    {
        //configure animator
        animator.speed = 0f;
        updateFlag = StartCoroutine(UpdateFlagProgress(flagInterval));
        //set flag to neutral
        SetFlagDisplay(0);
        SetFlagTeam(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        ShipController controller;
        if (other.TryGetComponent(out controller))
        {
            OnSpaceshipEnter(controller);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ShipController controller;
        if (other.TryGetComponent(out controller))
        {
            OnSpaceshipExit(controller);
        }
    }

    public void OnSpaceshipEnter(ShipController controller)
    {
        //a new spaceship has entered this zone
        //Debug.Log("Spaceship has entered");
        if (!inRange.Contains(controller)) { inRange.Add(controller); }
        
    }

    public void OnSpaceshipExit(ShipController controller)
    {
        //Debug.Log("Spaceship has exited");
        //a new spaceship has exited this zone
        if (inRange.Contains(controller)) { inRange.Remove(controller); }
    }

    private void Update()
    {
        //Debug.Log("Before: "+progress+", "+desiredProgress);
        progress = Mathf.Lerp(progress, desiredProgress, .1f);
        //Debug.Log("After: "+progress + ", " + desiredProgress);
        SetFlagPos(progress);
    }

    public void SetFlagDisplay(int team)
    {
        Debug.Log("Team color: "+ TeamManager.instance.teams[team]);
        Debug.Log("Material color: "+renderer.materials[0].GetColor("TeamColor"));
        renderer.materials[0].SetColor("TeamColor", TeamManager.instance.teams[team]);
    }

    public void SetFlagTeam(int team)
    {
        claimedBy = team;
    }

    public void SetFlagPos(float progress)
    {
        animator.Play("flagHoist", -1, progress);
    }

    public IEnumerator UpdateFlagProgress(float updateInterval)
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);
            switch (uniqueTeams)
            {
                case 0:
                    //Debug.Log("No teams");

                    break;
                default:
                    //check that winning team is not neutral (a draw)
                    int winning = winningTeam;
                    if (winning != 0)
                    {
                        //check if team owns flag

                        if (claimedBy != winning)
                        {
                            if (desiredProgress == 0)
                            {
                                //claim flag
                                SetFlagDisplay(winning);
                                SetFlagTeam(winning);
                                desiredProgress = flagSpeed;
                                break;
                            }
                            else
                            {
                                //Debug.Log("Progress > 0");

                                //team is taking flag
                                beingCaptured = true;
                                desiredProgress -= flagSpeed;
                                //Debug.Log(progress);
                            }
                        }
                        else
                        {
                            //team is lifting flag
                            beingCaptured = false;
                            desiredProgress += flagSpeed;
                        }
                    }
                    break;
            }
        }
    }
}

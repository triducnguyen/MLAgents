using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainingAreaManager : MonoBehaviour
{
    //ai in scene
    public List<AlienAgent> agents = new List<AlienAgent>();
    // Start is called before the first frame update

    //score
    public List<FlagpoleController> flags;
    public Dictionary<int, int> team2score = new Dictionary<int, int>();

    //which ai are ready
    List<AlienAgent> ready = new List<AlienAgent>();

    //episode time
    //timer routine
    Coroutine timer;

    //winning team and members
    public Tuple<int, List<AlienAgent>> winners
    {
        get
        {
            //find which team has most score
            int highest = 0;
            List<int> winningTeams = new List<int>();
            List<AlienAgent> members = new List<AlienAgent>();
            foreach (var key in team2score.Keys)
            {
                var val = team2score[key];
                if (val > highest)
                {
                    winningTeams.Clear();
                    winningTeams.Add(key);
                    highest = val;
                }
                else if (val == highest)
                {
                    winningTeams.Add(key);
                }
            }
            switch (winningTeams.Count)
            {
                case 0:
                    return new Tuple<int, List<AlienAgent>>(0, new List<AlienAgent>{ });
                    //nobody scored
                case 1:
                    //one team wins
                    //get all team members
                    foreach (var agent in agents)
                    {
                        if (agent.shipController.teamID == winningTeams[0])
                        {
                            members.Add(agent);
                        }
                    }
                    return new Tuple<int, List<AlienAgent>>(winningTeams[0], members);
                case 2:
                    //tie
                    return new Tuple<int, List<AlienAgent>>(-1, new List<AlienAgent> { });
                default:
                    return new Tuple<int, List<AlienAgent>>(0, new List<AlienAgent> { });
            }
        }
    }

    //whether area is started or not
    public bool started = false;

    int startTime = 200;
    int timeLeft
    {
        get
        {
            return _timeLeft;
        }
        set
        {
            if (value <= 0)
            {
                //out of time
                if (timer != null)
                {
                    StopCoroutine(timer);
                }
                //check winning team
                var team = winners;
                switch (team.Item1)
                {
                    case -1:
                        //tie
                        break;
                    case 0:
                        //punish everyone
                        foreach (var member in agents)
                        {
                            member.AddReward(-.1f);
                        }
                        break;
                    case 1:
                        //one team wins
                        foreach (var member in team.Item2)
                        {
                            member.AddReward(1f);
                        }
                        break;
                }
                StopEpisode();
            }
        }
    }
    int _timeLeft = 200;
    //game timer
    public IEnumerator CountDown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        timeLeft--;
    }

    private void Awake()
    {
        ResetScore();
        
    }

    private void Start()
    {
        //get all flags in scene
        //flags = GameObject.FindGameObjectsWithTag("space_flag").ToList().Select(x => x.GetComponent<FlagpoleController>()).ToList();

    }

    public void AddScore(int team, int score)
    {
        team2score[team] = team2score[team] + score;
    }

    public void ResetScore()
    {
        //give all teams 0 score
        for (int i = 0; i < TeamManager.instance.teams.Length; i++)
        {
            //TeamManager.instance.teams[i];
            team2score[i] = 0;
        }
    }

    //reset ready
    public void ResetReady()
    {
        ready.Clear();
    } 

    //reset flags
    public void ResetFlags()
    {
        foreach (var flag in flags)
        {
            flag.progress = 1;
            flag.desiredProgress = 1;
            flag.claimedBy = 0;
            flag.SetFlagDisplay(0);
        }
    }

    //stops episode
    public void StopEpisode()
    {
        started = false;
        //stop all AI
        foreach (var agent in agents)
        {
            agent.EndEpisode();
        }
        //reset scores
        ResetScore();
        //reset flags
        ResetFlags();
    }

    public void Ready(AlienAgent agent)
    {
        ready.Add(agent);
        if (ready.Count == agents.Count)
        {
            //all agents are ready
            timer = StartCoroutine(CountDown(1));
            started = true;
        }
    }
}

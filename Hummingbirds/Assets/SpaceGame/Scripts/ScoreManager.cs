using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public int totalGameTime
    {
        get
        {
            return _totalGameTime;
        }
        set
        {
            if (value < 0)
            {
                value = 0;
            }
            _totalGameTime = value;
        }
    }
    public int _totalGameTime = 300;
    
    public List<FlagpoleController> flags = new List<FlagpoleController>();
    public Dictionary<int, int> team2score = new Dictionary<int, int>();

    private void Awake()
    {
        ResetScore();
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

    public void TimeOut()
    {

        ResetScore();
    }
}

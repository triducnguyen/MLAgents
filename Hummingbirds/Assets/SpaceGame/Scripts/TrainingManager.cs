using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  TrainingManager: Singleton<TrainingManager>
{
    int startTime = 300;
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
            }
        }
    }
    int _timeLeft = 300;

    //list of our ai
    List<TrainingAreaManager> trainingAreas = new List<TrainingAreaManager>();
    List<TrainingAreaManager> ready = new List<TrainingAreaManager>();
    //timer routine
    Coroutine timer;

    private void Start()
    {
        //begin timer
        timer = StartCoroutine(CountDown(1));
    }

    public IEnumerator CountDown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        timeLeft--;
    }

    public void StopEpisode()
    {
        //stop all areas
        foreach (var area in trainingAreas)
        {
            area.StopEpisode();
        }
        
        
    }

    public void Ready(TrainingAreaManager manager)
    {

        ready.Add(manager);
        if (ready.Count == trainingAreas.Count)
        {
            //reset timer
            timeLeft = startTime;
            //all areas are ready to start
            timer = StartCoroutine(CountDown(1));
        }
    }
}

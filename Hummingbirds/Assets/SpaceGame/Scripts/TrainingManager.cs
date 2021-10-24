using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  TrainingManager: Singleton<TrainingManager>
{
    //list of our ai
    List<AlienAgent> ai = new List<AlienAgent>();

    private void Start()
    {
        //begin timer

    }

    public IEnumerator StopEpisode(float seconds)
    {
        yield return new WaitForSeconds(seconds);

    }
}

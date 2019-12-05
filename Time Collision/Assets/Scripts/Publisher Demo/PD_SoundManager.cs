using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PD_SoundManager : MonoBehaviour
{
    public List<AudioSource> sources;

    private List<float> startTimes = new List<float>() { 0f, 0f, 0f };
    private List<float> stopTimes = new List<float>() { 1f, 1f, -1f };

    public void playSound(int whichSound)
    {
        sources[whichSound].Play();
        sources[whichSound].time = startTimes[whichSound];
    }

    public void Update()
    {
        for(int i = 0; i < sources.Count; i++)
        {
            if(sources[i].time > stopTimes[i] && stopTimes[i] > 0f)
            {
                sources[i].Stop();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeamlessLoop : MonoBehaviour
{
    public AudioSource source1;
    public AudioSource source2;
    public AudioClip clip;
    
    private double nextStartTime;
    private int toggle = 0;

    void Start()
    {
        nextStartTime = AudioSettings.dspTime + 0.2;
        source1.PlayScheduled(nextStartTime);
        nextStartTime += (double)clip.samples / clip.frequency;
    }

    void Update()
    {
        if (AudioSettings.dspTime > nextStartTime - 10.0) // Schedule 1 sec ahead
        {
            AudioSource activeSource = (toggle == 0) ? source2 : source1;
            activeSource.clip = clip;
            activeSource.PlayScheduled(nextStartTime);
            
            double duration = (double)clip.samples / clip.frequency;
            nextStartTime += duration;
            toggle = 1 - toggle;
        }
    }
}
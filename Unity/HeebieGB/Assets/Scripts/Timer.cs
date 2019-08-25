using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private static List<Timer> timers = new List<Timer>();
    public static void UpdateTimers(float time)
    {
        foreach (Timer t in timers){
            t.PassTime(time);
        }
    }

    private float duration = 0.0f;
    private float elapsed = 0.0f;

    private bool ReadyCondition { get { return elapsed > duration; } }

    public Timer(float duration)
    {
        this.duration = duration;
        timers.Add(this);
    }

    ~Timer()
    {
        timers.Remove(this);
    }


    public bool IsDone { get { return ReadyCondition; } }
    public float Elapsed { get { return elapsed; } }
    public float Duration { get { return duration; } }
    public float Remaining { get { return duration - elapsed; } }
    public void PassTime(float time)
    {
        elapsed += time;
    }
}

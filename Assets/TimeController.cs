using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public float slowDownFactor = 0.05f;
    public float slowDownDuration = 2f;

    void Update()
    {
        Time.timeScale = .5f;
        Time.fixedDeltaTime = 0.5f * 0.2f;
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.fixedDeltaTime * slowDownFactor;
    }
}
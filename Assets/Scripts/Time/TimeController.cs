using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public float slowDownFactor = 0.02f;
    public float slowDownTimeScale = 0.2f;

    private float startTimeScale;
    private float startFixedDeltaTime;

    private void Start()
    {
        startTimeScale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update()
    {
        //Time.timeScale = .5f;
        //Time.fixedDeltaTime = 0.5f * 0.2f;
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowDownTimeScale;
        Time.fixedDeltaTime = Time.fixedDeltaTime * slowDownFactor;
    }

    public void DoBaseMotion()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = slowDownFactor * Time.timeScale;
    }
}
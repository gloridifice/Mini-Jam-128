using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float _time;
    /// <summary>
    /// time passed in seconds
    /// </summary>
    public float Tick { get => _time; }

    private void FixedUpdate()
    {
        _time += Time.fixedDeltaTime;
        // Debug.Log("time: " + Tick);
    }
}

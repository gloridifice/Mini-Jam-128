using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PersonStatus
{
    Waiting,
    Saved,
    Died,
}

/// <summary>
/// <para>Class of trapped person.</para>
/// <para>Data only.</para>
/// </summary>
public class TrappedPerson : MonoBehaviour
{
    public float health;
    public Age age;
    // todo: fill in other basic info of person
    
    public TriageTag triageTag = TriageTags.None;
    public PersonStatus status;

    private Timer _timer;

    #region InfoGetting
        
        private bool _isGettingInfo;// TODO: impl change between true and false
        private float _startTime;
        private float _accumulatedTime;
        private float _currentTime;
        [SerializeField] private float timeToGetVoice;
        [SerializeField] private float timeToGetHeartbeat;
        [SerializeField] private float timeToGetFullInfo;
        private event EventHandler ShowVoice;
        private event EventHandler ShowHeartbeat;
        private event EventHandler ShowFullInfo;
        
        private float TimeAccumulation()
        {
            return _timer.Tick - _startTime;
        }

        private void GetStartTime()
        {
            _startTime = _timer.Tick;
        }

        private void CheckPoints()
        {
            if (_isGettingInfo)
            {
                _currentTime = _accumulatedTime + TimeAccumulation();
                if (_currentTime > timeToGetVoice)
                {
                    ShowVoice?.Invoke(this, EventArgs.Empty);
                }

                if (_currentTime > timeToGetHeartbeat)
                {
                    ShowHeartbeat?.Invoke(this, EventArgs.Empty);
                }

                if (_currentTime > timeToGetFullInfo)
                {
                    ShowFullInfo?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                if (_currentTime < timeToGetFullInfo)
                {
                    if (_currentTime >= timeToGetHeartbeat)
                    {
                        _accumulatedTime = timeToGetHeartbeat;
                    }
                    else if (_currentTime >= timeToGetVoice)
                    {
                        _accumulatedTime = timeToGetVoice;
                    }
                    else
                    {
                        _accumulatedTime = 0f;
                    }
                }
                GetStartTime();
            }
        }

        private void EShowVoice(System.Object sender, EventArgs args)
        {
            // TODO: impl needed;
            //Debug.Log("voice");
            ShowVoice -= EShowVoice;
        }

        private void EShowHeartbeat(System.Object sender, EventArgs args)
        {
            // TODO: impl needed;
            //Debug.Log("heartbeat");
            ShowHeartbeat -= EShowHeartbeat;
        }

        private void EShowFullInfo(System.Object sender, EventArgs args)
        {
            // TODO: impl needed;
            //Debug.Log("fullInfo");
            ShowFullInfo -= EShowFullInfo;
        }
        
    #endregion

    private void Awake()
    {
        _timer = GameObject.Find("GameManager").GetComponent<Timer>();
    }

    private void Start()
    {
        GetStartTime();
        ShowVoice += EShowVoice;
        ShowHeartbeat += EShowHeartbeat;
        ShowFullInfo += EShowFullInfo;
    }

    private void Update()
    {
        CheckPoints();
    }

    // todo: figure out how do deal with subtitle, heartbeat and health facts
    // todo: how do a person get saved
    
    
}

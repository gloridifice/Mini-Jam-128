using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
    public uint timeToDie;
    
    public Age age;
    public int Heartbeat => GetHeartbeat(timeToDie);
    public int RespiratoryRate => GetRespiratoryRate(Heartbeat);

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

    // todo: add more model
    private int GetHeartbeat(uint time)
    {
        var exactBPM = GetBPM(time);
        return exactBPM switch
        {
            (>= 0) and (< 45) => 0,
            (>= 45) and (< 60) => 1,
            (>= 60) and (< 100) => 2,
            (>= 100) and (< 120) => 3,
            (>= 120) => 4,
        };
    }

    private uint GetBPM(uint t)
    {
        return t switch
        {
            (>= 0) and (< 40) => (uint)Random.Range(60, 80),
            (>= 40) and (< 120) => (uint)Random.Range(120, 140),
            (>= 120) and (< 220) => (uint)Random.Range(100, 120),
            (>= 220) and (< 300) => (uint)Random.Range(60, 80),
            _ => throw new OverflowException("Impossible time"),
        };
    }

    private int GetRespiratoryRate(int hb)
    {
        return hb;
    }
}

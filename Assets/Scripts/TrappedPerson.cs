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
/// Indicates the stage of heartbeat and respiratory rate.
/// <para>Zero means the person is dead.</para>
/// </summary>
public enum MeasureStage
{
    Lowest,
    Low,
    Normal,
    High,
    Highest,
    Zero,
}

/// <summary>
/// <para>Class of trapped person.</para>
/// </summary>
public class TrappedPerson : MonoBehaviour
{
    public float time;
    
    public Age age;
    public MeasureStage Heartbeat => GetHeartbeat(time - _timer.Tick);
    public MeasureStage RespiratoryRate => Heartbeat;

    public TriageTag triageTag = TriageTags.None;
    public PersonStatus status;

    private Timer _timer;

    #region InfoGetting
        
        private bool _isGettingInfo;// TODO: impl change between true and false
        private int _startTime;
        private int _accumulatedTime;
        private int _currentTime;
        private const int TimeToGetVoice = 3;
        private const int TimeToGetHeartbeat = 6;
        private const int TimeToGetFullInfo = 9;
        private event EventHandler ShowVoice;
        private event EventHandler ShowHeartbeat;
        private event EventHandler ShowFullInfo;
        
        private int TimeAccumulation()
        {
            return _timer.IntTick - _startTime;
        }

        private void GetStartTime()
        {
            _startTime = _timer.IntTick;
        }

        private void CheckPoints()
        {
            if (_isGettingInfo)
            {
                _currentTime = _accumulatedTime + TimeAccumulation();
                if (_currentTime > TimeToGetVoice)
                {
                    ShowVoice?.Invoke(this, EventArgs.Empty);
                }

                if (_currentTime > TimeToGetHeartbeat)
                {
                    ShowHeartbeat?.Invoke(this, EventArgs.Empty);
                }

                if (_currentTime > TimeToGetFullInfo)
                {
                    ShowFullInfo?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                _accumulatedTime = _currentTime switch
                {
                    (< TimeToGetVoice) => 0,
                    (>= TimeToGetVoice) and (< TimeToGetHeartbeat) => TimeToGetVoice,
                    (>= TimeToGetHeartbeat) and (< TimeToGetFullInfo) => TimeToGetHeartbeat,
                    (>= TimeToGetFullInfo) => TimeToGetFullInfo,
                };
                GetStartTime();
            }
        }

        private void EShowVoice(System.Object sender, EventArgs args)
        {
            // TODO: impl needed;
            //Debug.Log("voice");
            ShowVoice -= EShowVoice;// remove this if u want to call every frame
        }

        private void EShowHeartbeat(System.Object sender, EventArgs args)
        {
            // TODO: impl needed;
            //Debug.Log("heartbeat");
            ShowHeartbeat -= EShowHeartbeat;// remove this if u want to call every frame
        }

        private void EShowFullInfo(System.Object sender, EventArgs args)
        {
            // TODO: impl needed;
            //Debug.Log("fullInfo");
            ShowFullInfo -= EShowFullInfo;// remove this if u want to call every frame
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

        //hb = Heartbeat;
    }
    
    // todo: how do a person get saved

    // todo: add more model
    private MeasureStage GetHeartbeat(float leftTime)
    {
        var exactBPM = GetBPM(leftTime);
        Debug.Log(exactBPM);
        return exactBPM switch
        {
            0 => MeasureStage.Zero,
            (> 0) and (< 45) => MeasureStage.Lowest,
            (>= 45) and (< 60) => MeasureStage.Low,
            (>= 60) and (< 100) => MeasureStage.Normal,
            (>= 100) and (< 120) => MeasureStage.High,
            (>= 120) => MeasureStage.Highest,
        };
    }

    private uint GetBPM(float t)
    {
        return t switch
        {
            (>= 0) and (< 40) => (uint)Random.Range(60, 80),
            (>= 40) and (< 120) => (uint)Random.Range(120, 140),
            (>= 120) and (< 220) => (uint)Random.Range(100, 120),
            (>= 220) and (< 300) => (uint)Random.Range(60, 80),
            _ => 0,
        };
    }
}

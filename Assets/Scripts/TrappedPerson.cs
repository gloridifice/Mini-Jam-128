using System;
using System.Collections;
using System.Collections.Generic;
using GameManager;
using TriageTags;
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
    public MeasureStage Heartbeat => GetHeartbeat(time - timer.Tick);
    public MeasureStage RespiratoryRate => Heartbeat;

    public TriageTag triageTag = TriageTags.TriageTags.None;
    public PersonStatus status;

    private Timer timer;

    #region InfoGetting
        
        private bool isGettingInfo;// TODO: impl change between true and false
        private int startTime;
        private int accumulatedTime;
        private int currentTime;
        private const int TimeToGetVoice = 3;
        private const int TimeToGetHeartbeat = 6;
        private const int TimeToGetFullInfo = 9;
        private event EventHandler ShowVoice;
        private event EventHandler ShowHeartbeat;
        private event EventHandler ShowFullInfo;
        
        private int TimeAccumulation()
        {
            return timer.IntTick - startTime;
        }

        private void GetStartTime()
        {
            startTime = timer.IntTick;
        }

        private void CheckPoints()
        {
            if (isGettingInfo)
            {
                currentTime = accumulatedTime + TimeAccumulation();
                if (currentTime > TimeToGetVoice)
                {
                    ShowVoice?.Invoke(this, EventArgs.Empty);
                }

                if (currentTime > TimeToGetHeartbeat)
                {
                    ShowHeartbeat?.Invoke(this, EventArgs.Empty);
                }

                if (currentTime > TimeToGetFullInfo)
                {
                    ShowFullInfo?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                accumulatedTime = currentTime switch
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


    #region GetRescue

    public const int TimeToRescue = 10;// TODO: decide this variable
    [HideInInspector]public int rescueTime;
    private int rescueTimeStart;
    private bool isGettingRescue;
    
    public void StartRescue()
    {
        isGettingRescue = true;
        rescueTimeStart = timer.IntTick;
    }

    public void BreakRescue()
    {
        isGettingRescue = false;
        rescueTime = 0;
    }

    public void GetRescue()
    {
        if (isGettingRescue)
        {
            rescueTime = timer.IntTick - rescueTimeStart;
        }
    }
    
    #endregion
    
    private void Awake()
    {
        timer = GameObject.Find("GameManager").GetComponent<Timer>();
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
        GetRescue();

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

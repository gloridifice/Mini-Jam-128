using System;
using System.Collections;
using System.Collections.Generic;
using GameManager;
using TriageTags;
using UnityEngine;
using UnityEngine.Assertions;
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

public enum SubtitleTag
{
    General,
}

/// <summary>
/// <para>Class of trapped person.</para>
/// </summary>
public class TrappedPerson : MonoBehaviour
{
    public int time;
    public int TimeRemain => time - Timer.IntTick;

    public Age age;
    public MeasureStage Heartbeat => GetHeartbeat(time - Timer.IntTick);
    public uint BPM => GetBPM(time - Timer.IntTick);
    public MeasureStage RespiratoryRate => Heartbeat;
    public int InjurySeverity => GetInjurySeverity();

    public SubtitleTag subtitleTag;
    private TriageTag triageTag = TriageTags.TriageTags.None;

    public TriageTag TriageTag
    {
        get => triageTag;
        set
        {
            OnTrappedPersonTagChanged?.Invoke(triageTag, value);
            triageTag = value;
        }
    }
    private PersonStatus status = PersonStatus.Waiting;
    public PersonStatus Status
    {
        get => status;
        set
        {
            OnPersonStatusChanged.Invoke(status, value);
            status = value;
        }
    }

    public event Action<TriageTag, TriageTag> OnTrappedPersonTagChanged = (triageTag, tag1) => { };

    public event Action<PersonStatus, PersonStatus> OnPersonStatusChanged = (status, arg3) => { };

    private Timer Timer => LevelManager.Instance.Timer;

    #region InfoGetting

    private bool isGettingInfo; // TODO: impl change between true and false
    private int startTime;
    private int accumulatedTime;
    private int currentTime;
    private const int TimeToGetVoice = 3;
    private const int TimeToGetHeartbeat = 6;
    private const int TimeToGetFullInfo = 9;
    // private event EventHandler ShowVoice;
    // private event EventHandler ShowHeartbeat;
    // private event EventHandler ShowFullInfo;

    private int TimeAccumulation()
    {
        return Timer.IntTick - startTime;
    }

    private void GetStartTime()
    {
        startTime = Timer.IntTick;
    }

    private void CheckInspectingStatus()
    {
        if (isGettingInfo)
        {
            currentTime = accumulatedTime + TimeAccumulation();
            // if (currentTime > TimeToGetVoice)
            // {
            //     ShowVoice?.Invoke(this, EventArgs.Empty);
            // }
            //
            // if (currentTime > TimeToGetHeartbeat)
            // {
            //     ShowHeartbeat?.Invoke(this, EventArgs.Empty);
            // }
            //
            // if (currentTime > TimeToGetFullInfo)
            // {
            //     ShowFullInfo?.Invoke(this, EventArgs.Empty);
            // }
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

    // private void EShowVoice(System.Object sender, EventArgs args)
    // {
    //     // TODO: impl needed;
    //     //Debug.Log("voice");
    //     ShowVoice -= EShowVoice; // remove this if u want to call every frame
    // }
    //
    // private void EShowHeartbeat(System.Object sender, EventArgs args)
    // {
    //     // TODO: impl needed;
    //     //Debug.Log("heartbeat");
    //     ShowHeartbeat -= EShowHeartbeat; // remove this if u want to call every frame
    // }
    //
    // private void EShowFullInfo(System.Object sender, EventArgs args)
    // {
    //     // TODO: impl needed;
    //     //Debug.Log("fullInfo");
    //     ShowFullInfo -= EShowFullInfo; // remove this if u want to call every frame
    // }

    #endregion


    #region GetRescue

    public const int TimeToRescue = 10;
    [HideInInspector] public int rescueTime;
    private int rescueTimeStart;
    private bool isGettingRescue;

    public void StartRescue()
    {
        isGettingRescue = true;
        rescueTimeStart = Timer.IntTick;
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
            rescueTime = Timer.IntTick - rescueTimeStart;
        }
    }

    #endregion

    private void Start()
    {
        GetStartTime();
        // ShowVoice += EShowVoice;
        // ShowHeartbeat += EShowHeartbeat;
        // ShowFullInfo += EShowFullInfo;
    }

    private void Update()
    {
        CheckInspectingStatus();
        GetRescue();
    }

    // todo: add more model
    private MeasureStage GetHeartbeat(int leftTime)
    {
        var exactBPM = GetBPM(leftTime);
        // Debug.Log(exactBPM);
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

    private uint GetBPM(int t)
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

    private int GetInjurySeverity()
    {
        int injurySeverity = 0;
        injurySeverity += (int)age.age;
        injurySeverity += ISFromHeartbeat(Heartbeat);
        injurySeverity += ISFromRespiratoryRate(RespiratoryRate);

        return injurySeverity;
    }

    private int ISFromRespiratoryRate(MeasureStage respiratoryRate)
    {
        switch (respiratoryRate)
        {
            case MeasureStage.Lowest:
            case MeasureStage.Highest:
                return 2;
            case MeasureStage.Low:
            case MeasureStage.High:
                return 1;
            case MeasureStage.Normal:
                return 0;
            case MeasureStage.Zero:
            default:
                throw new ArgumentOutOfRangeException(nameof(respiratoryRate), respiratoryRate, null);
        }
    }

    private int ISFromHeartbeat(MeasureStage heartbeat)
    {
        switch (heartbeat)
        {
            case MeasureStage.Lowest:
            case MeasureStage.Highest:
                return 3;
            case MeasureStage.Low:
                return 2;
            case MeasureStage.Normal:
                return 0;
            case MeasureStage.High:
                return 1;
            case MeasureStage.Zero:
            default:
                throw new ArgumentOutOfRangeException(nameof(heartbeat), heartbeat, null);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using GameManager;
using TMPro;
using TriageTags;
using UI.Viewport;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
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
    public MeasureStage Heartbeat => GetHeartbeat(time - Timer.Tick);
    public uint BPM => GetBPM(time - Timer.Tick);
    public MeasureStage RespiratoryRate => Heartbeat;
    public int InjurySeverity => GetInjurySeverity();

    private TriageTag triageTag = TriageTags.TriageTags.None;

    public TriageTag TriageTag
    {
        get => triageTag;
        set
        {
            if (value != triageTag)
            {
                onTagChanged?.Invoke(this, triageTag, value);
            }

            triageTag = value;
        }
    }

    private PersonStatus status = PersonStatus.Waiting;

    public PersonStatus Status
    {
        get => status;
        set
        {
            if (status != value)
            {
                onStatusChanged.Invoke(this, status, value);
            }

            status = value;
        }
    }

    [HideInInspector] public UnityEvent<TrappedPerson, TriageTag, TriageTag> onTagChanged;
    [HideInInspector] public UnityEvent<TrappedPerson, PersonStatus, PersonStatus> onStatusChanged;
    [HideInInspector] public FloatEvent onScanningProgressChanged;
    [HideInInspector] public FloatEvent onRescueProgressChanged;
    [HideInInspector] public FloatEvent onHeartBeatChanged;
    [HideInInspector] public UnityEvent onSoundUnlock;
    [HideInInspector] public UnityEvent onHealthUnlock;
    [HideInInspector] public UnityEvent onLifeUnlock;
    [HideInInspector] public UnityEvent onGetRescue;

    public ViewportTrappedPerson viewportPerson;

    private Timer Timer => LevelManager.Instance.Timer;

    #region Scanning

    public class ScanningInfo
    {
        public bool isUnlock;
        public float unlockTime;
        public float rate;

        public ScanningInfo(float unlockTime, float fullTime)
        {
            this.unlockTime = unlockTime;
            this.rate = unlockTime / fullTime;
        }
    }

    [HideInInspector] public bool isGettingInfo;
    public bool ShouldShowScanning => scanningCurrentTime > 0;
    private float scanningStartTime;
    private float scanningAccumulatedTime;
    private float scanningCurrentTime;
    private const float FullScanTime = 9;
    public ScanningInfo soundScanningInfo = new(3, FullScanTime);
    public ScanningInfo healthScanningInfo = new(6, FullScanTime);
    public ScanningInfo lifeScanningInfo = new(9, FullScanTime);
    private event EventHandler ShowVoice;
    private event EventHandler ShowHeartbeat;
    private event EventHandler ShowFullInfo;
    private bool scanned;

    private float TimeAccumulation()
    {
        return Timer.Tick - scanningStartTime;
    }

    private void RefreshStartTime()
    {
        scanningStartTime = Timer.Tick;
    }

    private bool isScanningProgressClear = false;
    public float ScanningRate => Mathf.Clamp(scanningCurrentTime / FullScanTime, 0f, 1f);

    private void ScanningUpdate()
    {
        if (scanned) return;
        if (isGettingInfo)
        {
            isScanningProgressClear = false;
            scanningCurrentTime = scanningAccumulatedTime + TimeAccumulation();
            if (scanningCurrentTime > soundScanningInfo.unlockTime)
            {
                ShowVoice?.Invoke(this, EventArgs.Empty);
            }

            if (scanningCurrentTime > healthScanningInfo.unlockTime)
            {
                ShowHeartbeat?.Invoke(this, EventArgs.Empty);
            }

            if (scanningCurrentTime > lifeScanningInfo.unlockTime)
            {
                ShowFullInfo?.Invoke(this, EventArgs.Empty);
            }

            onScanningProgressChanged.Invoke(ScanningRate);
        }
        else
        {
            RefreshStartTime();
            if (!isScanningProgressClear)
            {
                if (soundScanningInfo.isUnlock) scanningAccumulatedTime = soundScanningInfo.unlockTime;
                if (healthScanningInfo.isUnlock) scanningAccumulatedTime = healthScanningInfo.unlockTime;
                scanningCurrentTime = scanningAccumulatedTime;
                onScanningProgressChanged.Invoke(ScanningRate);
                isScanningProgressClear = true;
            }
        }
    }

    private void EShowVoice(System.Object sender, EventArgs args)
    {
        soundScanningInfo.isUnlock = true;
        onSoundUnlock.Invoke();
        ShowVoice -= EShowVoice; // remove this if u want to call every frame
    }

    private void EShowHeartbeat(System.Object sender, EventArgs args)
    {
        healthScanningInfo.isUnlock = true;
        onHealthUnlock.Invoke();
        ShowHeartbeat -= EShowHeartbeat; // remove this if u want to call every frame
    }

    private void EShowFullInfo(System.Object sender, EventArgs args)
    {
        lifeScanningInfo.isUnlock = true;
        onLifeUnlock.Invoke();
        scanned = true;
        ShowFullInfo -= EShowFullInfo; // remove this if u want to call every frame
    }

    #endregion


    #region GetRescue

    public const float TimeToRescue = 10;
    [HideInInspector] public float rescueTime;
    public bool ShouldShowRescueBar => rescueTime > 0 && PersonStatus.Waiting == Status;
    private float rescueTimeStart;
    private bool isGettingRescue;

    public void StartRescue()
    {
        isGettingRescue = true;
        rescueTimeStart = Timer.Tick;
    }

    public void BreakRescue()
    {
        isGettingRescue = false;
        rescueTime = 0;
    }

    public void RescueUpdate()
    {
        if (isGettingRescue)
        {
            rescueTime = Timer.Tick - rescueTimeStart;
            if (rescueTime > TimeToRescue) onGetRescue.Invoke();
            onRescueProgressChanged.Invoke(rescueTime / TimeToRescue);
        }
    }

    #endregion


    private void Start()
    {
        RefreshStartTime();
        ShowVoice += EShowVoice;
        ShowHeartbeat += EShowHeartbeat;
        ShowFullInfo += EShowFullInfo;
        LevelManager.Instance.CameraController.OnCameraMoved += OnCameraMoved;
    }

    [HideInInspector] public UnityEvent<TrappedPerson> onBeFound;
    [HideInInspector] public bool isFound;

    private void Update()
    {
        ScanningUpdate();
        RescueUpdate();
    }

    void OnCameraMoved(CameraController controller)
    {
        CheckBeFound(controller);
    }

    void CheckBeFound(CameraController controller)
    {
        if (!isFound)
        {
            Vector2 pos = transform.position.XZ();
            Vector2 center = controller.Center;
            Vector2 size = controller.worldSpaceSize;

            Vector2 point = center - size / 2;
            Vector2 sub = pos - point;
            if (sub.x < size.x && sub.x > 0 && sub.y < size.y && sub.y > 0)
            {
                isFound = true;
                onBeFound.Invoke(this);
            }
        }
    }

    // todo: add more model
    private MeasureStage GetHeartbeat(float leftTime)
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
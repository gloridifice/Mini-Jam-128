using System;
using System.Collections;
using System.Collections.Generic;
using GameManager;
using Level;
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

public enum SubtitleTag
{
    General,
    Child,
    PregnantWoman,
}

public enum Severity
{
    Low,
    Mid,
    High
}

/// <summary>
/// <para>Class of trapped person.</para>
/// </summary>
public class TrappedPerson : MonoBehaviour
{
    public static float GetLifeTime(Severity s)
    {
        switch (s)
        {
            case Severity.High: return LevelManager.Instance.highLifeTime;
            case Severity.Mid: return LevelManager.Instance.midLifeTime;
            case Severity.Low: return LevelManager.Instance.lowLifeTime;
        }

        return 0;
    }

    public float Time => GetLifeTime(severity) - timeOffset;
    public float timeOffset;
    public float TimeRemain => Time - Timer.Tick;
    public Severity severity;

    public TrappedPersonInfo personalInfo;
    public MeasureStage Heartbeat => GetHeartbeat((int)TimeRemain, severity);
    private uint bpm;
    public MeasureStage RespiratoryRate => Heartbeat;
    public int InjurySeverity => GetInjurySeverity();

    public SubtitleTag subtitleTag;
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

    public ViewportTrappedPerson viewportPerson;

    private Timer Timer => LevelManager.Instance.Timer;

    #region Scanning

    [HideInInspector] public bool isGettingInfo;

    public class ScanningInfo
    {
        public bool isUnlock;

        public float UnlockRate
        {
            get
            {
                switch (type)
                {
                    case ScanningInfoType.Sound: return LevelManager.Instance.soundScanningTime;
                    case ScanningInfoType.Health: return LevelManager.Instance.healthScanningTime;
                    case ScanningInfoType.Life: return LevelManager.Instance.lifeScanningTime;
                }

                return 0f;
            }
        }
        private ScanningInfoType type;
        public float UnlockTime => UnlockRate * LevelManager.Instance.scanningTime;
        public float Rate => UnlockTime / LevelManager.Instance.scanningTime;

        public ScanningInfo(ScanningInfoType type)
        {
            this.type = type;
        }
    }

    public enum ScanningInfoType
    {
        Sound,Health,Life
    }

    public ScanningInfo soundScanningInfo = new(ScanningInfoType.Sound);
    public ScanningInfo healthScanningInfo = new(ScanningInfoType.Health);
    public ScanningInfo lifeScanningInfo = new(ScanningInfoType.Life);
    public bool ShouldShowScanning => scanningCurrentTime > 0;
    private float scanningStartTime;
    private float scanningAccumulatedTime;
    private float scanningCurrentTime;
    private float FullScanTime => LevelManager.Instance.scanningTime;
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
            if (scanningCurrentTime > soundScanningInfo.UnlockTime)
            {
                ShowVoice?.Invoke(this, EventArgs.Empty);
            }

            if (scanningCurrentTime > healthScanningInfo.UnlockTime)
            {
                ShowHeartbeat?.Invoke(this, EventArgs.Empty);
            }

            if (scanningCurrentTime > lifeScanningInfo.UnlockTime)
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
                if (soundScanningInfo.isUnlock) scanningAccumulatedTime = soundScanningInfo.UnlockTime;
                if (healthScanningInfo.isUnlock) scanningAccumulatedTime = healthScanningInfo.UnlockTime;
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

    public float TimeToRescue => LevelManager.Instance.rescueTime;
    [HideInInspector] public float rescueTime;
    public bool ShouldShowRescueBar => rescueTime > 0 && PersonStatus.Waiting == Status;
    private float rescueTimeStart;
    private bool isGettingRescue;

    public void StartRescue()
    {
        if (!isGettingRescue)
        {
         isGettingRescue = true;
         rescueTimeStart = Timer.Tick;
        }
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

    public string GetSubtitle()
    {
        return LevelManager.Instance.SubtitlesPool.GetSubtitle(subtitleTag, InjurySeverity);
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

    private MeasureStage GetHeartbeat(int leftTime, Severity s)
    {
        var exactBPM = GetBPM(leftTime, severity);
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

    public uint GetBPM(int t, Severity s)
    {
        return s switch
        {
            Severity.Low => LowSeverityBPM(t),
            Severity.Mid => MidSeverityBPM(t),
            Severity.High => HighSeverityBPM(t),
            _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
        };
    }

    private uint LowSeverityBPM(int t)
    {
        var remain = t / LevelManager.Instance.lowLifeTime;
        return remain switch
        {
            (>= 0) and (< 60 / 390f) => (uint)Random.Range(30, 40),
            (>= 60 / 390f) and (< 150 / 390f) => (uint)Random.Range(120, 130),
            (>= 150 / 390f) and (< 270 / 390f) => (uint)Random.Range(80, 110),
            (>= 270 / 390f) and (< 1f) => (uint)Random.Range(60, 65),
            _ => 0,
        };
    }

    private uint MidSeverityBPM(int t)
    {
        var remain = t / LevelManager.Instance.midLifeTime;
        return remain switch
        {
            (>= 0f) and (< 40 / 300f) => (uint)Random.Range(30, 40),
            (>= 40 / 300f) and (< 120 / 300f) => (uint)Random.Range(120, 140),
            (>= 120 / 300f) and (< 220 / 300f) => (uint)Random.Range(100, 120),
            (>= 220 / 300f) and (< 1f) => (uint)Random.Range(60, 80),
            _ => 0,
        };
    }

    private uint HighSeverityBPM(int t)
    {
        var remain = t / LevelManager.Instance.highLifeTime;
        return remain switch
        {
            (>= 0f) and (< 60 / 240f) => (uint)Random.Range(130, 150),
            (>= 60 / 240f) and (< 90 / 240f) => (uint)Random.Range(60, 120),
            (>= 90 / 240f) and (< 180 / 240f) => (uint)Random.Range(120, 130),
            (>= 180 / 240f) and (< 1f) => (uint)Random.Range(80, 100),
            _ => 0,
        };
    }

    public TrappedPersonSettlementInfo GetSettlementInfo()
    {
        TrappedPersonSettlementInfo info = new TrappedPersonSettlementInfo(TimeRemain, personalInfo);
        return info;
    }

    private int GetInjurySeverity()
    {
        int injurySeverity = 0;
        injurySeverity += (int)personalInfo.age.AgePeriod;
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
                return 0;
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
                return 0;
            default:
                throw new ArgumentOutOfRangeException(nameof(heartbeat), heartbeat, null);
        }
    }
}
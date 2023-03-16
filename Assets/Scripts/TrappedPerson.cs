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

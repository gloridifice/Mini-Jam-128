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
    // todo: fill in other basic info of person
    public TriageTag triageTag = TriageTags.None;
    public PersonStatus status;

    // todo: figure out how do deal with subtitle, heartbeat and health facts
    // todo: how do a person get saved
}

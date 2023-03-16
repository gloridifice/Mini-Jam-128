using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriageTag
{
    None,
    Black,
    Red,
    Yellow,
    Green,
}

public enum PersonStatus
{
    Waiting,
    Saved,
    Died,
}

public class TrappedPerson : MonoBehaviour
{
    public float health;
    // todo: fill in other basic info of person
    public TriageTag triageTag;
    public PersonStatus status;

    // todo: figure out how do deal with subtitle, heartbeat and health facts
    // todo: how do a person get saved
    public void SetTag(TriageTag t)
    {
        triageTag = t;
    }

}

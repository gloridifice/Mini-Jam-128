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

/// <summary>
/// Pure data structure of trapped person.
/// </summary>
public class TrappedPerson : MonoBehaviour
{
    public float health;
    // todo: fill in other basic info of person
    public TriageTag triageTag;

}

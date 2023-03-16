using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    private List<TrappedPerson> _savedPersons;
    private List<TrappedPerson> _diedPersons;

    public List<TrappedPerson> SavedPersons
    {
        get => _savedPersons;
    }

    public List<TrappedPerson> DiedPersons
    {
        get => _diedPersons;
    }

    private void Awake()
    {
        _savedPersons = new List<TrappedPerson>();
        _diedPersons = new List<TrappedPerson>();
    }

    public void AddSavedPerson(TrappedPerson tar)
    {
        _savedPersons.Add(tar);
    }

    public void AddDiedPerson(TrappedPerson tar)
    {
        _diedPersons.Add(tar);
    }
}

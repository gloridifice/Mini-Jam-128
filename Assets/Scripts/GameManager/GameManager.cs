using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TrappedPerson[] trappedPersons;

    private Timer _timer;

    private void Start()
    {
        _timer = GetComponent<Timer>();
    }

    private void Update()
    {
        FreshPersonStatus();
    }

    private void FreshPersonStatus()
    {
        foreach (var person in trappedPersons)
        {
            if (person.status == PersonStatus.Died) continue;
            if (person.health < _timer.Tick)
            {
                person.status = PersonStatus.Died;
                // Debug.Log("person " + person.name + " died");
            }
        }
    }
}

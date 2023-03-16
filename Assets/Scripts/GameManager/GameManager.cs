using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Counter), typeof(Ending))]
public class GameManager : MonoBehaviour
{
    public TrappedPerson[] trappedPersons;
    private Counter _counter;

    private Timer _timer;

    private void Start()
    {
        _timer = GetComponent<Timer>();
        _counter = GetComponent<Counter>();
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
            if (person.timeToDie < _timer.Tick)
            {
                person.status = PersonStatus.Died;
                _counter.AddDiedPerson(person);
                // Debug.Log("person " + person.name + " died");
            }
        }
    }
}

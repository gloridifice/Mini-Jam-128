using System;
using System.Collections.Generic;

public class Rescue
{
    private List<TrappedPerson> _high;
    private Queue<TrappedPerson> _mid;
    private Queue<TrappedPerson> _low;
    private Dictionary<TrappedPerson, Priority> _table;
    
    // todo: how to deal with the breaking change of high?

    public enum Priority
    {
        High,
        Mid,
        Low,
    }

    public void Insert(TrappedPerson person, Priority priority)
    {
        if (!_table.ContainsKey(person))
        {
            _table.Add(person, priority);
            InnerInsert(person, priority);
        }
        else
        {
            Remove(person, priority);
        }
    }

    private void Remove(TrappedPerson person, Priority priority)
    {
        switch (priority)
        {
            case Priority.High:
                throw new NotImplementedException();
                break;
            case Priority.Mid:
                
                break;
            case Priority.Low:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(priority), priority, null);
        }
    }

    private void InnerInsert(TrappedPerson person, Priority priority)
    {
        switch (priority)
        {
            case Priority.High:
                throw new NotImplementedException();
                break;
            case Priority.Mid:
                _mid.Enqueue(person);
                break;
            case Priority.Low:
                _mid.Enqueue(person);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(priority), priority, null);
        }
    }
    
    
}
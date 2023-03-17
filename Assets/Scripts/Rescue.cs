using System;
using System.Collections.Generic;

public class Rescue
{
    private List<TrappedPerson> high;
    private Queue<TrappedPerson> mid;
    private Queue<TrappedPerson> low;
    private Dictionary<TrappedPerson, Priority> table;
    
    // todo: how to deal with the breaking change of high?

    public enum Priority
    {
        High,
        Mid,
        Low,
    }

    public void Insert(TrappedPerson person, Priority priority)
    {
        if (!table.ContainsKey(person))
        {
            table.Add(person, priority);
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
                mid.Enqueue(person);
                break;
            case Priority.Low:
                mid.Enqueue(person);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(priority), priority, null);
        }
    }
    
    
}
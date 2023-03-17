using System;
using System.Collections.Generic;

[Serializable]
public class Rescue
{
    private List<TrappedPerson> high;
    private List<TrappedPerson> mid;
    private List<TrappedPerson> low;
    public List<(TrappedPerson, Priority)> rescuing;
    private const int RescuingLimit = 2;
    private Dictionary<TrappedPerson, Priority> table;

    public int Count => high.Count + mid.Count + low.Count;
    public int HighCount => high.Count;

    public enum Priority
    {
        High,
        Mid,
        Low,
    }

    public Rescue()
    {
        high = new List<TrappedPerson>();
        mid = new List<TrappedPerson>();
        low = new List<TrappedPerson>();
        rescuing = new List<(TrappedPerson, Priority)>();
        table = new Dictionary<TrappedPerson, Priority>();
    }
    
    public TrappedPerson PushBack()
    {
        if (rescuing.Count >= RescuingLimit) return null;
        var bottom = Pop();
        if (bottom == null) return bottom;
        rescuing.Add((bottom, table[bottom]));
        return bottom; 
    }

    public TrappedPerson PopFront()
    {
        if (rescuing.Count == 0) return null;
        var top = rescuing[0];
        table.Remove(top.Item1);
        rescuing.Remove(top);
        return top.Item1;
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
            RemovePerson(person, table[person]);
            InnerInsert(person, priority);
        }
    }

    public bool Find(TrappedPerson person)
    {
        return table.ContainsKey(person);
    }

    public TrappedPerson Remove(TrappedPerson person)
    {
        if (!Find(person)) return null;
        foreach (var (pe, pr) in rescuing)
        {
            if (pe == person) rescuing.Remove((pe, pr));
            table.Remove(person);
            return pe;
        }
        var priority = table[person];
        RemovePerson(person, priority);
        RemoveTable(person);
        return person;
    }
    
    private void PushFront(TrappedPerson person)
    {
        rescuing.Insert(0, (person, table[person]));
        if (rescuing.Count > RescuingLimit)
        {
            PopBack();
        }
    }
    
    private void PopBack()
    {
        var bottom = rescuing[^1];
        rescuing.Remove(bottom);
        Insert(bottom.Item1, bottom.Item2);
        bottom.Item1.BreakRescue();
    }

    
    /// <summary>
    /// Get the top in the whole queue include high, mid and low.
    /// </summary>
    /// <returns>The element of highest priority or <b>null</b></returns>
    private TrappedPerson Top()
    {
        if (HighCount != 0)
        {
            return high[^1];
        }

        if (mid.Count != 0)
        {
            return mid[0];
        }

        if (low.Count != 0)
        {
            return low[0];
        }

        return null;
    }

    /// <summary>
    /// Pop out the element of highest priority in the whole queue.
    /// </summary>
    /// <returns>Is there anything to pop.</returns>
    private TrappedPerson Pop()
    {
        var top = Top();
        RemovePerson(top, table[top]);
        return top;
    }

    /// <summary>
    /// Get the top in the high queue.
    /// </summary>
    /// <returns>The element of highest priority or <b>null</b></returns>
    private TrappedPerson HighTop()
    {
        if (HighCount == 0) return null;
        return high[^1];
    }

    /// <summary>
    /// Pop out the element of highest priority in the high queue.
    /// </summary>
    /// <returns>Is there anything to pop.</returns>
    private TrappedPerson HighPop()
    {
        var top = HighTop();
        RemovePerson(top, Priority.High);
        return top;
    }
    
    private void RemoveTable(TrappedPerson person)
    {
        if (!table.ContainsKey(person))  return;
        table.Remove(person);
    }
    
    private void RemovePerson(TrappedPerson person, Priority priority)
    {
        switch (priority)
        {
            case Priority.High:
                high.Remove(person);
                break;
            case Priority.Mid:
                mid.Remove(person);
                break;
            case Priority.Low:
                low.Remove(person);
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
                PushFront(person);
                break;
            case Priority.Mid:
                mid.Add(person);
                break;
            case Priority.Low:
                low.Add(person);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(priority), priority, null);
        }
    }
    
    
}
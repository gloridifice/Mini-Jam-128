using System;
using System.Collections.Generic;
using System.Linq;

public class Rescue
{
    private List<TrappedPerson> high;
    private List<TrappedPerson> mid;
    private List<TrappedPerson> low;
    private Dictionary<TrappedPerson, Priority> table;

    public int Count => high.Count + mid.Count + low.Count;
    public int HighCount => high.Count;
    
    // todo: how to deal with the breaking change of high?

    public enum Priority
    {
        High,
        Mid,
        Low,
    }

    /// <summary>
    /// Get the top in the whole queue include high, mid and low.
    /// </summary>
    /// <returns>The element of highest priority or <b>null</b></returns>
    public TrappedPerson Top()
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
    public bool Pop()
    {
        if (Count == 0) return false;
        Remove(Top(), table[Top()]);
        return true;
    }

    /// <summary>
    /// Get the top in the high queue.
    /// </summary>
    /// <returns>The element of highest priority or <b>null</b></returns>
    public TrappedPerson HighTop()
    {
        if (HighCount == 0) return null;
        return high[^1];
    }

    /// <summary>
    /// Pop out the element of highest priority in the high queue.
    /// </summary>
    /// <returns>Is there anything to pop.</returns>
    public bool HighPop()
    {
        if (HighCount == 0) return false;
        Remove(HighTop(), Priority.High);
        return true;
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
            InnerInsert(person, priority);
        }
    }

    private void Remove(TrappedPerson person, Priority priority)
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
                high.Add(person);
                break;
            case Priority.Mid:
                mid.Add(person);
                break;
            case Priority.Low:
                mid.Add(person);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(priority), priority, null);
        }
    }
    
    
}
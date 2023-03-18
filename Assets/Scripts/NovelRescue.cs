using System;
using System.Collections.Generic;
using TriageTags;

[Serializable]
public class NovelRescue
{
    public const int RescuingLimit = 2;
    
    public List<TrappedPerson> high;
    public List<TrappedPerson> mid;
    public List<TrappedPerson> low;
    public List<TrappedPerson> rescuing;

    public NovelRescue()
    {
        high = new List<TrappedPerson>();
        mid = new List<TrappedPerson>();
        low = new List<TrappedPerson>();
        rescuing = new List<TrappedPerson>();
    }
    
    public void Insert(TrappedPerson person, TriageTag triageTag)
    {
        // 1. if this is a first enqueue
        if (person.triageTag == TriageTags.TriageTags.None)
        {
            InnerInsert(person, triageTag);
        }
        // 2. if this is a changing tag
        else if (person.triageTag != TriageTags.TriageTags.Black)
        {
            if (!rescuing.Contains(person))
            {
                GetOriginalQueue(person).Remove(person);
                InnerInsert(person, triageTag);
            }
            else
            {
                // if this person is in rescuing
                // we should consider if it will still be the first one after its changing
                // if so, we should do nothing but change its tag
                if (triageTag == TriageTags.TriageTags.Red)
                {
                    rescuing.Remove(person);
                    InnerInsert(person, TriageTags.TriageTags.Red);
                }
                // else
                // {
                //     if (CountBeforeInsert(triageTag) != 0)
                //     {
                //         rescuing.Remove(person);
                //         person.BreakRescue();
                //         InnerInsert(person, triageTag);
                //     }
                // }
            }
        }

        person.triageTag = triageTag;
        // todo: invoke an event to tell ui that we have changed a tag
    }

    public void ShiftUp()
    {
        if (high.Count + mid.Count + low.Count == 0) return;
        if (high.Count != 0)
        {
            rescuing.Add(high[^1]);
            high.Remove(high[^1]);
        }
        else if (mid.Count != 0)
        {
            rescuing.Add(mid[0]);
            mid.Remove(mid[0]);
        }
        else
        {
            rescuing.Add(low[0]);
            low.Remove(low[0]);
        }
    }

    public void Remove(TrappedPerson person)
    {
        if (person.triageTag == TriageTags.TriageTags.None || person.triageTag == TriageTags.TriageTags.Black) return;
        if (rescuing.Contains(person))
        {
            rescuing.Remove(person);
            person.BreakRescue();
        }
        GetOriginalQueue(person).Remove(person);
    }

    private void InnerInsert(TrappedPerson person, TriageTag triageTag)
    {
        if (triageTag == TriageTags.TriageTags.None || triageTag == TriageTags.TriageTags.Black)
        {
            throw new ArgumentOutOfRangeException(nameof(triageTag), triageTag, "Can't set the tag to None!");
        }

        if (triageTag == TriageTags.TriageTags.Red)
        {
            // todo: deal with red tag
            rescuing.Insert(0, person);
            Reordering();
        }

        if (triageTag == TriageTags.TriageTags.Yellow)
        {
            mid.Add(person);
        }

        if (triageTag == TriageTags.TriageTags.Green)
        {
            low.Add(person);
        }
    }

    private void Reordering()
    {
        if (rescuing.Count > RescuingLimit)
        {
            var last = rescuing[^1];
            // remove the last one
            rescuing.RemoveAt(RescuingLimit);
            // no need to change its tag
            // put it back to its queue
            var originalQueue = GetOriginalQueue(last);
            if (originalQueue == high)
            {
                high.Add(last);
            }
            else
            {
                originalQueue.Insert(0, last);
            }
            // stop its rescuing
            last.BreakRescue();
        }
    }

    private List<TrappedPerson> GetOriginalQueue(TrappedPerson person)
    {
        var triageTag = person.triageTag;

        if (triageTag == TriageTags.TriageTags.Red)
        {
            return high;
        }

        if (triageTag == TriageTags.TriageTags.Yellow)
        {
            return mid;
        }

        if (triageTag == TriageTags.TriageTags.Green)
        {
            return low;
        }
        
        throw new ArgumentOutOfRangeException(nameof(triageTag), triageTag, "Can't set the tag to None or Black!");
        
    }

    private int CountBeforeInsert(TriageTag target)
    {
        if (target == TriageTags.TriageTags.Yellow) return high.Count;
        if (target == TriageTags.TriageTags.Green) return high.Count + mid.Count;

        throw new ArgumentOutOfRangeException(nameof(target), target, "This can only be yellow or green");
    }
}
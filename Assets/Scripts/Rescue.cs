using System.Collections.Generic;

public class Rescue
{
    private List<TrappedPerson> _high;
    private Queue<TrappedPerson> _mid;
    private Queue<TrappedPerson> _low;

    public enum Priority
    {
        High,
        Mid,
        Low,
    }

    public void Insert(TrappedPerson person, Priority priority)
    {
    }
}
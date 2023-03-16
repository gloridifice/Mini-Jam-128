using System;

public enum AgePeriod
{
    Child = 1,
    Teen = -1,
    Adult = -2,
    MidAge = 1,
    Elder = 2,
}

[Serializable]
public class Age
{
    public uint age;

    public AgePeriod AgePeriod =>
        age switch
        {
            (>= 0) and (<= 12) => AgePeriod.Child,
            (>= 13) and (<= 18) => AgePeriod.Teen,
            (>= 19) and (<= 45) => AgePeriod.Adult,
            (>= 46) and (<= 60) => AgePeriod.MidAge,
            (>= 61) => AgePeriod.Elder,
        };
}
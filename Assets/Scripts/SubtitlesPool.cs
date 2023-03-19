using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class SubtitlesPool : MonoBehaviour
{
    public Dictionary<SubtitleTag, List<string>> highPool;
    public Dictionary<SubtitleTag, List<string>> midPool;
    public Dictionary<SubtitleTag, List<string>> lowPool;
    private Subtitle[] subtitles;

    public string GetSubtitle(SubtitleTag subtitleTag, int injurySeverity)
    {
        var context = injurySeverity switch
        {
            (>= -2) and (<= 0) => lowPool,
            (>= 1) and (<= 4) => midPool,
            (>= 5) and (<= 7) => highPool,
            _ => throw new ArgumentOutOfRangeException(nameof(injurySeverity), injurySeverity,
                "Invalid injury severity"),
        };

        return InnerGetSubtitle(context, subtitleTag);
    }

    private string InnerGetSubtitle(Dictionary<SubtitleTag, List<string>> context, SubtitleTag subtitleTag)
    {
        var general = context[SubtitleTag.General];
        var totalCount = general.Count;
        if (subtitleTag != SubtitleTag.General)
        {
            totalCount += context[subtitleTag].Count;
        }

        var index = GetRandomIndex(totalCount);
        if (index < general.Count)
        {
            return general[index];
        }
        else if (subtitleTag != SubtitleTag.General)
        {
            var newIndex = index - general.Count;
            return context[subtitleTag][newIndex];
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, "Invalid index for this subtitle pool");
        }
    }

    private void Awake()
    {
        highPool = new Dictionary<SubtitleTag, List<string>>();
        midPool = new Dictionary<SubtitleTag, List<string>>();
        lowPool = new Dictionary<SubtitleTag, List<string>>();
        LoadSubtitles();
    }

    private int GetRandomIndex(int limit)
    {
        return Random.Range(0, limit);
    }

    private void LoadSubtitles()
    {
        subtitles = Resources.LoadAll<Subtitle>("");
        foreach (var subtitle in subtitles) 
        {
            highPool.Add(subtitle.subtitleTag, subtitle.high);
            midPool.Add(subtitle.subtitleTag, subtitle.mid);
            lowPool.Add(subtitle.subtitleTag, subtitle.low);
        }
    }
}

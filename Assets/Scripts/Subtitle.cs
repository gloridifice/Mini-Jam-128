using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Subtitle", menuName = "Subtitle", order = 0)]
public class Subtitle : ScriptableObject
{
    public SubtitleTag subtitleTag;

    public List<string> high;
    public List<string> mid;
    public List<string> low;
}

using UnityEngine;

namespace TriageTags
{
    public static class TriageTags
    {
        public static readonly TriageTag Red = new TriageTag(Color.red);
        public static readonly TriageTag Yellow = new TriageTag(Color.yellow);
        public static readonly TriageTag Green = new TriageTag(Color.green);
        public static readonly TriageTag Black = new TriageTag(Color.black);
        public static readonly TriageTag None = new TriageTag(Color.clear);
    }
}
using UnityEngine;

namespace TriageTags
{
    public static class TriageTags
    {
        public static readonly TriageTag Red = new(Color.red, TriageTagType.Red,
            UniversalUtils.ColorFrom255(201,80,80), UniversalUtils.ColorFrom255(105,38,38));

        public static readonly TriageTag Yellow = new(Color.yellow, TriageTagType.Yellow,
            UniversalUtils.ColorFrom255(191,147,36), UniversalUtils.ColorFrom255(79,70,47));

        public static readonly TriageTag Green = new(Color.green, TriageTagType.Green,
            UniversalUtils.ColorFrom255(96,168,74), UniversalUtils.ColorFrom255(62,77,62));

        public static readonly TriageTag Black = new(Color.grey, TriageTagType.Black, new Color(0.2f, 0.2f, 0.2f),
            new Color(.2f, .2f, .2f));

        public static readonly TriageTag None = new(Color.clear, TriageTagType.None, Color.clear, Color.clear);

        public static TriageTag GetInstance(this TriageTagType type)
        {
            switch (type)
            {
                case TriageTagType.Red: return Red;
                case TriageTagType.Yellow: return Yellow;
                case TriageTagType.Green: return Green;
                case TriageTagType.Black: return Black;
                default: return None;
            }
        }
    }

    public enum TriageTagType
    {
        None,
        Red,
        Yellow,
        Green,
        Black
    }
}
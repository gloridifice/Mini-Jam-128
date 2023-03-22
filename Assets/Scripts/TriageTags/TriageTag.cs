using UnityEngine;

namespace TriageTags
{
    public class TriageTag
    {
        public Color color;
        public Color uiLightColor;
        public Color uiDarkColor;
        public TriageTagType type;

        public TriageTag(Color color, TriageTagType type, Color uiLightColor, Color uiDarkColor)
        {
            this.color = color;
            this.type = type;
            this.uiDarkColor = uiDarkColor;
            this.uiLightColor = uiLightColor;
        }
    }
}
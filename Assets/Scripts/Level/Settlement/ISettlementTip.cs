using UI.Settlement;
using UnityEngine;

namespace Level
{
    public interface ISettlementTip
    {
        bool GetSettlementTip(SettlementUIManager  manager, out string tip);
    }

    public class DyingTip : ISettlementTip
    {
        public bool GetSettlementTip(SettlementUIManager  manager, out string tip)
        {
            tip = "";
            int count = 0;
            foreach (var info in manager.PersonCounter.SavedPersonSettlementInfos)
            {
                if (info.timeRemain < 10)
                {
                    count++;
                }
            }

            if (count > 0)
            {
                tip = count + " people were rescued with their lives dying.";
                return true;
            }

            return false;
        }
    }

    public class FamilyTip : ISettlementTip
    {
        public bool GetSettlementTip(SettlementUIManager  manager, out string tip)
        {
            tip = "";
            int count = 0;
            foreach (var info in manager.PersonCounter.SavedPersonSettlementInfos)
            {
                if (info.personalInfo.familiesCount > 0)
                {
                    count++;
                }
            }

            if (count > 0)
            {
                tip = count + " families have a chance to be reunited.";
                return true;
            }

            return false;
        }
    }

    public class PartTip : ISettlementTip
    {
        private string bodyPart;

        public PartTip(string bodyPart)
        {
            this.bodyPart = bodyPart;
        }

        public bool GetSettlementTip(SettlementUIManager manager, out string tip)
        {
            tip = "";
            int size = manager.PersonCounter.SavedPersonSettlementInfos.Count;

            if (size > 0 && Random.Range(0f, 1f) > 0.5f)
            {
                int count = Random.Range(1, (int)(size / 2));
                tip = count + " saved from losing their " + bodyPart;
                return true;
            }

            return false;
        }
    }

    public class ReturnToWorkTip : ISettlementTip
    {
        public bool GetSettlementTip( SettlementUIManager manager, out string tip)
        {
            tip = "";
            int count = 0;
            foreach (var info in manager.PersonCounter.SavedPersonSettlementInfos)
            {
                if (info.personalInfo.age.AgePeriod == AgePeriod.Adult)
                {
                    count++;
                }
            }

            if (count > 0)
            {
                int rCount = Random.Range(1, count);
                tip = rCount + " return to work.";
                return true;
            }

            return false;
        }
    }
}

using System.Collections.Generic;

namespace Level
{
    public class LevelSettlementInfo
    {
        public List<TrappedPersonSettlementInfo> savedPersonInfo;
        public int savedPersonCount;
        public int foundPersonCount;
        public int allPersonCount;

        public LevelSettlementInfo(List<TrappedPersonSettlementInfo> savedPersonInfo, int savedPersonCount, int foundPersonCount, int allPersonCount)
        {
            this.savedPersonInfo = savedPersonInfo;
            this.savedPersonCount = savedPersonCount;
            this.foundPersonCount = foundPersonCount;
            this.allPersonCount = allPersonCount;
        }
    }
}
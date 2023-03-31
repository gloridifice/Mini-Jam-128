namespace Level
{
    public class TrappedPersonSettlementInfo
    {
        public float timeRemain;
        public TrappedPersonInfo personalInfo;

        public TrappedPersonSettlementInfo(float timeRemain, TrappedPersonInfo personalInfo)
        {
            this.timeRemain = timeRemain;
            this.personalInfo = personalInfo;
        }
    }
}
namespace Code.Util
{
    public class CountCondition : UnitSkillCondition
    {
        private int _needCount;
        private int _curCount;

        public override bool CanUseSkill
        {
            get => _curCount >= _needCount;
            protected set { }
        }

        public void SetNeedCount(int needCount)
        {
            _needCount = needCount;
            _curCount = 0;
        }

        public void AddCount(int count) => _curCount += count;
        
        public override void ResetCondition()
        {
            _curCount = 0;
        }
    }
}
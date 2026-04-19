namespace EventSystem
{
    public static class LevelEvents
    {
        public static readonly AddExpEvent AddExpEvent = new AddExpEvent();
        public static readonly ChangeLevelEvent ChangeLevelEvent = new ChangeLevelEvent();
        public static readonly ChangeExpEvent ChangeExpEvent = new ChangeExpEvent();
        public static readonly ChangeRequiredExpEvent ChangeRequiredExpEvent = new ChangeRequiredExpEvent();
    }

    public class AddExpEvent : GameEvent
    {
        public float addExp;

        public AddExpEvent Initializer(float exp)
        {
            addExp = exp;
            return this;
        }
    }
    
    public class ChangeLevelEvent : GameEvent
    {
        public int currentLevel;
        
        public ChangeLevelEvent Initializer(int level)
        {
            currentLevel = level;
            return this;
        }
    }
    
    public class ChangeExpEvent : GameEvent
    {
        public float currentExp;
        
        public ChangeExpEvent Initializer(float exp)
        {
            currentExp = exp;
            return this;
        }
    }
    
    public class ChangeRequiredExpEvent : GameEvent
    {
        public float requiredExp;
        
        public ChangeRequiredExpEvent Initializer(float exp)
        {
            requiredExp = exp;
            return this;
        }
    }
}
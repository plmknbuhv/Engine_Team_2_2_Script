namespace EventSystem
{
    public static class CostEvents
    {
        public static readonly FailUseCostEvent FailUseCostEvent = new FailUseCostEvent();
        public static readonly ChangeCostAmountEvent ChangeCostAmountEvent = new ChangeCostAmountEvent();
    }

    public class FailUseCostEvent : GameEvent
    {
        public int amount;

        public FailUseCostEvent Initializer(int count)
        {
            amount = count;
            return this;
        }
    }
    
    public class ChangeCostAmountEvent : GameEvent
    {
        public int amount;

        public ChangeCostAmountEvent Initializer(int count)
        {
            amount = count;
            return this;
        }
    }
}
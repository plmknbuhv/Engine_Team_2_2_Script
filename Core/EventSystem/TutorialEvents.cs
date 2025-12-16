namespace EventSystem
{
    public static class TutorialEvents
    {
        public static readonly NextStepTutorialEvent NextStepTutorialEvent = new NextStepTutorialEvent();
        public static readonly ViewTutorialMessageEvent ViewTutorialMessageEvent = new ViewTutorialMessageEvent();
    }

    public class ViewTutorialMessageEvent : GameEvent
    {
        public string message;

        public ViewTutorialMessageEvent Initializer(string str)
        {
            message = str;
            return this;
        }
    }
    
    public class NextStepTutorialEvent : GameEvent
    { }
}
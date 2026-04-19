using Code.UI.Logics;
using Code.UI.Logics.StateMachine;

namespace EventSystem
{
    public class UIEvents
    {
        public static readonly UIStateChangeEvent UIStateChangeEvent = new UIStateChangeEvent();
        public static readonly UIFadeStartEvent UIFadeStartEvent = new UIFadeStartEvent();
        public static readonly UIFadeCompleteEvent UIFadeCompleteEvent = new UIFadeCompleteEvent();
    }
    public class UIStateChangeEvent : GameEvent
    {
        public UIStateType NewStateType { get; private set; }

        public UIStateChangeEvent Initializer(UIStateType newStateType = UIStateType.None)
        {
            NewStateType = newStateType;
            return this;
        }
    }
    
    public class UIFadeStartEvent : GameEvent
    {
        public bool IsFadeIn { get; private set; }

        public UIFadeStartEvent Initializer(bool isFadeIn)
        {
            IsFadeIn = isFadeIn;
            return this;
        }
    }
    
    public class UIFadeCompleteEvent : GameEvent
    {
        public bool IsFadeIn { get; private set; }

        public UIFadeCompleteEvent Initializer(bool isFadeIn)
        {
            IsFadeIn = isFadeIn;
            return this;
        }
    }
}
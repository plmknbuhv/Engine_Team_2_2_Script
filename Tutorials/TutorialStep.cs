using System;
using EventSystem;
using UnityEngine;

namespace Code.Tutorials
{
    public abstract class TutorialStep : MonoBehaviour
    {
        [SerializeField] protected GameEventChannelSO tutorialChannel;
        [field: SerializeField] public TutorialPartDataSO TutorialData { get; protected set; }

        public virtual void Enter()
        {
            tutorialChannel.RaiseEvent(TutorialEvents.ViewTutorialMessageEvent.Initializer(TutorialData.explanation));
        }

        public virtual void UpdateStep()
        {
            
        }

        public virtual void End()
        {
        }

        protected void NextStep()
        {
            tutorialChannel.RaiseEvent(TutorialEvents.NextStepTutorialEvent);
        }
        
        #if UNITY_EDITOR

        private void OnValidate()
        {
            if (TutorialData == null) return;
            if (string.IsNullOrEmpty(TutorialData.name)) return;
            
            name = TutorialData.name;
        }

#endif
    }
}
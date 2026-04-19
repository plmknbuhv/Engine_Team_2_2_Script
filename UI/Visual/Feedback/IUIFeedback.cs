using UnityEngine;

namespace Code.UI.Visual.Feedback
{
    public interface IUIFeedback
    {
        public string FeedbackName { get;}
        public void Initialize(GameObject tar);
        public void Play();
    }
}
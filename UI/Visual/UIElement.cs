using System;
using System.Collections.Generic;
using System.Linq;
using Code.UI.Visual.Style;
using Code.UI.Visual.Feedback;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.UI.Visual
{
    public class UIElement : MonoBehaviour
    {
        [SerializeField] private bool isRootElement;
        [SerializeField] private Transform stylesContainer;
        private IStyle[] styles;
        private UIElement[] childElements;
        private Dictionary<string, List<IUIFeedback>> feedbacks;


        private void Awake()
        {
            childElements = GetComponentsInChildren<UIElement>(true).Where(e => e != this).ToArray();

            if (isRootElement) return;
            styles = stylesContainer.GetComponents<IStyle>();
            foreach (var style in styles)
            {
                style.Initialize(gameObject);
            }

            var feedbackComponents = stylesContainer.GetComponentsInChildren<IUIFeedback>();
            feedbacks = new Dictionary<string, List<IUIFeedback>>();
            foreach (var feedback in feedbackComponents)
            {
                feedback.Initialize(gameObject);
                if (!feedbacks.ContainsKey(feedback.FeedbackName))
                    feedbacks[feedback.FeedbackName] = new List<IUIFeedback>();
                feedbacks[feedback.FeedbackName].Add(feedback);
            }
        }

        #region Style

        public async UniTask AddState(string state, int priority = 0, bool waitForChildren = false)
        {
            if (childElements != null)
            {
                if (waitForChildren)
                {
                    var tasks = childElements.Select(child => child.AddState(state, priority, true)).ToArray();
                    await UniTask.WhenAll(tasks);
                }
                else
                {
                    foreach (var child in childElements)
                        child.AddState(state, priority).Forget();
                }
            }

            if (!isRootElement && styles != null)
            {
                await UniTask.WhenAll(styles.Select(s => s.AddState(state, priority)));
            }
        }

        public async UniTask RemoveState(string state, bool waitForChildren = false)
        {
            if (childElements != null)
            {
                if (waitForChildren)
                {
                    var tasks = childElements.Select(child => child.RemoveState(state, true)).ToArray();
                    await UniTask.WhenAll(tasks);
                }
                else
                {
                    foreach (var child in childElements)
                        child.RemoveState(state).Forget();
                }
            }

            if (!isRootElement && styles != null)
            {
                await UniTask.WhenAll(styles.Select(s => s.RemoveState(state)));
            }
        }

        public async UniTask ClearStates()
        {
            if (childElements != null)
            {
                foreach (var child in childElements)
                    child.ClearStates().Forget();
            }

            if (!isRootElement && styles != null)
            {
                await UniTask.WhenAll(styles.Select(s => s.ClearStates()));
            }
        }

        #endregion

        #region Feedback

        public void PlayFeedback(string feedbackName)
        {
            if (!isRootElement && feedbacks != null)
            {
                if (feedbacks.TryGetValue(feedbackName, out var feedback))
                {
                    foreach (var fb in feedback)
                        fb.Play();
                }
            }

            if (childElements == null) return;
            foreach (var child in childElements)
                child.PlayFeedback(feedbackName);
        }

        #endregion
    }
}
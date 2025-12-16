using System;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Logics.StateMachine
{
    public class BasicUIState : MonoBehaviour, IUIState
    {
        [field: SerializeField] public UIStateType StateType { get; private set; }
        [SerializeField] private UIElement canvasElement;
        [SerializeField] private Canvas canvas;
        [SerializeField] private GraphicRaycaster graphicRaycaster;

        private const string _showKey = "show";

        public async UniTask OnEnter()
        {
            canvas.enabled = true;
            await canvasElement.AddState(_showKey, 10);
            if (graphicRaycaster == null) return;
            graphicRaycaster.enabled = true;
        }

        public async UniTask OnExit()
        {
            if (graphicRaycaster != null)
                graphicRaycaster.enabled = false;
            await canvasElement.RemoveState(_showKey);
            canvas.enabled = false;
        }
    }
}
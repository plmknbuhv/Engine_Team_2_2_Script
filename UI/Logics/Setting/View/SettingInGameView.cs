using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using Settings.InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Logics.Setting.View
{
    public class SettingInGameView : SettingView
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private GraphicRaycaster graphicRaycaster;
        [SerializeField] private UIElement canvasElement;
        [SerializeField] private PlayerInputSO playerInputSO;

        private const string ShowKey = "show";
        public override async UniTask OnEnter()
        {
            playerInputSO.SetInputEnabled(false);
            base.OnEnter().Forget();
            canvas.enabled = true;
            await canvasElement.AddState(ShowKey);
            graphicRaycaster.enabled = true;
        }

        public override async UniTask OnExit()
        {
            base.OnExit().Forget();
            graphicRaycaster.enabled = false;
            await canvasElement.RemoveState(ShowKey, true);
            canvas.enabled = false;
            playerInputSO.SetInputEnabled(true);
        }
    }
}
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Visual.Style
{
    public class LayoutElementStyle : BaseStyle<LayoutElement, LayoutElementStyleData>
    {
        private bool _isFinished = false;

        protected override async UniTask ApplyStyle(LayoutElementStyleData currentState)
        {
            if (_isInitialized)
            {
                _isFinished = false;
                DOVirtual.Float(target.flexibleWidth, currentState.FlexibleWidth, currentState.Duration,
                        v => target.flexibleWidth = v).SetEase(currentState.Ease).SetUpdate(true)
                    .OnComplete(() => _isFinished = true);
                DOVirtual.Float(target.flexibleHeight, currentState.FlexibleHeight, currentState.Duration,
                        v => target.flexibleHeight = v).SetEase(currentState.Ease).SetUpdate(true)
                    .OnComplete(() => _isFinished = true);
                await UniTask.WaitUntil(() => _isFinished);
            }
            else
            {
                target.flexibleWidth = currentState.FlexibleWidth;
                target.flexibleHeight = currentState.FlexibleHeight;
            }
        }
    }

    [System.Serializable]
    public class LayoutElementStyleData : BaseStyleData
    {
        public float FlexibleWidth;
        public float FlexibleHeight;
    }
}
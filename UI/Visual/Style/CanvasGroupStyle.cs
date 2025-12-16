using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.UI.Visual.Style
{
    public class CanvasGroupStyle : BaseStyle<CanvasGroup, CanvasGroupStyleData>
    {
        protected override async UniTask ApplyStyle(CanvasGroupStyleData currentState)
        {
            if (_isInitialized)
            {
                DOVirtual.Float(target.alpha, currentState.Alpha, currentState.Duration, value => target.alpha = value)
                    .SetEase(currentState.Ease).SetUpdate(true);
                await UniTask.WaitForSeconds(currentState.Duration, true);
            }

            target.alpha = currentState.Alpha;
        }
    }

    [Serializable]
    public class CanvasGroupStyleData : BaseStyleData
    {
        public float Alpha = 1;
    }
}
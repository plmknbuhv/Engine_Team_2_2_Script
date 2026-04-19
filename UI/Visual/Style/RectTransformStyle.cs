using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.UI.Visual.Style
{
    public class RectTransformStyle : BaseStyle<RectTransform, RectTransformStyleData>
    {
        private Tweener _scaleTweener;
        private Tweener _rotateTweener;

        protected override async UniTask ApplyStyle(RectTransformStyleData currentState)
        {
            if (_isInitialized)
            {
                _scaleTweener?.Kill();
                _rotateTweener?.Kill();

                var targetScaleVec3 = new Vector3(currentState.Scale.x, currentState.Scale.y, 1);

                _scaleTweener = target.DOScale(targetScaleVec3, currentState.Duration).SetEase(currentState.Ease)
                    .SetUpdate(true);
                _rotateTweener = target.DORotate(currentState.Rotation, currentState.Duration)
                    .SetEase(currentState.Ease).SetUpdate(true);

                await UniTask.WaitForSeconds(currentState.Duration, true);
            }
            else
            {
                target.localScale = new Vector3(currentState.Scale.x, currentState.Scale.y, 1);
                target.rotation = Quaternion.Euler(currentState.Rotation);
            }
        }
    }

    [Serializable]
    public class RectTransformStyleData : BaseStyleData
    {
        public Vector2 Scale = Vector2.one;
        public Vector3 Rotation = Vector3.zero;
    }
}
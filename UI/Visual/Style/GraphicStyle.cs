using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Visual.Style
{
    public class GraphicStyle : BaseStyle<Graphic, GraphicBaseStyleData>
    {
        protected override async UniTask ApplyStyle(GraphicBaseStyleData currentState)
        {
            if (_isInitialized)
            {
                target.DOColor(currentState.Color, currentState.Duration).SetEase(currentState.Ease).SetUpdate(true);
                await UniTask.WaitForSeconds(currentState.Duration, true);
            }

            if (target != null)
                target.color = currentState.Color;
        }
    }

    [Serializable]
    public class GraphicBaseStyleData : BaseStyleData
    {
        public Color Color = Color.white;
    }
}
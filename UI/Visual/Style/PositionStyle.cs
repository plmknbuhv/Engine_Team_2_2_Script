using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.UI.Visual.Style
{
    public class PositionStyle : BaseStyle<Transform, PositionStyleData>
    {
        public override void Initialize(GameObject tar)
        {
            target = tar.GetComponent<Transform>();

            foreach (var styleData in styleDatas)
            {
                if (!styles.ContainsKey(styleData.State))
                    styles.Add(styleData.State, styleData);
            }
            
            foreach (var style in styles.Values)
            {
                style.Position = style.Target.position;
                if (!style.UseX)
                    style.Position.x = transform.position.x;
                if (!style.UseY)
                    style.Position.y = transform.position.y;
            }
            
            UpdateCurrentState().Forget();
        }

        protected override async UniTask ApplyStyle(PositionStyleData currentState)
        {
            if (_isInitialized)
            {
                target.DOMove(currentState.Position, currentState.Duration).SetEase(currentState.Ease);
                await UniTask.WaitForSeconds(currentState.Duration);
            }
            else
            {
                target.position = currentState.Position;
            }
        }
    }

    [Serializable]
    public class PositionStyleData : BaseStyleData
    {
        public Transform Target;
        public bool UseX = true;
        public bool UseY = true;
        [HideInInspector] public Vector3 Position;
    }
}
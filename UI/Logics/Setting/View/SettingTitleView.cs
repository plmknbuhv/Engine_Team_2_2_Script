using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Logics.Setting.View
{
    public class SettingTitleView : SettingView
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private GraphicRaycaster graphicRaycaster;
        [Header("Show Camera Setting")]
        [SerializeField] private Transform viewPoint;
        [SerializeField] private Transform titlePoint;
        [SerializeField] private Transform settingPoint;
        [SerializeField] private float moveDuration = 0.5f;
        [SerializeField] private Ease moveEase = Ease.OutBack;
        [Header("Elements")]
        [SerializeField] private Transform hidePosition;
        [SerializeField] private Transform[] elements;
        [SerializeField] private float duration = .1f;
        [SerializeField] private float delay = 0.1f;
        [SerializeField] private Ease hideEase = Ease.InBack;
        [SerializeField] private Ease showEase = Ease.OutBack;

        
        private Dictionary<Transform, Vector3> _positionMap;
        
        protected override void Awake()
        {
            base.Awake();
            _positionMap = new Dictionary<Transform, Vector3>();
            foreach (var element in elements)
            {
                _positionMap[element] = element.position;
            }
        }

        #region Show and Hide
        
        public override async UniTask OnEnter()
        {
            base.OnEnter().Forget();
            canvas.enabled = true;
            viewPoint.DOMove(settingPoint.position, moveDuration).SetEase(moveEase).SetUpdate(true);
            await ShowElements();
            graphicRaycaster.enabled = true;
        }

        public override async UniTask OnExit()
        {
            base.OnExit().Forget();
            graphicRaycaster.enabled = false;
            viewPoint.DOMove(titlePoint.position, moveDuration).SetEase(moveEase).SetUpdate(true);
            await HideElements();
            canvas.enabled = false;
        }
        
        private async UniTask HideElements()
        {
            foreach (var element in elements)
            {
                _positionMap[element] = element.position;
            }
            
            foreach (var trm in _positionMap.Keys.Reverse())
            {
                var pos =trm.position;
                pos.x = hidePosition.position.x;
                trm.DOMove(pos, duration).SetEase(hideEase).SetUpdate(true);
                await UniTask.WaitForSeconds(delay, true);
            }
            await UniTask.WaitForSeconds(duration - delay, true);
        }
        
        private async UniTask ShowElements()
        {
            Vector3 pos;

            foreach (var trm in _positionMap.Keys)
            {
                pos = _positionMap[trm];
                trm.DOMove(pos, duration).SetEase(showEase).SetUpdate(true);
                await UniTask.WaitForSeconds(delay, true);
            }
            await UniTask.WaitForSeconds(duration - delay, true);
        }
        
        #endregion
    }
}

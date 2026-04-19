using Cannons.StatSystem;
using Code.Units;
using EventSystem;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.Events;

namespace Cannons
{
    public class CannonExplosionBombCompo : MonoBehaviour
    {
        public UnityEvent OnBombEvent;
        
        [SerializeField] private GameEventChannelSO cannonChannel;
        [SerializeField] private GameEventChannelSO feedbackChannel;
        [SerializeField] private PoolItemSO explosionEffectItem;
        
        [Header("Bomb Session")] 
        [SerializeField] private float bombRadius = 1;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private int maxCheckCount = 5;
        private Collider[] impactTargetCols;

        [Inject] private CannonStatCompo _statCompo;
        
        
        private void Awake()
        {
            impactTargetCols = new Collider[maxCheckCount];
            cannonChannel.AddListener<CannonShootExplosionEvent>(HandleShootExplosion);
        }

        private void OnDestroy()
        {
            cannonChannel.RemoveListener<CannonShootExplosionEvent>(HandleShootExplosion);
        }

        private void HandleShootExplosion(CannonShootExplosionEvent evt)
        {
            float size = (float)evt.sizeType + _statCompo.GetStatValue(CannonStatType.EXPLOSION_RANGE);
            
            RangeApplyDamage(evt.endPos, size);
            
            //Event
            var effectEvt = FeedbackEvents.PlayEffectFeedbackEvent
                .Initializer(explosionEffectItem, evt.endPos, size);
            feedbackChannel.RaiseEvent(effectEvt);
            
            OnBombEvent?.Invoke();
        }
        
        public void RangeApplyDamage(Vector3 endPos, float size = 1)
        {
            var hitCount = Physics.OverlapSphereNonAlloc(endPos, bombRadius * size, impactTargetCols, enemyLayer);

            for (int i = 0; i < hitCount; i++)
            {
                if (impactTargetCols[i].TryGetComponent(out Unit unit) && unit.TeamType == UnitTeamType.Enemy)
                {
                    unit.ApplyDamage((int)_statCompo.GetStatValue(CannonStatType.EXPLOSION_DAMAGE), null);
                }
            }
        }
    }
}
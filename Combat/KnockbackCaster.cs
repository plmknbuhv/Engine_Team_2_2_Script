using System;
using Code.Entities;
using Code.Util;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Combat
{
    //나중에 ICaster로 분리해서 UnitDetector와 KnockbackCaster에 구현
    public class KnockbackCaster : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private int detectCnt;
        [SerializeField] private float detectRange;
        [SerializeField] private float yThreshold = 0.2f;
        [SerializeField] private LayerMask targetLayer;

        [Header("Test")]
        [SerializeField] private float testForce;
        
        private Entity _entity;
        private Collider[] _results;

        [ContextMenu("Cast")]
        public void TestCast()
        {
            _results ??= new Collider[detectCnt];
            CastKnockback(testForce, null);
        }
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _results = new Collider[detectCnt];
        }

        private void Update()
        {
            /*if(Keyboard.current.tKey.wasPressedThisFrame)
                TestCast();*/
        }

        public void CastKnockback(float knockbackForce, Entity dealer, int damage = 0)
        {
            int cnt = Physics.OverlapSphereNonAlloc(transform.position, detectRange, _results, targetLayer);
            
            if(cnt == 0) return;

            for (int i = 0; i < cnt; i++)
            {
                Collider target = _results[i];

                if(target.transform.position.y - transform.position.y > yThreshold) continue;
                
                if (target.TryGetComponent(out ICanKnockback knockback))
                {
                    Vector3 direction = (target.transform.position - transform.position).RemoveY().normalized;
                    knockback.Knockback(direction, knockbackForce, dealer);
                }
                
                if (damage > 0 && target.TryGetComponent(out IDamageable damageable))
                {
                    damageable.ApplyDamage(damage, dealer);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, detectRange);
        }
    }
}
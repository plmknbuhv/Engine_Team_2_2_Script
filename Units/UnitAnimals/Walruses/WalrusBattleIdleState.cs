using Code.Entities;
using Code.Units.State.Friendly;
using Enemies;
using UnityEngine;

namespace Code.Units.UnitAnimals.Walruses
{
    public class WalrusBattleIdleState : FriendlyBattleIdleState
    {
        private readonly Walrus _walrus;

        public WalrusBattleIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _walrus = entity as Walrus;
            Debug.Assert(_walrus != null, $"Walrus is null : {entity.gameObject.name}");
        }

        protected override void ChangeState(string stateName)
        {
            if (_walrus.TryGetFreezeEnemy(out Unit target))
            {
                _walrus.StartChaseToBiteTarget(target);
                _walrus.ChangeState("CHASE");
            }
            else
            {
                Debug.Log(target);
                base.ChangeState(stateName);
            }
        }
    }
}
using Code.Entities;
using Code.Units.State;
using UnityEngine;

namespace Code.Units.UnitAnimals.SeaHorses
{
    public class SeaHorseRideWaveState : RideWaveState
    {
        private readonly SeaHorse _seaHorse;
        
        public SeaHorseRideWaveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _seaHorse = entity as SeaHorse;
            Debug.Assert(_seaHorse != null, $"SeaHorse is null : {entity.gameObject.name}");
        }

        public override void Enter()
        {
            base.Enter();

            _seaHorse.CanBuff = false;
        }

        public override void Exit()
        {
            base.Exit();
            
            _seaHorse.CanBuff = true;
        }
    }
}
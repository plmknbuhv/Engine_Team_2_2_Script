using Code.Entities;
using Code.Units.State;

namespace Code.Units.UnitAnimals.Giraffes
{
    public class GiraffeDeadState : UnitDeadState
    {
        private Giraffe _giraffe;
        
        public GiraffeDeadState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _giraffe = _unit as Giraffe;
        }

        public override void Enter()
        {
            base.Enter();
            _giraffe.GetOffMonkey();
            
            const string spinEffect = "GiraffeSpinEffect";
            
            _giraffe.EntityVFX.StopVFX(spinEffect);
        }
    }
}
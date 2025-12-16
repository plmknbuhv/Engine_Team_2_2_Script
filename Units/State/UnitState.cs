using Code.Entities;
using Code.Entities.FSM;
using Code.Units.Movements;

namespace Code.Units.State
{
    public abstract class UnitState : EntityState
    {
        protected Unit _unit;
        protected Movement _movement;

        protected UnitState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _unit = entity as Unit;
            _movement = entity.GetCompo<Movement>();
        }
    }
}
using Code.Entities;
using Code.Units.UnitAnimals.Monkeys;
using UnityEngine;

namespace Code.Units.UnitAnimals.Giraffes
{
    public class Giraffe : FriendlyUnit
    {
        [SerializeField] private Transform spinParent;

        public bool IsAlreadySpin { get; private set; } 
        public EntityVFX EntityVFX { get; private set; } 
        public Monkey Monkey { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            EntityVFX = GetCompo<EntityVFX>();
        }

        public void TakeGiraffe(Monkey monkey)
        {
            IsAlreadySpin = true;
            monkey.transform.SetParent(spinParent);
            Monkey = monkey;
            ChangeState("SPIN");
        }

        public void GetOffMonkey()
        {
            if (Monkey != null)
            {
                Monkey.transform.SetParent(null);
                Monkey.ComeDown();
                Monkey = null;
            }
        }

        public override void ResetItem()
        {
            base.ResetItem();
            Monkey = null;
            IsAlreadySpin = false;
        }
    }
}
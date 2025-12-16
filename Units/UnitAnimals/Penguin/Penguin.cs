using UnityEngine;
using UnityEngine.Events;

namespace Code.Units.UnitAnimals.Penguin
{
    public class Penguin : FriendlyUnit
    {
        public UnityEvent OnSwimStart;
        public UnityEvent OnSwimAttack;
        public bool IsSwimEnd { get; set; }

        public override void ResetItem()
        {
            base.ResetItem();
            
            IsSwimEnd = false;
        }
    }
}
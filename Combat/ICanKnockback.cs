using Code.Entities;
using UnityEngine;

namespace Code.Combat
{
    public interface ICanKnockback
    {
        public void Knockback(Vector3 direction, float force, Entity dealer);
    }
}
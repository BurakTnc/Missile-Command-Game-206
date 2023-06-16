using UnityEngine;

namespace _Root.Scripts.Interfaces
{
    public interface ILaunchable
    {
        public void Fire(Vector3 targetPosition);
        public void Explode(bool hasDamaged);
    }
}
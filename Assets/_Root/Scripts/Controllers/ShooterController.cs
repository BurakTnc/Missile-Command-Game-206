using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class ShooterController : MonoBehaviour
    {
        [SerializeField] private LauncherController[] launchers;

        private int _shootingTurn;
        
        public void Fire()
        {
            launchers[_shootingTurn].Fire();
            _shootingTurn++;
            if (_shootingTurn == launchers.Length)
            {
                _shootingTurn = 0;
            }
        }
    }
}
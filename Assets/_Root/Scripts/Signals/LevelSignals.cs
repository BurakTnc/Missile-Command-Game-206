using UnityEngine;
using UnityEngine.Events;

namespace _Root.Scripts.Signals
{
    public class LevelSignals : MonoBehaviour
    {
        public static LevelSignals Instance;

        public UnityAction IncreaseDifficulty = delegate { };
    }
}
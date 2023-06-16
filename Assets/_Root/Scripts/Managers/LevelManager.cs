using _Root.Scripts.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Root.Scripts.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        public int levelID;

        private int _difficulty;

        private void Awake()
        {
            #region Singleton

            if (Instance != this && Instance != null) 
            {
                Destroy(this);
                return;
            }

            Instance = this;

            #endregion
            
            GetValues();
        }
        
        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        #region Subscribtons

        private void Subscribe()
        {
            CoreGameSignals.Instance.OnSave += Save;
            CoreGameSignals.Instance.OnLevelWin += LevelWin;
        }
        
        private void UnSubscribe()
        {
            CoreGameSignals.Instance.OnSave -= Save;
            CoreGameSignals.Instance.OnLevelWin -= LevelWin;
        }

        #endregion
        
        private void GetValues()
        {
            levelID = PlayerPrefs.GetInt("levelID", 1);
            _difficulty = PlayerPrefs.GetInt("difficulty", 0);
        }

        private void LevelWin()
        {
            levelID++;
            _difficulty++;
            if (_difficulty >= 5) 
            {
                //LevelSignals.Instance.IncreaseDifficulty?.Invoke();
                _difficulty = 0;
            }
            CoreGameSignals.Instance.OnSave?.Invoke();
        }

        private void Save()
        {
            PlayerPrefs.SetInt("levelID",levelID);
            PlayerPrefs.SetInt("difficulty",_difficulty);
        }
        
    }
}
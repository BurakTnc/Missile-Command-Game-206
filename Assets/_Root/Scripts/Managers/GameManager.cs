using System;
using _Root.Scripts.Signals;
using UnityEngine;

namespace _Root.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public int money;
        public int destroyedBuildingCount;
        public int targetMissileCount;

        private bool _isRunning;
        private float _timer = 60;

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

        #region Subscribtions
        private void OnEnable()
        {
            Subscribe();
        }

        private void Update()
        {
            if(!_isRunning)
                return;

            _timer -= Time.deltaTime;
            _timer = Mathf.Clamp(_timer, 0, 60);
            if (_timer <= 0) 
            {
                CoreGameSignals.Instance.OnLevelWin?.Invoke();
            }
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void Subscribe()
        {
            CoreGameSignals.Instance.OnSave += Save;
            CoreGameSignals.Instance.OnGameStart += OnGameStart;

        }

        private void UnSubscribe()
        {
            CoreGameSignals.Instance.OnSave -= Save;
            CoreGameSignals.Instance.OnGameStart -= OnGameStart;
        }

        #endregion

        private void OnGameStart()
        {
            _timer = 60;
            _isRunning = true;
        }
        private void GetValues()
        {
            money = PlayerPrefs.GetInt("money", 0);
        }

        private void Save()
        {
            PlayerPrefs.SetInt("money",money);
        }

        public void ArrangeMoney(int value)
        {
            money += value;
        }

        public void OnBuildingDestroy()
        {
            destroyedBuildingCount++;
            if (destroyedBuildingCount>=5)
            {
                CoreGameSignals.Instance.OnLevelFail?.Invoke();
            }
            CoreGameSignals.Instance.OnSave?.Invoke();
        }

        public void OnMissileDestroyed()
        {
            CoreGameSignals.Instance.OnSave?.Invoke();
            targetMissileCount++;
            // if (targetMissileCount>=20)
            // {
            //     CoreGameSignals.Instance.OnLevelWin?.Invoke();
            // }
        }
        public int GetMoney()
        {
            return money < 0 ? 0 : money;
        }
    }
}
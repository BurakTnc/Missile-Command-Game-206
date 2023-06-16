using System;
using System.Collections.Generic;
using _Root.Scripts.Interfaces;
using _Root.Scripts.Signals;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Root.Scripts.Controllers
{
    public class EnemyLauncherController : MonoBehaviour
    {
        [SerializeField] private float defaultLaunchCooldown;
        [SerializeField] private float launchingHeight;
        [SerializeField] private float spawnAreaLength;
        
        [SerializeField] private List<Transform> targetList = new List<Transform>();

        private float _targetPositionLength;
        private float _launchTimer;
        private bool _isRunning;

        private void Awake()
        {
            LaunchCooldown = defaultLaunchCooldown;
            _targetPositionLength = spawnAreaLength - 5;
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void Subscribe()
        {
            CoreGameSignals.Instance.OnLevelFail += StopLaunching;
            CoreGameSignals.Instance.OnLevelWin += StopLaunching;
            CoreGameSignals.Instance.OnGameStart += ContinueLaunching;
        }

        private void UnSubscribe()
        {
            CoreGameSignals.Instance.OnLevelFail -= StopLaunching;
            CoreGameSignals.Instance.OnLevelWin -= StopLaunching;
            CoreGameSignals.Instance.OnGameStart -= ContinueLaunching;
        }

        private void Update()
        {
            BeginLaunching();
        }

        private void StopLaunching()
        {
            _isRunning = false;
        }

        private void ContinueLaunching()
        {
            _isRunning = true;
        }

        public float LaunchCooldown { get; set; }


        private void BeginLaunching()
        {
            if(!_isRunning)
                return;
            
            if (_launchTimer <= 0)
            {
                _launchTimer += Random.Range(0.5f,LaunchCooldown);
                var pos = Random.Range(-spawnAreaLength, spawnAreaLength + .1f);
                LaunchMissile(pos);
            }
            else
            {
                _launchTimer -= Time.deltaTime;
                _launchTimer = Mathf.Clamp(_launchTimer, 0, LaunchCooldown);
            }
                
        }
        public void LaunchMissile(float xPos)
        {
            var selectedMissile = Random.Range(1, 5);
            var missile = Instantiate(Resources.Load<GameObject>($"Spawnables/EnemyRocket{selectedMissile}")).transform;
            var launchPos = new Vector3(xPos, launchingHeight, 461);
            var selectedTarget = Random.Range(-_targetPositionLength, _targetPositionLength+1);

            missile.position = launchPos;
            var target = new Vector3(selectedTarget, -55, missile.position.z);
            missile.transform.LookAt(target,Vector3.forward);
            
            if (missile.TryGetComponent(out ILaunchable launchedMissile))
            {
                launchedMissile.Fire(target);
            }
            
        }


    }
}
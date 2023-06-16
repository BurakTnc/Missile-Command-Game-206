using System;
using _Root.Scripts.Interfaces;
using _Root.Scripts.Objects;
using _Root.Scripts.Signals;
using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class LauncherController : MonoBehaviour
    {
        [SerializeField] private Transform launcher;
        [SerializeField] private Transform firePosition;
        [SerializeField] private Transform linePosition;

        private Transform _aimedPosition;
        private Transform _launchersReference;
        private LineRenderer _lineRenderer;
        private bool _isRunning;

        private void Awake()
        {
            _aimedPosition = GameObject.Find("Aim Position").transform;
            _launchersReference = GameObject.Find("Launchers Reference").transform;
            _lineRenderer = GetComponent<LineRenderer>();
        }
        

        private void Update()
        {
            AimToTarget();
            SetLinePositions();
        }

        private void AimToTarget()
        {
            if(!_aimedPosition)
                return;
            launcher.transform.LookAt(_launchersReference,Vector3.up);
        }

        private void SetLinePositions()
        {
            _lineRenderer.SetPosition(0,linePosition.position);
            _lineRenderer.SetPosition(1,_launchersReference.position);
        }
        public void Fire()
        {
            var missile = Instantiate(Resources.Load<GameObject>("Spawnables/Player Rocket"));

            missile.transform.position = firePosition.position;

            if (missile.TryGetComponent(out ILaunchable launchedMissile))
            {
                launchedMissile.Fire(_aimedPosition.position);
            }
        }
    }
}

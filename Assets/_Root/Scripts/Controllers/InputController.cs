using System;
using _Root.Scripts.Signals;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace _Root.Scripts.Controllers
{
    public class InputController : MonoBehaviour
    {
      [HideInInspector] public Vector3 aimedPosition;
       
       [SerializeField] private GameObject crossHair;
       [SerializeField] private GameObject launchersReference;
       [SerializeField] private Transform targetPosition;

       private ShooterController _shooter;
       private Camera _camera;
       private bool _isRunning;


       private void Awake()
       {
           _shooter = GetComponent<ShooterController>();
           _camera = Camera.main;
       }

       private void OnEnable()
       {
           CoreGameSignals.Instance.OnLevelFail += StopLaunching;
           CoreGameSignals.Instance.OnLevelWin += StopLaunching;
           CoreGameSignals.Instance.OnGameStart += ContinueLaunching;
       }

       private void OnDisable()
       {
           CoreGameSignals.Instance.OnLevelFail -= StopLaunching;
           CoreGameSignals.Instance.OnLevelWin -= StopLaunching;
           CoreGameSignals.Instance.OnGameStart -= ContinueLaunching;
       }

       private void StopLaunching()
       {
           _isRunning = false;
           Cursor.visible = true;
       }

       private void ContinueLaunching()
       {
           _isRunning = true;
           Cursor.visible = false;
       }
       private void Fire()
       {
           if (Input.GetMouseButtonDown(0))
           {
               _shooter.Fire();
           }
       }
       private void Aim()
       {
           aimedPosition = _camera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, Mathf.Abs(
               _camera.transform.position.z)));
   
           crossHair.transform.position = aimedPosition;
           launchersReference.transform.position =
               new Vector3(aimedPosition.x * 21f, aimedPosition.y * 21f, launchersReference.transform.position.z);
           
           var lockPosition = new Vector3(aimedPosition.x * 13.5f, aimedPosition.y * 13.5f, targetPosition.position.z);
           targetPosition.position = lockPosition;
       }

       private void Update()
       {
           if(!_isRunning)
               return;
           
           Aim();
           Fire();
       }
    }
}
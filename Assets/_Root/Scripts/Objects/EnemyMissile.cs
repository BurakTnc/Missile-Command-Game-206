using System;
using _Root.Scripts.Controllers;
using _Root.Scripts.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace _Root.Scripts.Objects
{
    public class EnemyMissile : MonoBehaviour, ILaunchable
    {
        [SerializeField] private int damage;
        [SerializeField] private float launchSpeed;
        [SerializeField] private GameObject explosionFx;
        public void Fire(Vector3 targetPosition)
        {
            var duration = Vector3.Distance(transform.position, targetPosition) / launchSpeed;
            transform.DOMove(targetPosition, duration).SetEase(Ease.InSine).OnComplete(() => Explode());
        }

        public void Explode(bool hasDamaged=false)
        {
            transform.DOKill();
            explosionFx.transform.SetParent(null);
            explosionFx.SetActive(true);
            
            if (hasDamaged)
            {
                GiveDamage();
            }

            Destroy(gameObject);
        }

        private void GiveDamage()
        {
            
        }

        private void DestroyProps(Transform prop,Vector3 impactPosition)
        {
            if (prop.TryGetComponent(out BuildingController buildingController))
            {
                buildingController.Demolish(impactPosition, damage);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Prop"))
            {
                Explode(true);
                DestroyProps(other.transform,transform.position);
            }
        }
    }
}
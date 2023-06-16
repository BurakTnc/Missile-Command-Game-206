using System;
using _Root.Scripts.Interfaces;
using _Root.Scripts.Managers;
using DG.Tweening;
using UnityEngine;

namespace _Root.Scripts.Objects
{
    public class Missile : MonoBehaviour, ILaunchable
    {
    [SerializeField] private float explosionRadius;
    [SerializeField] private float launchSpeed;
    [SerializeField] private LayerMask enemyMissileLayers;
    [SerializeField] private GameObject explosionFx;

    private bool _isExploding;

    private void Update()
    {
        if(!_isExploding)
            return;
        ScanTheMissiles();
    }

    private void ScanTheMissiles()
    {
        var detectedColliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyMissileLayers);

        if (detectedColliders.Length <= 0)
            return;
        
        foreach (var enemy in detectedColliders)
        {
            if (enemy.gameObject.TryGetComponent(out ILaunchable missile))
            {
                missile.Explode(false);
                GameManager.Instance.money += 5;
                GameManager.Instance.OnMissileDestroyed();
            }
        }
    }

    public void Explode(bool hasDamaged=false)
    {
        _isExploding = true;
        transform.DOKill();
        explosionFx.transform.SetParent(null);
        explosionFx.SetActive(true);
        Destroy(gameObject,2f);
    }

    public void Fire(Vector3 targetPosition)
    {
        var duration = Vector3.Distance(transform.position, targetPosition) / launchSpeed;
        transform.DOMove(targetPosition, duration).SetEase(Ease.InSine).OnComplete(() => Explode());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
    }
}
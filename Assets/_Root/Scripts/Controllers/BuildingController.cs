using System.Collections.Generic;
using _Root.Scripts.Managers;
using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class BuildingController : MonoBehaviour
    {
        [SerializeField] private float demolishForce;
        [SerializeField] private float explosionForce;

        private bool _hasExploded;
        private float _health = 100;
        private readonly List<GameObject> _demolishedList = new List<GameObject>();

        public void Demolish(Vector3 impactPosition,int takenDamage)
        {
            ShakeManager.Instance.ShakeCamera(true);
            
            var pieceCount = transform.childCount;
            var demolishedPieces = Random.Range(1,7 );

            if (pieceCount > 6)
            {
                for (var i = 0; i < demolishedPieces; i++)
                {
                    _demolishedList.Add(transform.GetChild(i).gameObject);
                }

                foreach (var piece in _demolishedList)
                {
                    piece.transform.SetParent(null);
                    if (piece.TryGetComponent(out Rigidbody rb))
                    {
                        rb.isKinematic = false;
                        rb.AddExplosionForce(demolishForce, impactPosition, 50, -.1f, ForceMode.Impulse);
                    }

                    if (piece.TryGetComponent(out BoxCollider boxCollider))
                    {
                        boxCollider.enabled = true;
                    }
                }
                _demolishedList.Clear();
            }

            _health -= takenDamage;
            
            if(_health>0)
                return;
            
            Explode(impactPosition);


        }

        private void Explode(Vector3 impactPosition)
        {
            if(_hasExploded)
                return;
            
            _hasExploded = true;
            ShakeManager.Instance.ShakeCamera(false);
            GameManager.Instance.OnBuildingDestroy();
            var pieceCount = transform.childCount;

            if (pieceCount <= 0)
                return;
            
            for (var i = 0; i < pieceCount; i++)
            {
                _demolishedList.Add(transform.GetChild(i).gameObject);
            }

            foreach (var piece in _demolishedList)
            {
                piece.transform.SetParent(null);
                if (piece.TryGetComponent(out Rigidbody rb))
                {
                    rb.isKinematic = false;
                    rb.AddExplosionForce(explosionForce, impactPosition, 3, .01f, ForceMode.VelocityChange);
                }

                if (piece.TryGetComponent(out BoxCollider boxCollider))
                {
                    boxCollider.enabled = true;
                }
                    
            }
            _demolishedList.Clear();
        }
    }
}
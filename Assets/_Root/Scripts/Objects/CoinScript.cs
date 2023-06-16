using System.Collections;
using _Root.Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Objects
{
    [RequireComponent(typeof(AudioSource))]
    public class CoinScript : MonoBehaviour
    {
        private RectTransform _target;
        private RectTransform _rectTransform;
        private AudioSource _source;
        private bool _hapticSupported;

        [SerializeField] private int earnValue;

        private void Awake()
        {
            _target = GameObject.Find("CoinImage").GetComponent<RectTransform>();
            _rectTransform = GetComponent<RectTransform>();
            _source = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _rectTransform.pivot = _target.pivot;
            var x = Random.Range(-500, 0);
            var y = Random.Range(-1300, -601);
            _rectTransform.anchoredPosition = new Vector3(x, y, 0);
            StartCoroutine(Latency());
        }

        private IEnumerator Latency()
        {
            var r = Random.Range(.2f, .5f);
            yield return new WaitForSeconds(r);
            MoveToTarget(Random.Range(.5f, 1f));

        }

        private void MoveToTarget(float time)
        {
            _rectTransform.DOAnchorPos(_target.anchoredPosition, time).SetEase(Ease.InBack);
            _rectTransform.DOScale(Vector2.one, time).OnComplete(() => SetMoney());
        }

        private void SetMoney()
        {
            _source.Play();
            GameManager.Instance.ArrangeMoney(earnValue);
            Destroy(gameObject,1);
        }
    }
}
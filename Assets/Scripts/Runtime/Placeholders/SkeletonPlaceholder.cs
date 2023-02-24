using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Obert.UI.Runtime.Placeholders
{
    public sealed class SkeletonPlaceholder : MonoBehaviour
    {
        private Coroutine _currentCoroutine;

        [SerializeField] private UnityEvent<bool> isLoading;

        private void Awake()
        {
            isLoading?.Invoke(false);
        }

        
        public void AsyncAction(IEnumerator action, Action onComplete)
        {
            CancelCurrentCoroutine();
            _currentCoroutine = StartCoroutine(LoadCoroutine(action, onComplete));
        }

        private void CancelCurrentCoroutine()
        {
            if (_isLoading) SetIsLoading(false);
            if (_currentCoroutine == null) return;
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }

        private void OnDestroy()
        {
            CancelCurrentCoroutine();
        }

        private bool _isLoading;

        private IEnumerator LoadCoroutine(IEnumerator action, Action onComplete)
        {
            SetIsLoading(true);
            isLoading.Invoke(_isLoading);
            yield return action;
            SetIsLoading(false);
            onComplete?.Invoke();
        }

        private void SetIsLoading(bool value)
        {
            _isLoading = value;
            isLoading.Invoke(_isLoading);
        }
    }
}
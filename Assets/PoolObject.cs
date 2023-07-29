using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DefaultNamespace
{
    public class PoolObject : MonoBehaviour
    {
        public float Speed = 5f;

        private bool _isGamePaused = false;

        private void OnEnable()
        {
            GameManager.GameEnded      += OnGameEnded;
            GameManager.GamePaused     += OnGamePaused;
            GameManager.GameResumed    += OnGameResumed;
            GameManager.ReturnedToMenu += OnGameEnded;
            Invoke(nameof(AddBacktoPoolDelayed), 5f);
        }

        private void OnDisable()
        {
            GameManager.GameEnded      -= OnGameEnded;
            GameManager.GamePaused     -= OnGamePaused;
            GameManager.GameResumed    -= OnGameResumed;
            GameManager.ReturnedToMenu -= OnGameEnded;
        }

        private void OnGamePaused()
        {
            _isGamePaused = true;
        }

        private void OnGameResumed()
        {
            _isGamePaused = false;
        }

        private void AddBacktoPoolDelayed()
        {
            OnGameEnded();
        }

        private void OnGameEnded()
        {
            Pooler.AddBackToPool?.Invoke(this);
        }

        private void FixedUpdate()
        {
            if (_isGamePaused) return;
            transform.position += Vector3.down * (Time.deltaTime * Speed);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("ScoreBorder"))
            {
                GameManager.IncrementScore?.Invoke();
            }

            else if (col.CompareTag("Ball"))
            {
                GameManager.GameEnded?.Invoke();
            }
        }
    }
}
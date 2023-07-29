using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public BallController BallController;
        public Pooler         Pooler;

        public GameObject MainMenu, Settings, Statistics, InGameUI, GameOverSprite, GameOver;

        public TMP_Text ScoreText1, BestScoreText1, ScoreText2, BestScoreText2;

        public float SpawnedObjectsIncreaseAfter = 5f;
        public int   InitialSpawnedObjects       = 3;
        public float SpawnInterval               = 2f;

        public        List<float> speedPerDifficulty = new();
        public static Action      GameEnded;
        public static Action      IncrementScore;
        public static Action      GamePaused;
        public static Action      GameResumed;
        public static Action      ReturnedToMenu;

        private int _selectedDifficulty = 0;

        private Coroutine _gameRoutine, _spawnIncreaseRoutine;

        private int _score     = 0;
        private int _highScore = 0;

        private void OnEnable()
        {
            IncrementScore += (() =>
            {
                _score          += 1;
                ScoreText1.text =  _score.ToString();
                ScoreText2.text =  _score.ToString();
            });

            GameEnded += (() =>
            {
                PauseGame();
                Invoke(nameof(EndGame), 1f);
                GameOverSprite.SetActive(true);
                if (_score <= _highScore) return;
                _highScore          = _score;
                BestScoreText1.text = _highScore.ToString();
                BestScoreText2.text = _highScore.ToString();
                
            });
        }

        public void StartGame()
        {
            _score          = 0;
            ScoreText1.text = _score.ToString();
            ScoreText2.text = _score.ToString();
            GameOver.SetActive(false);
            MainMenu.SetActive(false);
            InGameUI.SetActive(true);
            GameOverSprite.SetActive(false);
            BallController.gameObject.SetActive(true);
            BallController.IsGameStarted = true;
            _gameRoutine                 = StartCoroutine(GameRoutine());
            _spawnIncreaseRoutine        = StartCoroutine(SpawnCountIncreaseRoutine());
            InitialSpawnedObjects        = 3;
        }

        private IEnumerator GameRoutine()
        {
            while (true)
            {
                for (int i = 0; i < InitialSpawnedObjects; i++)
                {
                    PoolObject obj = Pooler.GetFromPool();
                    if (!obj) continue;
                    obj.Speed = speedPerDifficulty[_selectedDifficulty];
                    obj.gameObject.SetActive(true);

                    yield return new WaitForSeconds(0.1f);
                }

                yield return new WaitForSeconds(SpawnInterval);
            }
        }

        private IEnumerator SpawnCountIncreaseRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(SpawnedObjectsIncreaseAfter);
                InitialSpawnedObjects += 2;
            }
        }

        private void Start()
        {
            Pooler.GeneratePool();
            ShowMenu();
        }

        public void EndGame()
        {
            GameOver.SetActive(true);
            InGameUI.SetActive(false);
            BallController.gameObject.SetActive(false);
        }

        public void PauseGame()
        {
            StopCoroutine(_gameRoutine);
            StopCoroutine(_spawnIncreaseRoutine);
            BallController.IsGameStarted = false;
            GamePaused?.Invoke();
        }

        public void ResumeGame()
        {
            BallController.IsGameStarted = true;
            _gameRoutine                 = StartCoroutine(GameRoutine());
            _spawnIncreaseRoutine        = StartCoroutine(SpawnCountIncreaseRoutine());
            GameResumed?.Invoke();
        }


        public void ShowMenu()
        {
            BallController.IsGameStarted = false;
            BallController.gameObject.SetActive(false);
            InGameUI.SetActive(false);
            MainMenu.SetActive(true);
            GameOver.SetActive(false);
        }

        public void ShowStatistics()
        {
            MainMenu.SetActive(false);
            Statistics.SetActive(true);
        }

        public void HideStatistics()
        {
            MainMenu.SetActive(true);
            Statistics.SetActive(false);
        }

        public void ShowSettings()
        {
            MainMenu.SetActive(false);
            Settings.SetActive(true);
        }

        public void HideSettings()
        {
            MainMenu.SetActive(true);
            Settings.SetActive(false);
        }

        public void SetDifficulty(int difficulty)
        {
            _selectedDifficulty = difficulty;
        }

        public void BackToHome()
        {
            BallController.gameObject.SetActive(false);
            InGameUI.SetActive(false);
            StopCoroutine(_gameRoutine);
            StopCoroutine(_spawnIncreaseRoutine);
            ReturnedToMenu?.Invoke();
            MainMenu.SetActive(true);
        }
    }
}
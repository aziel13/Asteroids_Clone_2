using System;
using DefaultNamespace.Utility;
using UnityEngine;
using Unity.Cinemachine;

namespace DefaultNamespace
{
    public class GameManager : Monobehaviour_Singleton<GameManager>
    {
        private static int _levelNumber = 1;
        
        private static int finalScore = 0;

        public static int levelNumber => _levelNumber;

        public static int finalScore1 => finalScore;
        
        [SerializeField] private Camera _mainCamera;
        
        public Camera mainCamera => _mainCamera;

        [SerializeField] private CinemachineCamera cinemachineCamera;
        private bool isGameRunning = true;

        private bool unPauseButtonPressed = false;

        private int _score;
        private int _lives;
        
        private float _time = 0;
        private bool isTimerActive;

        public event EventHandler<OnGameRunningStateChangeEventArgs> OnGameRunningStateChange;

        public class OnGameRunningStateChangeEventArgs : EventArgs
        {
            public bool isGameRunning;
        }

        public event EventHandler<OnStatsChangeEventArgs> OnStatsChange;

        public class OnStatsChangeEventArgs : EventArgs
        {
            public int level;
            public int score;
            public int lives;
        }

        private void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            
            
            AsteroidManager.Instance.OnAllAsteroidsCleared += AsteroidManager_OnLevelCleared;

            Starship.Instance.OnGameStateChange += Starship_OnGameStateChange;


            AsteroidManager.Instance.OnDestroyAsteroid += AsteroidManager_OnDestroyAsteroid;
        }

        private void AsteroidManager_OnDestroyAsteroid(object sender, AsteroidManager.OnDestroyAsteroidEventArgs e)
        {
            
            
            
            
        }

        private void Starship_OnGameStateChange(object sender, Starship.OnGameStateChangeEventArgs e)
        {

        }

        private void AsteroidManager_OnLevelCleared(object sender, AsteroidManager.OnAllAsteroidsClearedEventArgs e)
        {

        }

        private void LoadCurrentLevel()
        {



        }

        private void Update()
        {
            if (isGameRunning)
            {
                _time += Time.deltaTime;
            }

            if (!isGameRunning)
            {
                if (GameInput.Instance.IsPausedActionPressed() && !unPauseButtonPressed)
                {
                    Instance.SetGameRunningState();
                    unPauseButtonPressed = true;
                }

                if (!GameInput.Instance.IsPausedActionPressed())
                {
                    unPauseButtonPressed = false;
                }

            }
        }

        private void FixedUpdate()
        {
            if (isTimerActive)
            {
                if (GameInput.Instance.IsPausedActionPressed() && !unPauseButtonPressed)
                {
                    Instance.SetGameRunningState();
                    unPauseButtonPressed = true;
                }

                if (!GameInput.Instance.IsPausedActionPressed())
                {
                    unPauseButtonPressed = false;
                }

            }
        }

        public void SetGameRunningState()
        {

            if (!isGameRunning)
            {
                isGameRunning = true;
                Time.timeScale = 1f;

            }
            else
            {
                isGameRunning = false;
                Time.timeScale = 0f;
            }

            OnGameRunningStateChange?.Invoke(this,
                new OnGameRunningStateChangeEventArgs
                {
                    isGameRunning = Instance.isGameRunning,
                });
        }

        public void ResetStaticGameValues()
        {

            Time.timeScale = 1f;
            _levelNumber = 1;
            finalScore = 0;

        }

        public int GetLevelNumber()
        {
            return _levelNumber;
        }

        public void AsteroidDestroyed()
        {
            
        }

        public void Death()
        {
            
        }

    }
}
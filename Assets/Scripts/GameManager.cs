using System;
using System.Collections.Generic;
using DefaultNamespace.Utility;
using UnityEngine;
using Unity.Cinemachine;
using Unity.Mathematics;

namespace DefaultNamespace
{
    public class GameManager : Monobehaviour_Singleton<GameManager>
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private GameObject mainPlayerSpawnPoint;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private float sphereCastRadius = 1f;
        private static int _levelNumber = 1;
        
        private static int finalScore = 0;

        public static int levelNumber => _levelNumber;

        public static int finalScore1 => finalScore;
        
        public Camera mainCamera => _mainCamera;
        
        private bool isGameRunning = true;
        
        private Starship.GameState _gameState;
        
        private bool unPauseButtonPressed = false;

        private int _score = 0;
        private int _lives = 3;
        
        private float _time = 0;
        private bool isTimerActive;

        private float playerPrefabMaxDistance = 0;
       
        
        public event EventHandler<OnPlayerSpawnEventArgs> OnPlayerSpawn;
        public class OnPlayerSpawnEventArgs : EventArgs
        {
            
            public GameObject playerGameObject;
            
        }
        public event EventHandler<OnStateChangeEventArgs> OnStateChange;

        public class OnStateChangeEventArgs : EventArgs
        {
            
            public Starship.GameState gameState;
            
        }

        public event EventHandler<OnStatsChangeEventArgs> OnStatsChange;

        public class OnStatsChangeEventArgs : EventArgs
        {
            public int level;
            public int score;
            public int lives;
        }
        
        public event EventHandler<OnLevelChangeEventArgs> OnLevelChange;

        public class OnLevelChangeEventArgs : EventArgs
        {
            public int level;
        }

        private void Awake()
        {
            base.Awake();
            _gameState = Starship.GameState.Startup;
            
        }

        private void Start()
        {
            
            AsteroidManager.Instance.OnAllAsteroidsCleared += AsteroidManager_OnLevelCleared;

           

            AsteroidManager.Instance.OnDestroyAsteroid += AsteroidManager_OnDestroyAsteroid;

            if (playerPrefab)
            {
                if (playerPrefab.TryGetComponent<PolygonCollider2D>(out PolygonCollider2D polygonCollider2D))
                {
                    List<Vector2> points = new List<Vector2>();
                    
                    
                    // if the collider has more than 2 points find the largest distance between each point
                    
                    for ( int i = 1 ; i < polygonCollider2D.pathCount ; i++ ) {
                
                        Vector2[] pathPoints = polygonCollider2D.GetPath(i);
                    
                        points.AddRange(pathPoints);
                        
                    }
                    
                    List<Vector2> worldPoints = new List<Vector2>();
                    foreach (Vector2 point in points)
                    {
                        worldPoints.Add(polygonCollider2D.transform.TransformPoint(point));
                    }
                    
                    for ( int i = 0 ; i < worldPoints.Count ; i++ ) {
                        for ( int j = i + 1 ; j < worldPoints.Count ; j++ ) {   
                            float distance = Vector2.Distance(worldPoints[i], worldPoints[j]);
                            if (distance > playerPrefabMaxDistance)
                            {
                                playerPrefabMaxDistance = distance;
                            }
                        }
                      
                    }
                }
            }

        }

        private void Starship_OnCrash(object sender, Starship.OnCrashEventArgs e)
        {
            
            if (_lives > 0)
            {
                
                _lives--;

                OnStatsChange?.Invoke(this,
                new OnStatsChangeEventArgs
                {
                    level = _levelNumber,
                    score = _score,
                    lives = _lives,
                });

                OnStateChange?.Invoke(this,
                    new OnStateChangeEventArgs
                    {
                        gameState = Starship.GameState.Respawning,   
                    });                
            }
            else
            {
                OnStateChange?.Invoke(this,
                    new OnStateChangeEventArgs
                    {
                        gameState = Starship.GameState.GameOver,   
                    });        
            }
        }

        private void AsteroidManager_OnDestroyAsteroid(object sender, AsteroidManager.OnDestroyAsteroidEventArgs e)
        {
            Asteroid asteroidScript = e.asteroidToDestroyGameObject.GetComponent<Asteroid>();


            _score += asteroidScript.getScoreValue;
            
            OnStatsChange?.Invoke(this,
                new OnStatsChangeEventArgs
                {
                    level = _levelNumber,
                    score = _score,
                    lives = _lives,
                });
            
        }

        private void Starship_OnGameStateChange(object sender, Starship.OnGameStateChangeEventArgs e)
        {
            if (_gameState == Starship.GameState.Startup && e.gameState == Starship.GameState.GameRunning)
            {
                
                spawnPlayer();
                
                // update the ui values
                OnStatsChange?.Invoke(this,
                    new OnStatsChangeEventArgs
                    {
                        level = _levelNumber,
                        score = _score,
                        lives = _lives,
                    });
                
            } else if (_gameState == Starship.GameState.Respawning && e.gameState == Starship.GameState.GameRunning)
            {

                spawnPlayer();

            }
        }

        private void AsteroidManager_OnLevelCleared(object sender, AsteroidManager.OnAllAsteroidsClearedEventArgs e)
        {
            //reset the players position.
            
            // increase the level
            _levelNumber++;
            
            OnLevelChange?.Invoke(this,
                new OnLevelChangeEventArgs
                {
                    level = _levelNumber, 
                });
            
        }

        private void spawnPlayer()
        {

            if (_gameState == Starship.GameState.Startup)
            {
                GameObject player = Instantiate(playerPrefab,mainPlayerSpawnPoint.transform.position, quaternion.identity);
                OnPlayerSpawn?.Invoke(this,
                    new OnPlayerSpawnEventArgs
                    {
                        playerGameObject = player, 
                    });
                Starship.Instance.OnCrash += Starship_OnCrash;
                Starship.Instance.OnGameStateChange += Starship_OnGameStateChange;


            }
            else if(_gameState == Starship.GameState.Respawning)
            {

                //check if there is an asteroid at the main spawn point.
                var rayHit = Physics2D.CircleCast(mainPlayerSpawnPoint.transform.position, playerPrefabMaxDistance + sphereCastRadius, Vector2.zero);
                GameObject player = Instantiate(playerPrefab,mainPlayerSpawnPoint.transform.position, quaternion.identity);
                OnPlayerSpawn?.Invoke(this,
                    new OnPlayerSpawnEventArgs
                    {
                        playerGameObject = player, 
                    });

                if (!rayHit.collider) 
                {
                    // if circle cast  hit an asteroid give the player temporary immunity to damage
                    
                } 
                Starship.Instance.OnCrash += Starship_OnCrash;
                Starship.Instance.OnGameStateChange += Starship_OnGameStateChange;

            }

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
                _gameState = Starship.GameState.GameRunning;

            }
            else
            {
                isGameRunning = false;
                Time.timeScale = 0f;
                _gameState = Starship.GameState.Paused;
            }

            OnStateChange?.Invoke(this,
                new OnStateChangeEventArgs
                {
                    gameState = _gameState,
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

    }
}
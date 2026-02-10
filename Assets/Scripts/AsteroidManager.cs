using System;
using System.Collections.Generic;
using DefaultNamespace.Utility;
using UnityEngine;
using Random = UnityEngine.Random;


namespace DefaultNamespace
{
    public class AsteroidManager : Monobehaviour_Singleton<AsteroidManager>

    {
        [SerializeField] private GameObject[] AsteroidSpawners;
        
        [SerializeField] private GameObject LargeAsteroidPrefab;
        [SerializeField] protected GameObject[] asteroidShardPrefabs;
        
        public event EventHandler<OnAllAsteroidsClearedEventArgs> OnAllAsteroidsCleared;
        
        public class OnAllAsteroidsClearedEventArgs : EventArgs
        {
            public int shatteredAsteroidPeicesDestroyedCount;
            public int largeAsteroidDestroyedCount;
        }
        
        public event EventHandler<OnDestroyAsteroidEventArgs> OnDestroyAsteroid;
        
        public class OnDestroyAsteroidEventArgs : EventArgs
        {
           public GameObject asteroidToDestroyGameObject;
        }

        
        

        private int LargeAsteroidCount = 0;
        private int ShatteredAsteroidCount = 0;

        private List<Asteroid> asteroids;



        public int largeAsteroidCount => LargeAsteroidCount;

        public int shatteredAsteroidCount => ShatteredAsteroidCount;

        [SerializeField]private int asteroidLevelIncrementFactor = 2;
        private int asteroidLevelMultiplierFactor = 2;

        private int largeAsteroidDestroyedCount;
        private int shatteredAsteroidDestroyedCount;
        private int _asteroidShardMax = 6;
        private int _asteroidShardMin = 1;



        private bool isGameRunning = false;
        
        private void Awake()
        {
            base.Awake();

        }

        private void Start()
        {
            if (asteroidShardPrefabs == null || asteroidShardPrefabs.Length == 0)
            {
                _asteroidShardMin = 0;
            }

            _asteroidShardMax = asteroidShardPrefabs.Length;
            
           
            GameManager.Instance.OnStateChange += GameManager_OnGameStateChange;
        }
        
        private void GameManager_OnGameStateChange(object sender, GameManager.OnStateChangeEventArgs e)
        {
            if (e.gameState == GameManager.GameState.GameRunning)
            {
                SpawnLargeAsteroids();
            }
        }
       
        private void SpawnLargeAsteroids()
        {
            if (LargeAsteroidCount == 0 && ShatteredAsteroidCount == 0)
            {
                int AsteroidSpawnerIndex = 0;

                isGameRunning = true;
                var levelAsteroidCount = GetAsteroidCountForCurrentLevel();
                for (int i = 0; i < levelAsteroidCount; i++)
                {

                    GameObject gameasteroidSpawnPoint = AsteroidSpawners[AsteroidSpawnerIndex];

                    GameObject largeAsteroid = Instantiate(LargeAsteroidPrefab,
                        gameasteroidSpawnPoint.transform.position, Quaternion.identity);
                    Asteroid largeAsteroid_script = largeAsteroid.GetComponent<Asteroid>();
                    largeAsteroid_script.OnAsteroidDestroyed += Asteroid_OnAsteroidDestroyed;

                    float spawnSpeed = Random.Range(
                        largeAsteroid_script.asteroidSpeed - largeAsteroid_script.asteroidSize1,
                        largeAsteroid_script.asteroidMaxSpeed1 - largeAsteroid_script.asteroidSize1);

                    AsteroidSpawnPoint asteroidSpawnPoint =
                        AsteroidSpawners[AsteroidSpawnerIndex].GetComponent<AsteroidSpawnPoint>();


                    int targetDirection = Random.Range(0, asteroidSpawnPoint.asteroidTargets.Length - 1);

                    Vector3 direction = asteroidSpawnPoint.asteroidTargets[targetDirection].position;

                    Vector2 direction2d = new Vector2(direction.x, direction.y);


                    Rigidbody2D asteroidRigidBody = largeAsteroid.GetComponent<Rigidbody2D>();
                    asteroidRigidBody.AddForce(spawnSpeed * direction2d);

                    AsteroidSpawnerIndex++;

                    if (AsteroidSpawnerIndex == AsteroidSpawners.Length)
                    {
                        AsteroidSpawnerIndex = 0;
                    }

                    
                }
                LargeAsteroidCount+= levelAsteroidCount;
            }
        }

        private void Asteroid_OnAsteroidDestroyed(object sender,  Asteroid.OnAsteroidDestroyedEventArgs  e)
        {
            
            Asteroid asteroidScript = e.asteroidGameObject.GetComponent<Asteroid>();
            
            e.asteroidGameObject.GetComponent<Asteroid>().OnAsteroidDestroyed -= Asteroid_OnAsteroidDestroyed;
            
            if (!asteroidScript.isAsteroidShard1)
            {
                  
                 if (asteroidScript.asteroidShardSpawnPoint1 != null &&  asteroidScript.asteroidShardPrefabObjects1 != null 
                    && asteroidScript.asteroidShardSpawnPoint1.Length ==  asteroidScript.asteroidShardPrefabObjects1.Length)
                 {
                     
                     for (int i = 0; i < asteroidShardPrefabs.Length; i++ )
                     {

                         GameObject shard = asteroidScript.asteroidShardPrefabObjects1[i];
                         
                         Transform asteroidShardSpawnPointPosition = asteroidScript.asteroidShardSpawnPoint1[i].transform;

                         GameObject asteroidShard = Instantiate(shard, asteroidShardSpawnPointPosition.position, Quaternion.identity);
                         Asteroid asteroidShard_script = asteroidShard.GetComponent<Asteroid>();
                     
                         asteroidShard_script.OnAsteroidDestroyed += Asteroid_OnAsteroidDestroyed;
                    
                         Rigidbody2D asteroidRigidBody = asteroidShard.GetComponent<Rigidbody2D>();
                     
                         float spawnSpeed = Random.Range(asteroidShard_script.asteroidSpeed - asteroidShard_script.asteroidSize1 ,
                             asteroidShard_script.asteroidMaxSpeed1 -  asteroidShard_script.asteroidSize1 );

                     
                         Vector2 direction2d = new Vector2(Random.value,Random.value).normalized;
                     
                         asteroidRigidBody.AddForce( spawnSpeed *  direction2d);

                        

                     }   
                     
                     ShatteredAsteroidCount+= asteroidShardPrefabs.Length;
                     
                 }

                 LargeAsteroidCount--;
            }
            else
            {
                
                ShatteredAsteroidCount--;

            }

            OnDestroyAsteroid?.Invoke(this, new OnDestroyAsteroidEventArgs
            {
                asteroidToDestroyGameObject = e.asteroidGameObject,
            });
            
            
            
            if (LargeAsteroidCount == 0 && ShatteredAsteroidCount == 0)
            {
                OnAllAsteroidsCleared?.Invoke(this, new OnAllAsteroidsClearedEventArgs
                {
                    shatteredAsteroidPeicesDestroyedCount = shatteredAsteroidDestroyedCount,
                    largeAsteroidDestroyedCount = largeAsteroidDestroyedCount
                });
            }
            
            // Debug.Log(largeAsteroidCount);
            // Debug.Log(ShatteredAsteroidCount);
        }

        private int GetAsteroidCountForCurrentLevel()
        {
            return (GameManager.Instance.GetLevelNumber() * asteroidLevelMultiplierFactor) +
                   asteroidLevelIncrementFactor;
        }


    }
}
using System;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


namespace DefaultNamespace
{
    public class Asteroid : MonoBehaviour
    {

        [SerializeField] protected float _asteroid_size = 0.3f;

       
        [SerializeField] protected float _asteroid_speed = 1f;


        [SerializeField] private float _asteroid_MaxSpeed = 2f;

        [SerializeField] private  int scoreValue = 1;

        public int getScoreValue => scoreValue;

        [SerializeField] private float asteroidDestroyTime = 0.02f;
        [SerializeField] private float bulletDestroyTime = 0.01f;
        
       // [SerializeField] private GameObject asteroidExplosionPrefab;
        [SerializeField] private GameObject _bulletExplosionPrefab;
        [SerializeField] private float bulletExplosionLifeTime;
        
        [SerializeField] private bool isAsteroidShard = false;
        
        [SerializeField] private GameObject[] asteroidShardPrefabObjects;
        
        [SerializeField] private  GameObject[] asteroidShardSpawnPoint;
    
        
        public float asteroidSpeed1 => _asteroid_speed;
        public float asteroidSize1 => _asteroid_size;

        public float asteroidDestroyTime1 => asteroidDestroyTime;

        public float asteroidMaxSpeed1 => _asteroid_MaxSpeed;
        public GameObject[] asteroidShardPrefabObjects1 => asteroidShardPrefabObjects;

        public GameObject[] asteroidShardSpawnPoint1 => asteroidShardSpawnPoint;

       
        public event EventHandler<OnAsteroidDestroyedEventArgs> OnAsteroidDestroyed;

      
        public float asteroidSize => _asteroid_size;

        public float asteroidSpeed => _asteroid_speed;

        public float asteroidMaxSpeed => _asteroid_MaxSpeed;

        public bool isAsteroidShard1 => isAsteroidShard;

        public event EventHandler OnAsteroidHit;
        
          
        
        public class OnAsteroidDestroyedEventArgs : EventArgs
        {
            public GameObject asteroidGameObject;
        }
        
        public void Start()
        {
 
            
        }

        /*public event EventHandler<OnBulletHitEventArgs> OnBulletHit;

        public class OnBulletHitEventArgs : EventArgs
        {

        }*/
        
        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.gameObject.TryGetComponent(out Bullet bullet))
            {
                OnAsteroidHit?.Invoke(this, EventArgs.Empty);

                GameObject explosion = Instantiate(_bulletExplosionPrefab, gameObject.transform.position, quaternion.identity);

                
                AsteroidManager.Instance.OnDestroyAsteroid += AsteroidManagerOnDestroyAsteroid;
                OnAsteroidDestroyed?.Invoke(this,
                    new OnAsteroidDestroyedEventArgs
                    { 
                        asteroidGameObject = this.gameObject,
                    });
                
                Destroy(explosion, bulletExplosionLifeTime);
                Destroy(bullet.gameObject);
                
            }
            /*
            if (collider2D.gameObject.TryGetComponent(out Asteroid asteroid))
            {
                Debug.Log("Asteroids Collide");
                Rigidbody2D asteroidRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                
                Rigidbody2D otherAsteroidRigidbody2D = asteroid.gameObject.GetComponent<Rigidbody2D>();
                
                Vector2 force = asteroidRigidbody2D.linearVelocity + otherAsteroidRigidbody2D.linearVelocity;
                
                
                asteroidRigidbody2D.AddForce( force, ForceMode2D.Impulse);
                otherAsteroidRigidbody2D.AddForce( -force, ForceMode2D.Impulse);

            }*/
            
             
        }

       

        private void AsteroidManagerOnDestroyAsteroid(object sender, AsteroidManager.OnDestroyAsteroidEventArgs e)
        {
            
            AsteroidManager.Instance.OnDestroyAsteroid -= AsteroidManagerOnDestroyAsteroid;
            
          

            
            Destroy(e.asteroidToDestroyGameObject,asteroidDestroyTime);
        }
    }
    

}
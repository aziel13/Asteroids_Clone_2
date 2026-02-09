using System;
using DefaultNamespace;
using DefaultNamespace.Utility;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Starship : Monobehaviour_Singleton<Starship>
{
    [Header("Ship parameters")]
     
    [SerializeField] private float force = 20f;
    [SerializeField] private float shipMaxVelocity = 40f;
    [SerializeField] private float turnSpeed = 600f;
    
    [Header("Object references")]

    
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private GameObject _ShipExplosionPrefab;
    [SerializeField] private float shipExplosionLifeTime;
    
    private float bulletFrequency = 1.5f;
    private float bulletTimer = 0f;
    private float bulletTimerMax = 0.5f;
    
    private Rigidbody2D shipRigidbody; 
    
    
    private bool isAlive = true;
    private bool isAccelerating = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isShipShooting = false;
    
    public event EventHandler OnUpForce;

    public event EventHandler OnRightForce;

    public event EventHandler OnLeftForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnWeaponDischarge;
    
    
    public enum GameState
    {
        Startup,
        GameRunning,
        GameEnd,
    }
    
    private GameState _gameState;
    public event EventHandler<OnGameStateChangeEventArgs> OnGameStateChange;

    [SerializeField] private int lives = 3;
    
    public class OnGameStateChangeEventArgs : EventArgs
    {
        public GameState gameState;
    }
    
    
    public event EventHandler<OnCrashEventArgs> OnCrash;

    
    public class OnCrashEventArgs : EventArgs
    {
        public int currentLives;
        public float scoreMultiplier;
        public int score;
    }
    
    private void Awake()
    {
        base.Awake();
        shipRigidbody = GetComponent<Rigidbody2D>(); 
        _gameState = GameState.Startup;
    }


    private void FixedUpdate()
    {

        switch (_gameState)
        {
            default:
            case GameState.Startup:
                if (GameInput.Instance.IsUpActionPressed() ||
                    GameInput.Instance.IsLeftActionPressed() ||
                GameInput.Instance.IsRightActionPressed() ||
                    GameInput.Instance.IsWeaponDischargeActionPressed())
                {
                    SetGameState(GameState.GameRunning);
                }

                break;
            case GameState.GameRunning:
                
                if (GameInput.Instance.IsUpActionPressed())
                {
                    shipRigidbody.AddForce(transform.up * (force * Time.deltaTime));
                }

                if (GameInput.Instance.IsLeftActionPressed())
                {

                    shipRigidbody.AddTorque(turnSpeed * Time.deltaTime);
                }

                if (GameInput.Instance.IsRightActionPressed())
                {
                    shipRigidbody.AddTorque(-turnSpeed * Time.deltaTime);
                }

                if (GameInput.Instance.IsWeaponDischargeActionPressed())
                {
                    if (bulletTimer <= 0)
                    {
                        Bullet bullet = Instantiate(bulletPrefab, Vector3.zero, quaternion.identity);

                        bullet.transform.position = bulletSpawnPoint.position;
                        bullet.ShipTransform = transform;
                        bulletTimer = bulletTimerMax;
                    }
                }

                if (bulletTimer > 0)
                {
                    bulletTimer -= bulletFrequency * Time.deltaTime;
                }

                break;
            case GameState.GameEnd:
                break;
            
        }
    }

    private void SetGameState(GameState gameState)
    {
        _gameState =  gameState;
         
        OnGameStateChange?.Invoke(this, new OnGameStateChangeEventArgs
        {
            gameState = _gameState,
        });
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.TryGetComponent(out Asteroid asteroid))
        {
                 
            GameObject explosion = Instantiate(_ShipExplosionPrefab, gameObject.transform.position, quaternion.identity);
                 
            Destroy(gameObject);
 
        }
    }

}

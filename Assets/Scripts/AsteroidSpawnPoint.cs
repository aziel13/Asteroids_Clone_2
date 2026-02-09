using UnityEngine;

public class AsteroidSpawnPoint : MonoBehaviour
{

    [SerializeField] private Transform[] AsteroidTargets;

    public Transform[] asteroidTargets => AsteroidTargets;
    
}

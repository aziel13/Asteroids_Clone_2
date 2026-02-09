using UnityEngine;


[CreateAssetMenu(fileName = "AsteroidShardScriptableObject", menuName = "Scriptable Objects/AsteroidShardScriptableObject")]
public class AsteroidShardScriptableObject : ScriptableObject
{
    
    [SerializeField] private GameObject asteroidShardPrefab;
    [SerializeField] private  GameObject asteroidShardSpawnPoint;
    
}

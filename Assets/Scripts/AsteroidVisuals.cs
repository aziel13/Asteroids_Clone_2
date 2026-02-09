using System;
using DefaultNamespace;
using UnityEngine;

public class AsteroidVisuals : MonoBehaviour
{

    
    [SerializeField] private Sprite crackedAsteroidSprite;

    private Asteroid _asteroid;
    
    
    void Start()
    {
        _asteroid = GetComponent<Asteroid>();
        
        _asteroid.OnAsteroidHit += Asteroid_OnAsteroidHit;
    }

    private void Asteroid_OnAsteroidHit(object sender, EventArgs e)
    {
        if (!_asteroid.isAsteroidShard1)
        {
            
            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = crackedAsteroidSprite;
            
        }
    }
}

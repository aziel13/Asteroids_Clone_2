using UnityEngine;

public class StarshipVisuals : MonoBehaviour
{

    private GameObject _StarshipRenderer;
    
    private void Start()
    {
        _StarshipRenderer = gameObject.GetComponentInChildren<SpriteRenderer>().gameObject;
        
        Starship.Instance.OnGameStateChange += Starship_OnGameStateChange;
        _StarshipRenderer.SetActive(false);
    }

    private void Starship_OnGameStateChange(object sender, Starship.OnGameStateChangeEventArgs e)
    {
        _StarshipRenderer.SetActive(true);
    }
 
    
    
}

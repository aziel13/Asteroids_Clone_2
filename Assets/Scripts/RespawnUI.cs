using DefaultNamespace;
using TMPro;
using UnityEngine;

public class RespawnUI : MonoBehaviour
{
    private TextMeshProUGUI _RespawnTextUI;
    [SerializeField] private float flashSpeed = 5f;

    private void Start()
    {
        
        _RespawnTextUI = GetComponentInChildren<TextMeshProUGUI>();
        _RespawnTextUI.gameObject.SetActive(false);
       
        GameManager.Instance.OnStateChange += GameManager_OnGameStateChange;
        GameManager.Instance.OnPlayerSpawn += GameManager_OnPlayerSpawn;
        

    }

    private void GameManager_OnGameStateChange(object sender, GameManager.OnStateChangeEventArgs e)
    {
        GameStateChange(e.gameState);

    }
    
    
    private void GameManager_OnPlayerSpawn(object sender, GameManager.OnPlayerSpawnEventArgs e)
    {
        //when player is spawned subscribe to events from the player
        e.playerGameObject.GetComponent<Starship>().OnGameStateChange += Starship_OnGameStateChange;
        e.playerGameObject.GetComponent<Starship>().OnCrash += Starship_OnCrash;
    }
    
    private void Starship_OnCrash(object sender,Starship.OnCrashEventArgs e)
    {
        //unsubscribe from events from the player
        e.gameObject.GetComponent<Starship>().OnGameStateChange -= Starship_OnGameStateChange;

    }

    private void Starship_OnGameStateChange(object sender, Starship.OnGameStateChangeEventArgs e)
    {
        GameStateChange(e.gameState);
        
    }

    private void GameStateChange(Starship.GameState gameState)
    {
        
        if (gameState == Starship.GameState.Respawning)
        {
            _RespawnTextUI.gameObject.SetActive(true);
        }
        else
        {
            _RespawnTextUI.gameObject.SetActive(false);
        }
        
    }


    private void Update()
    {
        
        float alpha = Mathf.PingPong(Time.time, flashSpeed);
        _RespawnTextUI.color = new Color(_RespawnTextUI.color.r, _RespawnTextUI.color.g, _RespawnTextUI.color.b, alpha);
        
    }
    
}

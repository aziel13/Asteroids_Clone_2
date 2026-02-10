using DefaultNamespace;
using TMPro;
using UnityEngine;

public class GameOverUIVisuals : MonoBehaviour
{
    private TextMeshProUGUI _TextUI;
    [SerializeField] private float flashSpeed = 5f;

    private void Start()
    {
        _TextUI = GetComponentInChildren<TextMeshProUGUI>();
        GameManager.Instance.OnPlayerSpawn += GameManager_OnPlayerSpawn;
        
        _TextUI.gameObject.SetActive(false);
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
        if (e.gameState == Starship.GameState.GameOver)
        {
            
            _TextUI.gameObject.SetActive(true);
        }

    }

    private void Update()
    {
        float alpha = Mathf.PingPong(Time.time, flashSpeed);
        _TextUI.color = new Color(_TextUI.color.r, _TextUI.color.g, _TextUI.color.b, alpha);
        
    }

}

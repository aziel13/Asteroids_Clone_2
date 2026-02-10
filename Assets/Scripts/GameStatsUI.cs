using DefaultNamespace;
using TMPro;
using UnityEngine;

public class GameStatsUI : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI _LevelText;
    
    [SerializeField] private TextMeshProUGUI _ScoreText;
    
    [SerializeField] private TextMeshProUGUI _LivesText;

    private void Start()
    {
        set_active(false);
        GameManager.Instance.OnPlayerSpawn += GameManager_OnPlayerSpawn;
        GameManager.Instance.OnStatsChange += GameManagerOnStatsChange;

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
    public void GameManagerOnStatsChange(object sender, GameManager.OnStatsChangeEventArgs e)
    {
        _LevelText.text = $"{e.level}";
        _ScoreText.text = $"{e.score}";
        _LivesText.text = $"{e.lives}";
    }
    
    private void Starship_OnGameStateChange(object sender, Starship.OnGameStateChangeEventArgs e)
    {
        if (e.gameState == GameManager.GameState.GameRunning)
        {
            set_active(true);
        }

        else if (e.gameState == GameManager.GameState.Startup)
        {
            set_active(false);
        }
    }
    
    private void set_active(bool isActive)
    {

        if (isActive)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }

    }

 
}

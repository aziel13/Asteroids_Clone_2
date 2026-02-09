using DefaultNamespace;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI _LevelText;
    
    [SerializeField] private TextMeshProUGUI _ScoreText;
    
    [SerializeField] private TextMeshProUGUI _LivesText;

    private void Start()
    {
        set_active(false);
        Starship.Instance.OnGameStateChange += Starship_OnGameStateChange;

        GameManager.Instance.OnStatsChange += GameManagerOnStatsChange;

    }
    
    public void GameManagerOnStatsChange(object sender, GameManager.OnStatsChangeEventArgs e)
    {
        _LevelText.text = $"{e.level}";
        _ScoreText.text = $"{e.score}";
        _LivesText.text = $"{e.lives}";
    }
    
    private void Starship_OnGameStateChange(object sender, Starship.OnGameStateChangeEventArgs e)
    {
        if (e.gameState == Starship.GameState.GameRunning)
        {
            set_active(true);
        }

        else if (e.gameState == Starship.GameState.Startup)
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

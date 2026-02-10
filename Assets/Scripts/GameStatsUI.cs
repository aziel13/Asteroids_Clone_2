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
        GameManager.Instance.OnStateChange += GameManager_OnGameStateChange;
        GameManager.Instance.OnStatsChange += GameManagerOnStatsChange;

    }
    private void GameManager_OnGameStateChange(object sender, GameManager.OnStateChangeEventArgs e)
    {
        if (e.gameState == GameManager.GameState.GameRunning)
        {
            set_active(true);
        }

        else  
        {
            set_active(false);
        }
        
    }
    
    public void GameManagerOnStatsChange(object sender, GameManager.OnStatsChangeEventArgs e)
    {
        _LevelText.text = $"{e.level}";
        _ScoreText.text = $"{e.score}";
        _LivesText.text = $"{e.lives}";
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

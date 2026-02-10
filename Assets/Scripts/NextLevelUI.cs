using DefaultNamespace;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _TextUI;
    [SerializeField] private TextMeshProUGUI _NextLevelInstructionTextUI;

    [SerializeField] private float flashSpeed = 5f;
 
    private Action newGameButtonClickAction;
    private void Awake()
    {
  
    }
    private void Start()
    {
        GameManager.Instance.OnLevelChange += GameManager_OnLevelChange;
        
        GameManager.Instance.OnStateChange += GameManager_OnGameStateChange;
        
        gameObject.SetActive(false);
    }

    private void GameManager_OnGameStateChange(object sender, GameManager.OnStateChangeEventArgs e)
    {
        if (e.gameState == GameManager.GameState.GameRunning)
        {
            gameObject.SetActive(false);
        }
    }
    
    private void GameManager_OnLevelChange(object sender, GameManager.OnLevelChangeEventArgs e)
    {
        
            _TextUI.text = $"{e.level}\n";
            _TextUI.text += $"{e.score}\n";
           
            _NextLevelInstructionTextUI.text =  $"Press {GameInput.Instance.GetFireKeyName()} for Next Level!\n";
            
            gameObject.SetActive(true);
        

    }

    private void Update()
    {
        float alpha = Mathf.PingPong(Time.time, flashSpeed);
        _TextUI.color = new Color(_TextUI.color.r, _TextUI.color.g, _TextUI.color.b, alpha);
        
    }

}

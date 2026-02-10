using DefaultNamespace;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _TextUI;
    [SerializeField] private TextMeshProUGUI _NewGameInstruction;
        
    [SerializeField] private float flashSpeed = 5f;

    private Action newGameButtonClickAction;
    private void Awake()
    {
        
    }
    private void Start()
    {
        
        GameManager.Instance.OnGameOver += GameManager_OnGameOverChange;
        
        
        
        
        gameObject.SetActive(false);
    }
     
    
   
    private void GameManager_OnGameOverChange(object sender, GameManager.OnGameOverEventArgs e)
    {
        
            
            _TextUI.text = $"{e.level}\n";
            _TextUI.text += $"{e.final_score}\n";
            
           _NewGameInstruction.text = $"Press {GameInput.Instance.GetFireKeyName()} for New Game!\n";
            gameObject.SetActive(true);
        

    }

    private void Update()
    {
        float alpha = Mathf.PingPong(Time.time, flashSpeed);
        _TextUI.color = new Color(_TextUI.color.r, _TextUI.color.g, _TextUI.color.b, alpha);
        
    }

}

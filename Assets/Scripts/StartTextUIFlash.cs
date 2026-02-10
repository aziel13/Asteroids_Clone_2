using DefaultNamespace;
using UnityEngine;
using TMPro;

public class StartTextUIFlash : MonoBehaviour
{
    private TextMeshProUGUI _TextUI;
    [SerializeField] private float flashSpeed = 5f;

    private void Start()
    {
        
        _TextUI = GetComponentInChildren<TextMeshProUGUI>();
        GameManager.Instance.OnStateChange += GameManager_OnGameStateChange;
    }
    
    private void GameManager_OnGameStateChange(object sender, GameManager.OnStateChangeEventArgs e)
    {
        if (e.gameState == GameManager.GameState.GameRunning)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        float alpha = Mathf.PingPong(Time.time, flashSpeed);
        _TextUI.color = new Color(_TextUI.color.r, _TextUI.color.g, _TextUI.color.b, alpha);
        
    }


}

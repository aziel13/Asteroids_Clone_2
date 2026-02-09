using TMPro;
using UnityEngine;

public class CrashTextVisuals : MonoBehaviour
{
    private TextMeshProUGUI _TextUI;
    [SerializeField] private float flashSpeed = 5f;

    private void Start()
    {
        
       
        _TextUI = GetComponentInChildren<TextMeshProUGUI>();
        Starship.Instance.OnGameStateChange += Starship_OnGameStateChange;
        _TextUI.gameObject.SetActive(false);
    }

    private void Starship_OnGameStateChange(object sender, Starship.OnGameStateChangeEventArgs e)
    {
        if (e.gameState == Starship.GameState.GameEnd)
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

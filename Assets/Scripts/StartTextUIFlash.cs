using UnityEngine;
using TMPro;

public class StartTextUIFlash : MonoBehaviour
{
    private TextMeshProUGUI _TextUI;
    [SerializeField] private float flashSpeed = 5f;

    private void Start()
    {
        
        _TextUI = GetComponentInChildren<TextMeshProUGUI>();
        Starship.Instance.OnGameStateChange += Starship_OnGameStateChange;
    }

    private void Starship_OnGameStateChange(object sender, Starship.OnGameStateChangeEventArgs e)
    {
        _TextUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        float alpha = Mathf.PingPong(Time.time, flashSpeed);
        _TextUI.color = new Color(_TextUI.color.r, _TextUI.color.g, _TextUI.color.b, alpha);
        
    }


}

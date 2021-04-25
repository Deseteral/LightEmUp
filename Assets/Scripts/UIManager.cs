using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    private GameMaster gameMaster;
    private Damageable playerDamageable;
    
    private Text coinValueText;
    private Text healthValueText;
    private Text currentLevelValueText;

    private void Start() {
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        playerDamageable = GameObject.Find("Player").GetComponent<Damageable>();
        
        coinValueText = GameObject.Find("CoinValue").GetComponent<Text>();
        healthValueText = GameObject.Find("HealthValue").GetComponent<Text>();
        currentLevelValueText = GameObject.Find("CurrentLevelValue").GetComponent<Text>();
        
        currentLevelValueText.text = $"Level: {gameMaster.currentLevel}";
    }

    private void FixedUpdate() {
        coinValueText.text = gameMaster.coins.ToString();
        healthValueText.text = playerDamageable.health.ToString();
    }
}
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    private GameMaster gameMaster;
    private Damageable playerDamageable;
    
    private Text coinValueText;
    private Text healthValueText;

    private void Start() {
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        playerDamageable = GameObject.Find("Player").GetComponent<Damageable>();
        
        coinValueText = GameObject.Find("CoinValue").GetComponent<Text>();
        healthValueText = GameObject.Find("HealthValue").GetComponent<Text>();
    }

    private void FixedUpdate() {
        coinValueText.text = gameMaster.coins.ToString();
        healthValueText.text = playerDamageable.health.ToString();
    }
}
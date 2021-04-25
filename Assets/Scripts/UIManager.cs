using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    private GameMaster gameMaster;
    private Text coinValueText;
    
    private void Start() {
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        coinValueText = GameObject.Find("CoinValue").GetComponent<Text>();
    }

    private void FixedUpdate() {
        coinValueText.text = gameMaster.coins.ToString();
    }
}
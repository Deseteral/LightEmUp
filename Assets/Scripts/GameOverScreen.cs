using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {
    private Text scoreText;
    private GameMaster gameMaster;

    void Start() {
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
    }

    void Update() {
        scoreText.text = $"Score: {gameMaster.score}";

        if (Input.GetKey(KeyCode.Space)) {
            SceneManager.LoadScene("MenuScene");
        }
    }
}
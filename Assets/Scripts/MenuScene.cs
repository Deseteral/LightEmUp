using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour {
    private void Start() {
        var gameMaster = GameObject.Find("GameMaster");
        if (gameMaster != null) {
            Destroy(gameMaster);
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene("GameScene");
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            SceneManager.LoadScene("HelpScreen");
        }
    }
}
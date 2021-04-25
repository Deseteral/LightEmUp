using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour {
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene("GameScene");
        }
    }
}
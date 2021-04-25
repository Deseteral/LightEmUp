using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {
    public int coins = 30;
    public int currentLevel = 1;
    public int score = 0;
    
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    public void AdvanceToNextLevel() {
        currentLevel += 1;
        SceneManager.LoadScene("GameScene");
    }
}
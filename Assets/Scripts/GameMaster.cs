using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {
    public int coins;
    public int currentLevel = 1; 
    
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    public void AdvanceToNextLevel() {
        currentLevel += 1;
        SceneManager.LoadScene("GameScene");
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {
    public int coins;
    
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    public void AdvanceToNextLevel() {
        SceneManager.LoadScene("GameScene");
    }
}
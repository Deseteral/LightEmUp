using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public GameObject enemyPrefab;
    
    private Timer nextSpawnTimer = new Timer();

    private const int NEXT_SPAWN_TIME_MIN_MS = 3 * 1000;
    private const int NEXT_SPAWN_TIME_MAX_MS = 10 * 1000;
    
    void Start() {
        nextSpawnTimer.Set(0);
    }

    void Update() {
        if (nextSpawnTimer.Check()) {
            var spawnPosition = transform.position + (Vector3)(Vector2.right * 2f);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            
            nextSpawnTimer.Set(Random.Range(NEXT_SPAWN_TIME_MIN_MS, NEXT_SPAWN_TIME_MAX_MS));
        }
    }
}
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {
    public float speed = 3f;
    public float attentionRadius = 5.5f;

    public GameObject coinPrefab;

    private Vector2 delta = Vector2.zero;
    private Timer timer = new Timer();

    private new Rigidbody2D rigidbody;
    private GameObject player;
    private EnemySpawner parent;
    private GameObject coinsContainer;

    private void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        coinsContainer = GameObject.Find("CoinsContainer");
        
        timer.Set(0);
    }

    private void FixedUpdate() {
        Vector2 position = transform.position;
        Vector2 playerPosition = player.transform.position;
        float distanceToPlayer = Vector2.Distance(position, playerPosition);

        if (distanceToPlayer > 15f) return; // Perf optimization

        Vector2 directionToPlayer = (playerPosition - position).normalized;

        timer.Update();

        // Randomly change position
        if (timer.Check()) {
            ChangeDirection(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
        }

        // Is attracted to player?
        if (CanSeePlayer(directionToPlayer, distanceToPlayer) && distanceToPlayer <= attentionRadius) {
            delta = directionToPlayer;
        }

        // Apply force
        rigidbody.AddForce(delta * speed, ForceMode2D.Force);

        // DrawCircle(transform.position, AttentionRadius, Color.red);
    }

    private void ChangeDirection(Vector2 newDirection) {
        delta = newDirection;
        timer.Set(Random.Range(3000, 10000));
    }

    private void OnCollisionEnter2D(Collision2D target) {
        // Bounce off the wall
        if (target.gameObject.name == "Walls") {
            ChangeDirection(delta * -1);
        }
    }

    private bool CanSeePlayer(Vector2 directionToPlayer, float distanceToPlayer) {
        int tilemapAndPlayerMask = (1 << 7) | (1 << 8);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer + 10f, tilemapAndPlayerMask);

        return hit.collider != null && hit.collider.gameObject.CompareTag("Player");
    }

    public void SetParent(EnemySpawner enemySpawner) {
        parent = enemySpawner;
    }

    public void OnDeath() {
        parent.ChildrenDied();

        for (int i = 0; i < Random.Range(1, 5); i++) {
            var coinsGameObject = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            coinsGameObject.transform.parent = coinsContainer.transform;
        }
    }
}
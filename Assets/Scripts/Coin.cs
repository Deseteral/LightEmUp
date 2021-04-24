using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour {
    private GameObject player;
    private new Rigidbody2D rigidbody;
    
    private void Start() {
        player = GameObject.Find("Player");
        rigidbody = GetComponent<Rigidbody2D>();
        
        var splashDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        splashDirection *= 0.5f;
        GetComponent<Rigidbody2D>().AddForce(splashDirection, ForceMode2D.Impulse);
    }

    private void FixedUpdate() {
        var position = transform.position;
        var playerPosition = player.transform.position;
        
        Vector2 directionToPlayer = (playerPosition - position).normalized;
        float distanceToPlayer = Vector2.Distance(position, playerPosition);
        float speedScale = (1f / distanceToPlayer) * 4f;
        
        if (distanceToPlayer < 2f) {
            rigidbody.AddForce(directionToPlayer * speedScale, ForceMode2D.Force);
        }
    }

    private void OnCollisionEnter2D(Collision2D target) {
        if (target.gameObject.CompareTag("Player")) {
            target.gameObject.GetComponent<Player>().coins += 5;
            Destroy(gameObject);
        }
    }
}
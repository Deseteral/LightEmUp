using UnityEngine;

public class Coin : MonoBehaviour {
    void Start() {
        var splashDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        splashDirection *= 0.5f;
        GetComponent<Rigidbody2D>().AddForce(splashDirection, ForceMode2D.Impulse);
    }
    
    private void OnCollisionEnter2D(Collision2D target) {
        if (target.gameObject.CompareTag("Player")) {
            target.gameObject.GetComponent<Player>().coins += 5;
            Destroy(gameObject);
        }
    }
}
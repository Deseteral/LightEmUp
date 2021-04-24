using UnityEngine;

public class Bullet : MonoBehaviour {
    public Vector2 direction = Vector2.zero;
    public float speed = 0.25f;

    private void FixedUpdate() {
        var position = transform.position;
        position = position + ((Vector3)direction * speed);
        transform.position = position;
    }
    
    private void OnCollisionEnter2D(Collision2D target) {
        var targetObject = target.gameObject;
        if (targetObject.name == "Walls" || targetObject.CompareTag("Spawner") || targetObject.CompareTag("Enemy")) {
            Destroy(gameObject);
        }
    }
}
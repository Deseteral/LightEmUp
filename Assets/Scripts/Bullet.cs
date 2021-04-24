using UnityEngine;

public class Bullet : MonoBehaviour {
    public Vector2 direction = Vector2.zero;
    public float speed = 0.25f;
    
    void Start() { }

    private void FixedUpdate() {
        var position = transform.position;
        position = position + ((Vector3)direction * speed);
        transform.position = position;
    }
    
    private void OnCollisionEnter2D(Collision2D target) {
        if (target.gameObject.name == "Walls") {
            Destroy(gameObject);
        }
    }
}
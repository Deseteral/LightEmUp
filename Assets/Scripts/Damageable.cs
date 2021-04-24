using UnityEngine;

public class Damageable : MonoBehaviour {
    public int health = 100;

    public bool ignoreBullets = false;
    public bool ignoreEnemies = false;

    private bool blink = false;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void Update() {
        if (health <= 0) {
            Destroy(gameObject);
        }

        if (blink) {
            spriteRenderer.color = Color.white;
            blink = false;
        } else {
            spriteRenderer.color = originalColor;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.CompareTag("Bullet") && ignoreBullets == false) {
            health -= 5;
            blink = true;
        }
        
        if (other.collider.CompareTag("Enemy") && ignoreEnemies == false) {
            health -= 5;
            blink = true;
        }
    }
}
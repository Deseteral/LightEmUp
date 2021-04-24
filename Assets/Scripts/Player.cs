using DefaultNamespace;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 25f;
    public float recoilStrength = 1f;
    public int shootingDelayMs = 150;
    public GameObject bulletPrefab;

    public GameObject debugSpawnPrefab; // TODO: DEBUG REMOVE

    private new Rigidbody2D rigidbody;
    private Timer shootingDelayTimer = new Timer();

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        shootingDelayTimer.Set(0);
    }

    private void Update() {
        shootingDelayTimer.Update();
    }

    void FixedUpdate() {
        Vector2 delta = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) delta.y += speed;
        if (Input.GetKey(KeyCode.S)) delta.y -= speed;
        if (Input.GetKey(KeyCode.D)) delta.x += speed;
        if (Input.GetKey(KeyCode.A)) delta.x -= speed;

        rigidbody.AddForce(delta, ForceMode2D.Force);

        // Debug spawn enemy
        // TODO: DEBUG REMOVE
        if (Input.GetMouseButtonDown(1)) {
            var pos = Utils.MouseInWorld();
            Instantiate(debugSpawnPrefab, pos, Quaternion.identity);
        }

        // Shoot
        if (Input.GetMouseButton(0) && shootingDelayTimer.Check()) {
            var position = transform.position;
            var mouseInWorld = Utils.MouseInWorld();
            Vector2 shootingDirection = (mouseInWorld - position).normalized;
            var bulletPosition = (Vector2) transform.position + shootingDirection;
            
            GameObject bulletGameObject = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity);
            bulletGameObject.GetComponent<Bullet>().direction = shootingDirection;

            rigidbody.AddForce(-shootingDirection * recoilStrength, ForceMode2D.Impulse);
            
            shootingDelayTimer.Set(shootingDelayMs);
        }
    }
}
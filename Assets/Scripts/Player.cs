using System;
using DefaultNamespace;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 25f;
    public float recoilStrength = 1f;
    public int shootingDelayMs = 150;
    public int coins = 0;
    public GameObject bulletPrefab;

    private new Rigidbody2D rigidbody;
    private Timer shootingDelayTimer = new Timer();
    private GameObject bulletsContainer;
    private CableGrid cableGrid;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        bulletsContainer = GameObject.Find("BulletsContainer");
        cableGrid = GameObject.Find("CableGrid").GetComponent<CableGrid>();
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

        // Place cable
        if (Input.GetMouseButton(1)) {
            var pos = Utils.MouseInWorld();
            var tileX = (int)pos.x;
            var tileY = (int)pos.y;
            
            cableGrid.PlaceCable(tileX, tileY);
        }

        // Shoot
        if (Input.GetMouseButton(0) && shootingDelayTimer.Check()) {
            var position = transform.position;
            var mouseInWorld = Utils.MouseInWorld();
            Vector2 shootingDirection = (mouseInWorld - position).normalized;
            var bulletPosition = (Vector2) transform.position + shootingDirection;

            GameObject bulletGameObject = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity);
            bulletGameObject.GetComponent<Bullet>().direction = shootingDirection;
            bulletGameObject.transform.parent = bulletsContainer.transform;

            rigidbody.AddForce(-shootingDirection * recoilStrength, ForceMode2D.Impulse);

            shootingDelayTimer.Set(shootingDelayMs);
        }
    }
    
    private void OnGUI() {
        GUI.Label(new Rect(0, 0, 100, 50), coins.ToString());
    }
}
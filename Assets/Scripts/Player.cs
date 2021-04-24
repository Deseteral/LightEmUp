using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour {
    public float Speed = 3f;
    public float RecoilStrength = 1f;
    public int ShootingDelayMs = 10;
    public GameObject BulletPrefab;

    public GameObject enemyPrefab; // TODO: DEBUG REMOVE

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
        if (Input.GetKey(KeyCode.W)) delta.y += Speed;
        if (Input.GetKey(KeyCode.S)) delta.y -= Speed;
        if (Input.GetKey(KeyCode.D)) delta.x += Speed;
        if (Input.GetKey(KeyCode.A)) delta.x -= Speed;

        rigidbody.AddForce(delta, ForceMode2D.Force);

        // Debug spawn enemy
        // TODO: DEBUG REMOVE
        if (Input.GetMouseButtonDown(1)) {
            var pos = Utils.MouseInWorld();
            Instantiate(enemyPrefab, pos, Quaternion.identity);
        }

        // Shoot
        if (Input.GetMouseButton(0) && shootingDelayTimer.Check()) {
            var position = transform.position;
            var mouseInWorld = Utils.MouseInWorld();
            Vector2 shootingDirection = (mouseInWorld - position).normalized;
            var bulletPosition = (Vector2) transform.position + shootingDirection;
            
            GameObject bulletGameObject = Instantiate(BulletPrefab, bulletPosition, Quaternion.identity);
            bulletGameObject.GetComponent<Bullet>().Direction = shootingDirection;

            rigidbody.AddForce(-shootingDirection * RecoilStrength, ForceMode2D.Impulse);
            
            shootingDelayTimer.Set(ShootingDelayMs);
        }
    }
}
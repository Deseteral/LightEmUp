using System;
using DefaultNamespace;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 25f;
    public float recoilStrength = 0.2f;

    public Sprite[] sprites;

    private Vector2 delta;

    private new Rigidbody2D rigidbody;
    private Tool tool;
    private CableGrid cableGrid;
    private AudioManager audioManager;
    private GameMaster gameMaster;
    private SpriteRenderer spriteRenderer;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        tool = GetComponent<Tool>();
        cableGrid = GameObject.Find("CableGrid").GetComponent<CableGrid>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate() {
        var position = transform.position;
        var mouseInWorld = Utils.MouseInWorld();

        delta = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) delta += Vector2.up;
        if (Input.GetKey(KeyCode.S)) delta += Vector2.down;
        if (Input.GetKey(KeyCode.D)) delta += Vector2.right;
        if (Input.GetKey(KeyCode.A)) delta += Vector2.left;
        delta = delta.normalized;

        rigidbody.AddForce(delta * speed, ForceMode2D.Force);

        // Use tool
        if (Input.GetMouseButton(0)) {
            if (tool.toolType == ToolType.Gun) {
                Vector2 shootingDirection = (mouseInWorld - position).normalized;
                bool didShoot = tool.Use(shootingDirection);

                if (didShoot) {
                    // Apply recoil
                    rigidbody.AddForce(-shootingDirection * recoilStrength, ForceMode2D.Impulse);

                    // Play sound
                    audioManager.PlayPlayerShootingGunSound();
                }
            } else {
                tool.Use(mouseInWorld);
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            tool.SecondaryUse(mouseInWorld);
        }

        // Change tool type
        if (Input.GetKey(KeyCode.Alpha1)) tool.toolType = ToolType.Gun;
        if (Input.GetKey(KeyCode.Alpha2)) tool.toolType = ToolType.PlaceCable;
        if (Input.GetKey(KeyCode.Alpha3)) tool.toolType = ToolType.PlaceLamp;
        if (Input.GetKey(KeyCode.Alpha4)) tool.toolType = ToolType.PlaceTurret;

        // Rotate sprite
        {
            var dir = (mouseInWorld - position);
            float deadZone = 0.4f;
            int idx = 0;
            int xx = 0;
            int yy = 0;

            if (dir.x > deadZone) xx = 1;
            if (dir.x < -deadZone) xx = -1;
            if (dir.y > deadZone) yy = 1;
            if (dir.y < -deadZone) yy = -1;

            if (xx == 0 && yy == 1) idx = 0;
            if (xx == 1 && yy == 1) idx = 1;
            if (xx == 1 && yy == 0) idx = 2;
            if (xx == 1 && yy == -1) idx = 3;
            if (xx == 0 && yy == -1) idx = 4;
            if (xx == -1 && yy == -1) idx = 5;
            if (xx == -1 && yy == 0) idx = 6;
            if (xx == -1 && yy == 1) idx = 7;

            spriteRenderer.sprite = sprites[idx];
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider != null && other.collider.gameObject.CompareTag("Enemy")) {
            var enemy = other.collider.GetComponent<Enemy>();
            rigidbody.AddForce(enemy.GetDelta() * 3f, ForceMode2D.Impulse);
        }
    }
}
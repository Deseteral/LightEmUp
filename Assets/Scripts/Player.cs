using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour {
    public float Speed = 3f;
    public GameObject enemyPrefab;

    private new Rigidbody2D rigidbody;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        Vector2 delta = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) delta.y += Speed;
        if (Input.GetKey(KeyCode.S)) delta.y -= Speed;
        if (Input.GetKey(KeyCode.D)) delta.x += Speed;
        if (Input.GetKey(KeyCode.A)) delta.x -= Speed;

        rigidbody.AddForce(delta, ForceMode2D.Force);

        // Debug spawn enemy
        if (Input.GetMouseButtonDown(0)) {
            var v3 = Input.mousePosition;
            v3.z = 10.0f;
            v3 = Camera.main.ScreenToWorldPoint(v3);
            Object.Instantiate(enemyPrefab, v3, Quaternion.identity);
        }
    }
}
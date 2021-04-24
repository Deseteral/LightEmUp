using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour {
    public float Speed = 3f;

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
    }
}
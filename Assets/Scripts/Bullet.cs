using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public Vector2 Direction = Vector2.zero;
    public float Speed = 0.1f;
    
    void Start() { }

    private void FixedUpdate() {
        var position = transform.position;
        position = position + ((Vector3)Direction * Speed);
        transform.position = position;
    }
    
    private void OnCollisionEnter2D(Collision2D target) {
        if (target.gameObject.name == "Walls") {
            Destroy(gameObject);
        }
    }
}
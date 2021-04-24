using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {
    public float Speed = 3f;
    public float AttentionRadius = 4f;

    private Vector2 delta = Vector2.zero;
    private Timer timer = new Timer();

    private new Rigidbody2D rigidbody;

    private GameObject player;

    private void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        timer.Set(0);
    }

    private void FixedUpdate() {
        Vector2 position = transform.position;
        Vector2 playerPosition = player.transform.position;
        float distanceToPlayer = Vector2.Distance(position, playerPosition);
        Vector2 directionToPlayer = (playerPosition - position).normalized;

        timer.Update();

        // Randomly change position
        if (timer.Check()) {
            ChangeDirection(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
        }

        // Is attracted to player?
        if (CanSeePlayer(directionToPlayer, distanceToPlayer) && distanceToPlayer <= AttentionRadius) {
            delta = directionToPlayer;
        }

        // Apply force
        rigidbody.AddForce(delta * Speed, ForceMode2D.Force);

        // DrawCircle(transform.position, AttentionRadius, Color.red);
    }

    private void ChangeDirection(Vector2 newDirection) {
        delta = newDirection;
        timer.Set(Random.Range(3000, 10000));
    }

    private void OnCollisionEnter2D(Collision2D target) {
        // Bounce off the wall
        if (target.gameObject.name == "Walls") {
            ChangeDirection(delta * -1);
        }
    }

    private bool CanSeePlayer(Vector2 directionToPlayer, float distanceToPlayer) {
        int tilemapAndPlayerMask = (1 << 7) | (1 << 8);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer + 10f, tilemapAndPlayerMask);

        return hit.collider != null && hit.collider.gameObject.name == "Player";
    }

    // private void DrawCircle(Vector2 centerPosition, float radius, Color color) {
    //     int segments = 10;
    //     float stepAmount = (2f * Mathf.PI) / segments;
    //
    //     Vector3 prevPosition = Vector3.zero;
    //     for (int step = 0; step < segments; step++) {
    //         Vector2 offset = new Vector2(Mathf.Sin(step * stepAmount), Mathf.Cos(step * stepAmount));
    //         Vector3 position = centerPosition + (offset * radius);
    //         position.z = -10;
    //
    //         if (step == 0) {
    //             prevPosition = position;
    //         } else {
    //             Debug.DrawLine(prevPosition, position, color, 0, false);
    //             prevPosition = position;
    //         }
    //     }
    // }
}
using UnityEngine;

public class Turret : MonoBehaviour {
    private ElectricalDevice electricalDevice;
    private Gun gun;

    private void Start() {
        electricalDevice = GetComponent<ElectricalDevice>();
        gun = GetComponent<Gun>();
    }

    private void Update() {
        if (electricalDevice.hasPower == false) return; // TODO: POTENTIAL PREF PROBLEM
        
        var position = transform.position;
        
        // Find closest enemy
        GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");

        float maxDistance = 10f;

        Vector2 shootingDirection = Vector2.zero;
        foreach (var e in enemyGameObjects) {
            var targetPosition = e.transform.position;
            var distanceToTarget = Vector2.Distance(targetPosition, position);
            if (distanceToTarget > maxDistance) continue;

            Vector2 directionToTarget = (targetPosition - position).normalized;
            if (CanSeeTarget(directionToTarget, distanceToTarget)) {
                shootingDirection = directionToTarget;
                break;
            }
        }

        if (shootingDirection != Vector2.zero) {
            gun.Shoot(shootingDirection);
        }
    }
    
    private bool CanSeeTarget(Vector2 directionToTarget, float distanceToTarget) {
        int tilemapAndEnemiesMask = (1 << 6) | (1 << 8);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget + 10f, tilemapAndEnemiesMask);

        return hit.collider != null && hit.collider.gameObject.CompareTag("Enemy");
    }
}
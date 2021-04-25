using UnityEngine;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour {
    public float spread = 10f;
    public int shootingDelayMs = 150;
    public GameObject bulletPrefab;

    private GameObject bulletsContainer;
    private Timer shootingDelayTimer = new Timer();

    private void Start() {
        bulletsContainer = GameObject.Find("BulletsContainer");
        shootingDelayTimer.Set(0);
    }

    private void FixedUpdate() {
        shootingDelayTimer.Update();
    }

    public bool Shoot(Vector2 shootingDirection) {
        if (shootingDelayTimer.Check() == false) return false;

        // Apply spread
        shootingDirection = Quaternion.Euler(0, 0, Random.Range(-spread, spread)) * shootingDirection;
        
        // Shoot the thing
        var position = transform.position;
        var bulletPosition = (Vector2) position + shootingDirection;

        GameObject bulletGameObject = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity, bulletsContainer.transform);
        bulletGameObject.GetComponent<Bullet>().direction = shootingDirection;

        shootingDelayTimer.Set(shootingDelayMs);
        return true;
    }
}
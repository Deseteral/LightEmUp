using UnityEngine;

public class Gun : MonoBehaviour {
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

    public void Shoot(Vector2 shootingDirection) {
        if (shootingDelayTimer.Check() == false) return;

        var position = transform.position;
        var bulletPosition = (Vector2) position + shootingDirection;

        GameObject bulletGameObject = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity, bulletsContainer.transform);
        bulletGameObject.GetComponent<Bullet>().direction = shootingDirection;

        shootingDelayTimer.Set(shootingDelayMs);
    }
}
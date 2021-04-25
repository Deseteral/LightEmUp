using DefaultNamespace;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 25f;
    public float recoilStrength = 0.5f;

    public GameObject lampPrefab;
    public GameObject turretPrefab;

    private new Rigidbody2D rigidbody;
    private Gun gun;
    private CableGrid cableGrid;
    private AudioManager audioManager;
    private GameMaster gameMaster;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        gun = GetComponent<Gun>();
        cableGrid = GameObject.Find("CableGrid").GetComponent<CableGrid>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
    }

    void FixedUpdate() {
        var position = transform.position;
        var mouseInWorld = Utils.MouseInWorld();
        var tileX = (int) mouseInWorld.x;
        var tileY = (int) mouseInWorld.y;
        Vector3 tilePosition = new Vector3(tileX + 0.5f, tileY + 0.5f, 0);

        Vector2 delta = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) delta.y += speed;
        if (Input.GetKey(KeyCode.S)) delta.y -= speed;
        if (Input.GetKey(KeyCode.D)) delta.x += speed;
        if (Input.GetKey(KeyCode.A)) delta.x -= speed;

        rigidbody.AddForce(delta, ForceMode2D.Force);

        // Place cable
        if (Input.GetMouseButton(1)) {
            cableGrid.PlaceCable(tileX, tileY);
        }

        // Remove cable
        if (Input.GetKeyDown(KeyCode.R)) {
            cableGrid.RemoveCable(tileX, tileY);
        }

        // Place lamp
        if (Input.GetKeyDown(KeyCode.Q)) {
            Instantiate(lampPrefab, tilePosition, Quaternion.identity);
        }

        // Place turret
        if (Input.GetKeyDown(KeyCode.T)) {
            Instantiate(turretPrefab, tilePosition, Quaternion.identity);
        }

        // Shoot
        if (Input.GetMouseButton(0)) {
            Vector2 shootingDirection = (mouseInWorld - position).normalized;
            bool didShoot = gun.Shoot(shootingDirection);

            if (didShoot) {
                // Apply recoil
                rigidbody.AddForce(-shootingDirection * recoilStrength, ForceMode2D.Impulse);

                // Play sound
                audioManager.PlayPlayerShootingGunSound();
            }
        }
    }

    private void OnGUI() {
        GUI.Label(new Rect(0, 0, 100, 50), gameMaster.coins.ToString());
    }
}
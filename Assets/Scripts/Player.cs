using DefaultNamespace;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 25f;
    public float recoilStrength = 0.5f;

    private new Rigidbody2D rigidbody;
    private Tool tool;
    private CableGrid cableGrid;
    private AudioManager audioManager;
    private GameMaster gameMaster;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        tool = GetComponent<Tool>();
        cableGrid = GameObject.Find("CableGrid").GetComponent<CableGrid>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
    }

    void FixedUpdate() {
        var position = transform.position;
        var mouseInWorld = Utils.MouseInWorld();

        Vector2 delta = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) delta.y += speed;
        if (Input.GetKey(KeyCode.S)) delta.y -= speed;
        if (Input.GetKey(KeyCode.D)) delta.x += speed;
        if (Input.GetKey(KeyCode.A)) delta.x -= speed;

        rigidbody.AddForce(delta, ForceMode2D.Force);

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
    }

    private void OnGUI() {
        GUI.Label(new Rect(0, 0, 100, 50), gameMaster.coins.ToString());
        GUI.Label(new Rect(0, 60, 100, 50), tool.toolType.ToString());
    }
}
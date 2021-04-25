using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ToolType {
    Gun,
    PlaceCable,
    PlaceLamp,
    PlaceTurret,
}

public class Tool : MonoBehaviour {
    public float spread = 10f;
    public int shootingDelayMs = 150;
    public ToolType toolType = ToolType.Gun;

    public GameObject bulletPrefab;
    public GameObject lampPrefab;
    public GameObject turretPrefab;

    private MapGenerator mapGenerator;
    private CableGrid cableGrid;

    private GameObject bulletsContainer;
    private GameObject devicesContainer;

    private Dictionary<(int, int), GameObject> devicesMap;

    private Timer shootingDelayTimer = new Timer();

    private void Start() {
        cableGrid = GameObject.Find("CableGrid").GetComponent<CableGrid>();
        mapGenerator = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
        bulletsContainer = GameObject.Find("BulletsContainer");
        devicesContainer = GameObject.Find("DevicesContainer");

        devicesMap = new Dictionary<(int, int), GameObject>();

        shootingDelayTimer.Set(0);
    }

    private void FixedUpdate() {
        shootingDelayTimer.Update();
    }

    public bool Use(Vector2 positionOrDirection) {
        return toolType switch {
            ToolType.Gun => Shoot(positionOrDirection),
            ToolType.PlaceCable => PlaceCable(positionOrDirection),
            ToolType.PlaceLamp => PlaceLamp(positionOrDirection),
            ToolType.PlaceTurret => PlaceTurret(positionOrDirection),
            _ => false,
        };
    }

    public bool SecondaryUse(Vector2 position) {
        return RemoveItem(position);
    }

    private bool Shoot(Vector2 shootingDirection) {
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

    private bool PlaceCable(Vector2 position) {
        var tilePosition = GetTilePosition(position);
        return cableGrid.PlaceCable((int) tilePosition.x, (int) tilePosition.y);
    }

    private bool PlaceDevice(Vector2 position, GameObject devicePrefab) {
        var tilePosition = GetTilePosition(position);
        var coord = ((int) tilePosition.x, (int) tilePosition.y);

        if (devicesMap.ContainsKey(coord) == false) {
            var deviceObject = Instantiate(devicePrefab, tilePosition, Quaternion.identity, devicesContainer.transform);
            devicesMap[coord] = deviceObject;
            return true;
        }

        return false;
    }
    
    private bool PlaceLamp(Vector2 position) {
        return PlaceDevice(position, lampPrefab);
    }

    private bool PlaceTurret(Vector2 position) {
        return PlaceDevice(position, turretPrefab);
    }

    private bool RemoveItem(Vector2 position) {
        var tilePosition = GetTilePosition(position);
        var coord = ((int) tilePosition.x, (int) tilePosition.y);

        // Try remove device
        var device = devicesMap.GetOrDefault(coord, null); 
        if (device != null) {
            devicesMap.Remove(coord);
            Destroy(device.gameObject);
            return true;
        }
        
        // Try remove cable
        return cableGrid.RemoveCable((int) tilePosition.x, (int) tilePosition.y);
    }

    private Vector3 GetTilePosition(Vector2 position) {
        var tileX = (int) position.x;
        var tileY = (int) position.y;
        return new Vector3(tileX + 0.5f, tileY + 0.5f, 0);
    }
}
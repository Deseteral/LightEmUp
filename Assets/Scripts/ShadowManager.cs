using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShadowManager : MonoBehaviour {
    public Tile shadowTile;
    
    private Tilemap shadowTilemap;
    private MapGenerator mapGenerator;

    private static Vector3Int[] EVERY_POSITION = null;
    private static TileBase[] EVERY_SHADOW_TILE = null;

    private void Start() {
        shadowTilemap = GameObject.Find("Shadow").GetComponent<Tilemap>();
        mapGenerator = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();

        if (EVERY_POSITION == null || EVERY_SHADOW_TILE == null) {
            EVERY_POSITION = new Vector3Int[mapGenerator.size * mapGenerator.size];
            for (int y = 0; y < mapGenerator.size; y++) {
                for (int x = 0; x < mapGenerator.size; x++) {
                    int idx = x + y * mapGenerator.size;
                    EVERY_POSITION[idx] = new Vector3Int(x, y, 0);
                }
            }

            EVERY_SHADOW_TILE = Enumerable.Repeat<TileBase>(shadowTile, EVERY_POSITION.Length).ToArray();
        }
    }

    public void RegenerateShadowMap() {
        // Shadow everything
        shadowTilemap.SetTiles(EVERY_POSITION, EVERY_SHADOW_TILE);

        // Only set light tiles
        IEnumerable<Vector3Int> positions = new List<Vector3Int>();
        var lampGameObjects = GameObject.FindGameObjectsWithTag("Light");
        foreach (var lampGameObject in lampGameObjects) {
            // Check if lamp has power
            if (lampGameObject.GetComponent<ElectricalDevice>().hasPower == false) continue;
            
            var lamp = lampGameObject.GetComponent<Lamp>();
            positions = positions.Concat(lamp.GetLightTiles());
        }

        Vector3Int[] positionArray = positions.ToArray();
        TileBase[] tileArray = Enumerable.Repeat<TileBase>(null, positionArray.Length).ToArray();
        shadowTilemap.SetTiles(positionArray, tileArray);
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CableGrid : MonoBehaviour {
    public GameObject cableTilePrefab;

    private bool[,] map;
    private Dictionary<(int, int), CableTile> cableTiles = new Dictionary<(int, int), CableTile>();

    private MapGenerator mapGenerator;

    private static readonly List<(int, int)> DIRECTIONS = new List<(int, int)> {(0, 1), (1, 0), (0, -1), (-1, 0)};
    private static readonly List<(int, int)> DIRECTIONS_WITH_CENTER = DIRECTIONS.Concat(new List<(int, int)> {(0, 0)}).ToList();

    private void Start() {
        mapGenerator = GameObject.Find("GameManager").GetComponent<MapGenerator>();
        map = new bool[mapGenerator.size, mapGenerator.size];
    }

    public void PlaceCable(int x, int y) {
        // Check if cable can be placed
        var (generatorX, generatorY) = mapGenerator.generatorCoords;
        if (mapGenerator.GetCollisonMap()[x, y] || (generatorX == x && generatorY == y) || map[x, y]) return;

        // Create CableTile game object
        var cableTilePosition = new Vector3(x + 0.5f, y + 0.5f, 0);
        var cableTileGameObject = Instantiate(cableTilePrefab, cableTilePosition, Quaternion.identity, transform);
        var cableTile = cableTileGameObject.GetComponent<CableTile>();

        // Update maps
        cableTiles[(x, y)] = cableTile;
        map[x, y] = true;

        // Regenerate power information
        RegeneratePowerInfo();
        
        // Reset sprites on all cables
        foreach (var key in cableTiles.Keys) {
            cableTiles[key].ResetSprites(GetNeighbourCables(key.Item1, key.Item2));
        }
    }

    private List<bool> GetNeighbourCables(int x, int y) {
        var (generatorX, generatorY) = mapGenerator.generatorCoords;
        List<bool> neighbours = new List<bool>();

        foreach (var (i, j) in DIRECTIONS) {
            if (i == 0 && j == 0) continue;
            int nx = x + i;
            int ny = y + j;

            if (nx < 0 || ny < 0 || nx >= mapGenerator.size || ny >= mapGenerator.size) {
                neighbours.Add(false);
            } else if (generatorX == nx && generatorY == ny) {
                neighbours.Add(true);
            } else {
                neighbours.Add(map[nx, ny]);
            }
        }

        return neighbours;
    }

    private void RegeneratePowerInfo() {
        var cablePositions = cableTiles.Keys.ToList();
        var queue = new List<(int, int)> {mapGenerator.generatorCoords};

        while (queue.Count > 0) {
            var (x, y) = queue[0];
            queue.RemoveAt(0);

            foreach (var (dx, dy) in DIRECTIONS_WITH_CENTER) {
                int nx = x + dx;
                int ny = y + dy;
                var coord = (nx, ny);

                if (cablePositions.Contains(coord)) {
                    cablePositions.Remove(coord);
                    
                    queue.Add(coord);
                    cableTiles[coord].hasPower = true;
                }
            }
        }

        foreach (var coord in cablePositions) {
            cableTiles[coord].hasPower = false;
        }
    }
}
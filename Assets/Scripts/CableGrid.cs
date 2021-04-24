using System.Collections.Generic;
using UnityEngine;

public class CableGrid : MonoBehaviour {
    public GameObject cableTilePrefab;

    private bool[,] map;
    private Dictionary<(int, int), CableTile> cableTiles = new Dictionary<(int, int), CableTile>();

    private MapGenerator mapGenerator;

    private readonly List<(int, int)> DIRECTIONS = new List<(int, int)> {(0, 1), (1, 0), (0, -1), (-1, 0)};

    void Start() {
        mapGenerator = GameObject.Find("GameManager").GetComponent<MapGenerator>();
        map = new bool[mapGenerator.size, mapGenerator.size];
    }

    public void PlaceCable(int x, int y) {
        if (mapGenerator.GetCollisonMap()[x, y] == true) return;

        var cableTilePosition = new Vector3(x + 0.5f, y + 0.5f, 0);
        var cableTileGameObject = Instantiate(cableTilePrefab, cableTilePosition, Quaternion.identity, transform);
        var cableTile = cableTileGameObject.GetComponent<CableTile>();
        cableTile.ResetSprites(GetNeighbours(x, y));

        cableTiles[(x, y)] = cableTile;
        map[x, y] = true;
        
        foreach (var (i, j) in DIRECTIONS) {
            int nx = x + i;
            int ny = y + j;
            
            CableTile nCableTile;
            cableTiles.TryGetValue((nx, ny), out nCableTile);
            if (nCableTile != null) {
                nCableTile.ResetSprites(GetNeighbours(nx, ny));
            }
        }
    }

    public List<bool> GetNeighbours(int x, int y) {
        List<bool> neighbours = new List<bool>();

        foreach (var (i, j) in DIRECTIONS) {
            if (i == 0 && j == 0) continue;
            int nx = x + i;
            int ny = y + j;

            if (nx < 0 || ny < 0 || nx >= mapGenerator.size || ny >= mapGenerator.size) {
                neighbours.Add(false);
            } else if (mapGenerator.generatorCoords.Item1 == nx && mapGenerator.generatorCoords.Item2 == ny) {
                neighbours.Add(true);
            } else {
                neighbours.Add(map[nx, ny]);
            }
        }

        return neighbours;
    }
}
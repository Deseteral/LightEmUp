using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour {
    public Tilemap ground;
    public Tilemap walls;

    public Tile groundTile;
    public Tile wallTile;
    
    public int size = 64;

    private bool[,] m;

    void Start() {
        bool isValidMap = false;

        while (!isValidMap) {
            CelluarAutomata();
            RemoveClosedRooms();

            var (spawnX, spawnY) = FindSpawnPoint();

            if (spawnX == -1 || spawnY == -1) continue;
            isValidMap = true;
            
            GameObject.Find("Player").transform.position = new Vector3(spawnX + 0.5f, spawnY + 0.5f, 0);
        }

        ApplyMapToTilemap();
    }

    private void CelluarAutomata() {
        m = new bool[size, size];

        int numberOfSteps = 2;
        int birthLimit = 5;
        int deathLimit = 4;
        float chanceToStartAlive = 0.49f;

        // Initialization
        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                m[x, y] = Random.Range(0f, 1f) < chanceToStartAlive;
            }
        }

        // Simulation step
        for (int step = 0; step < numberOfSteps; step++) {
            var newMap = new bool[size, size];
            for (int y = 0; y < size; y++) {
                for (int x = 0; x < size; x++) {
                    var nc = CountNeighbours(x, y);
                    if (m[x, y]) {
                        newMap[x, y] = nc < deathLimit ? false : true;
                    } else if (nc > birthLimit) {
                        newMap[x, y] = true;
                    } else {
                        newMap[x, y] = false;
                    }
                }
            }

            m = newMap;
        }
        
        // Make enclosing walls
        for (int x = 0; x < size; x++) {
            m[x, 0] = true; // top
            m[x, size - 1] = true; // bottom
        }

        for (int y = 0; y < size; y++) {
            m[0, y] = true; // left
            m[size - 1, y] = true; // right
        }
        
    }

    private int CountNeighbours(int x, int y) {
        int count = 0;
        foreach (int i in new List<int> {-1, 0, 1}) {
            foreach (int j in new List<int> {-1, 0, 1}) {
                if (i == 0 && j == 0) continue;
                int nx = x + i;
                int ny = y + j;

                if ((nx < 0 || ny < 0 || nx >= size || ny >= size) || m[nx, ny]) {
                    count++;
                }
            }
        }

        return count;
    }

    private void RemoveClosedRooms() {
        var floodMap = new bool[size, size];
        var queue = new List<(int, int)>();

        floodMap[size / 2, size / 2] = true;
        queue.Add((size / 2, size / 2));
        
        // Mark neighbours
        while (queue.Count > 0) {
            var (x, y) = queue[0];
            queue.RemoveAt(0);
            
            foreach (int i in new List<int> {-1, 0, 1}) {
                foreach (int j in new List<int> {-1, 0, 1}) {
                    if (i != 0 && j != 0) continue;

                    int nx = x + i;
                    int ny = y + j;
                    
                    if (
                        (nx >= 0 && ny >= 0 && nx < size && ny < size) &&
                        (m[nx, ny] == false && floodMap[nx, ny] == false)
                    ) {
                        floodMap[nx, ny] = true;
                        queue.Add((nx, ny));
                    }
                }
            }
        }

        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                if (m[x, y] == false && floodMap[x, y] == false) m[x, y] = true;
            }
        }
    }

    private (int, int) FindSpawnPoint() {
        for (int distance = 0; distance < (size/2); distance++) {
            if (m[distance, distance] == false) return (distance, distance);
            if (m[distance, size-1-distance] == false) return (distance, size - 1 - distance);
            if (m[size-1-distance, distance] == false) return (size-1-distance, distance);
            if (m[size-1-distance, size-1-distance] == false) return (size-1-distance, size-1-distance);
        }

        return (-1, -1);
    }

    private void ApplyMapToTilemap() {
        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                ground.SetTile(new Vector3Int(x, y, 0), groundTile);
            }
        }
        
        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                if (m[x, y]) {
                    walls.SetTile(new Vector3Int(x, y, 0), wallTile);   
                }
            }
        }
    }
}
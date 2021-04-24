using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour {
    public Tilemap Ground;
    public Tilemap Walls;

    public Tile GroundTile;
    public Tile WallTile;
    
    public int Size = 512;

    private bool[,] m;

    void Start() {
        CelluarAutomata();
        RemoveClosedRooms();

        var (spawnX, spawnY) = FindSpawnPoint();
        GameObject.Find("Player").transform.position = new Vector3(spawnX, spawnY, 0);
        
        ApplyMapToTilemap();
    }

    private void CelluarAutomata() {
        m = new bool[Size, Size];

        int numberOfSteps = 2;
        int birthLimit = 5;
        int deathLimit = 4;
        float chanceToStartAlive = 0.49f;

        // Initialization
        for (int y = 0; y < Size; y++) {
            for (int x = 0; x < Size; x++) {
                m[x, y] = Random.Range(0f, 1f) < chanceToStartAlive;
            }
        }

        // Simulation step
        for (int step = 0; step < numberOfSteps; step++) {
            var newMap = new bool[Size, Size];
            for (int y = 0; y < Size; y++) {
                for (int x = 0; x < Size; x++) {
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
        for (int x = 0; x < Size; x++) {
            m[x, 0] = true; // top
            m[x, Size - 1] = true; // bottom
        }

        for (int y = 0; y < Size; y++) {
            m[0, y] = true; // left
            m[Size - 1, y] = true; // right
        }
        
    }

    private int CountNeighbours(int x, int y) {
        int count = 0;
        foreach (int i in new List<int> {-1, 0, 1}) {
            foreach (int j in new List<int> {-1, 0, 1}) {
                if (i == 0 && j == 0) continue;
                int nx = x + i;
                int ny = y + j;

                if ((nx < 0 || ny < 0 || nx >= Size || ny >= Size) || m[nx, ny]) {
                    count++;
                }
            }
        }

        return count;
    }

    private void RemoveClosedRooms() {
        var floodMap = new bool[Size, Size];
        var queue = new List<(int, int)>();

        floodMap[Size / 2, Size / 2] = true;
        queue.Add((Size / 2, Size / 2));
        
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
                        (nx >= 0 && ny >= 0 && nx < Size && ny < Size) &&
                        (m[nx, ny] == false && floodMap[nx, ny] == false)
                    ) {
                        floodMap[nx, ny] = true;
                        queue.Add((nx, ny));
                    }
                }
            }
        }

        for (int y = 0; y < Size; y++) {
            for (int x = 0; x < Size; x++) {
                if (m[x, y] == false && floodMap[x, y] == false) m[x, y] = true;
            }
        }
    }

    private (int, int) FindSpawnPoint() {
        int distance = -1;

        while (true) {
            distance++;
            if (m[distance, distance] == false) return (distance, distance);
            if (m[distance, Size-1-distance] == false) return (distance, Size - 1 - distance);
            if (m[Size-1-distance, distance] == false) return (Size-1-distance, distance);
            if (m[Size-1-distance, Size-1-distance] == false) return (Size-1-distance, Size-1-distance);
        }
    }

    private void ApplyMapToTilemap() {
        for (int y = 0; y < Size; y++) {
            for (int x = 0; x < Size; x++) {
                Ground.SetTile(new Vector3Int(x, y, 0), GroundTile);
            }
        }
        
        for (int y = 0; y < Size; y++) {
            for (int x = 0; x < Size; x++) {
                if (m[x, y]) {
                    Walls.SetTile(new Vector3Int(x, y, 0), WallTile);   
                }
            }
        }
    }
}
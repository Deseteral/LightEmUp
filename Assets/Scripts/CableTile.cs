using System.Collections.Generic;
using UnityEngine;

public class CableTile : MonoBehaviour {
    public bool hasPower = false;

    public SpriteRenderer[] directionalSprites;

    private readonly Color DISABLED_COLOR = new Color(0x5A / 255f, 0x5A / 255f, 0x5A / 255f);

    public void ResetSprites(List<bool> neighbours) {
        // Reset values
        foreach (var sprite in directionalSprites) {
            sprite.enabled = false;
        }

        // Enable sprites
        bool anySet = false;
        for (int index = 0; index < neighbours.Count; index++) {
            directionalSprites[index].enabled = neighbours[index];
            if (neighbours[index] == true) anySet = true;
        }

        directionalSprites[4].enabled = !anySet; // Set center
        
        // Change color based on power status
        if (hasPower) {
            foreach (var sprite in directionalSprites) {
                sprite.color = Color.white;
            }
        } else {
            foreach (var sprite in directionalSprites) {
                sprite.color = DISABLED_COLOR;
            }
        }
    }
}
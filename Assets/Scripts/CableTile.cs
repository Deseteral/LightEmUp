using System.Collections.Generic;
using UnityEngine;

public class CableTile : MonoBehaviour {
    public bool hasPower = false;

    public SpriteRenderer center;
    public SpriteRenderer[] directionalSprites;

    private readonly Color DISABLED_COLOR = new Color(0x5A / 255f, 0x5A / 255f, 0x5A / 255f);

    public void ResetSprites(List<bool> neighbours) {
        // Reset values
        center.enabled = false;
        foreach (var sprite in directionalSprites) {
            sprite.enabled = false;
        }

        // Enable sprites
        for (int index = 0; index < neighbours.Count; index++) {
            directionalSprites[index].enabled = neighbours[index];
        }

        Debug.Log(hasPower);
        
        // Change color based on power status
        if (hasPower) {
            center.color = Color.white;
            foreach (var sprite in directionalSprites) {
                sprite.color = Color.white;
            }
        } else {
            center.color = DISABLED_COLOR;
            foreach (var sprite in directionalSprites) {
                sprite.color = DISABLED_COLOR;
            }
        }
    }
}
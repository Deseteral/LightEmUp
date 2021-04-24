using System.Collections.Generic;
using UnityEngine;

public class CableTile : MonoBehaviour {
    public SpriteRenderer center;
    public SpriteRenderer[] directionalSprites;
    
    public void ResetSprites(List<bool> neighbours) {
        center.enabled = false;
        directionalSprites[0].enabled = false;
        directionalSprites[1].enabled = false;
        directionalSprites[2].enabled = false;
        directionalSprites[3].enabled = false;

        bool didSet = false;
        for (int index = 0; index < neighbours.Count; index++) {
            directionalSprites[index].enabled = neighbours[index];
            if (neighbours[index]) didSet = true;
        }

        if (didSet == false) center.enabled = true;
    }
}
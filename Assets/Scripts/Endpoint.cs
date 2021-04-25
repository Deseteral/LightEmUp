using UnityEngine;

public class Endpoint : MonoBehaviour {
    public bool hasPower = false;

    private CableGrid cableGrid;
    private int tileX;
    private int tileY;

    private void Start() {
        cableGrid = GameObject.Find("CableGrid").GetComponent<CableGrid>();
        ResetPositionInfo();
    }

    public void RegeneratePowerInfo() {
        hasPower = cableGrid.IsTilePowered(tileX, tileY);
    }

    public void ResetPositionInfo() {
        var position = transform.position;
        tileX = (int) position.x;
        tileY = (int) position.y;
    }
}
using UnityEngine;

public class ElectricalDevice : MonoBehaviour {
    public bool hasPower = false;

    private CableGrid cableGrid;
    private int x;
    private int y;

    private void Start() {
        cableGrid = GameObject.Find("CableGrid").GetComponent<CableGrid>();
        ResetPositionInfo();
    }

    private void FixedUpdate() {
        hasPower = cableGrid.IsTilePowered(x, y); // TODO: MIGHT BE A PERF PROBLEM
    }

    public void ResetPositionInfo() {
        var position = transform.position;
        x = (int)position.x;
        y = (int)position.y;
    }
}
using System;
using UnityEngine;

public class Endpoint : MonoBehaviour {
    public bool hasPower = false;

    private CableGrid cableGrid;
    private int tileX;
    private int tileY;

    private GameObject playerObject;
    private GameMaster gameMaster;

    private void Start() {
        cableGrid = GameObject.Find("CableGrid").GetComponent<CableGrid>();
        playerObject = GameObject.Find("Player");
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        ResetPositionInfo();
    }

    private void FixedUpdate() {
        if (hasPower) {
            var playerPosition = playerObject.transform.position;
            if (Vector3.Distance(transform.position, playerPosition) < 2f) {
                if (Input.GetKeyDown(KeyCode.X)) gameMaster.AdvanceToNextLevel();
            }
        }
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
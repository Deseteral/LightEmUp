using UnityEngine;

public class Lamp : MonoBehaviour {
    
    private static readonly Color ENABLED_COLOR = Color.yellow;
    private static readonly Color DISABLED_COLOR = Color.gray;

    private ElectricalDevice electricalDevice;
    private SpriteRenderer spriteRenderer;

    private void Start() {
        electricalDevice = GetComponent<ElectricalDevice>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        spriteRenderer.color = electricalDevice.hasPower ? ENABLED_COLOR : DISABLED_COLOR;
    }
}
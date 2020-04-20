using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyOverlay : MonoBehaviour
{
    // Create an overlay animation for a key in game

    public Sprite[] sprites;
    [SerializeField]
    SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateOverlay(float value) {
        // Update the overlay level
        // To be called from another object script
        spriteRenderer.sprite = sprites[(int)Mathf.Clamp(Mathf.Round(value * (sprites.Length)), 0, sprites.Length-1)];
    }

    public void SetDisplay(bool show) {
        if(spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.enabled = show;
    }
}

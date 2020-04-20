using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameKeyButton : MonoBehaviour {
    public Sprite keySprite;
    public string inputAxisName;

    public bool complete;
    public float timeToCompletion;
    public bool changing;
    private float progress;
    // Components
    private SpriteRenderer keyRenderer;
    private KeyOverlay overlay;
    private Transform parentTransform;

    // Player object
    private Transform player;
    public float activationRange = 3.0f;
    public bool active = true;
    public bool playerInRange;

    private void OnValidate() {
        keyRenderer = GetComponent<SpriteRenderer>();
        keyRenderer.sprite = keySprite;
    }

    void Start() {
        // Get components
        keyRenderer = GetComponent<SpriteRenderer>();
        overlay = GetComponentInChildren<KeyOverlay>();
        parentTransform = transform.parent.GetComponent<Transform>();
        // Get player
        GameObject pl = GameObject.FindGameObjectWithTag("Player");
        if (pl != null)
            player = pl.GetComponent<Transform>();
        Reset();
    }



    void Update() {
        // Assume button is off
        SetDisplay(false);
        if (!active || !playerInRange) {
            progress = 0;
        }
        if (!complete) {
            if (player != null)
                playerInRange = Vector2.SqrMagnitude(player.position - parentTransform.position) <= activationRange * activationRange;

            // If player is within range, and button is active
            if (active && playerInRange) {
                // Show buttons
                SetDisplay(true);
                float change = (Input.GetAxisRaw(inputAxisName) * 3 - 2) * Time.deltaTime / timeToCompletion;
                changing = change > 0;

                progress = Mathf.Clamp(progress + change, 0, 1);
                overlay.UpdateOverlay(progress);
                if (progress >= 1) {
                    complete = true;
                    SetDisplay(false);
                }
            }
        }
    }

    void SetDisplay(bool show) {
        // Show or hide the button
        if (keyRenderer != null)
            keyRenderer.enabled = show;
        overlay.SetDisplay(show);
    }

    public void Reset() {
        // Reset the button to be used again
        SetDisplay(false);
        complete = false;
        progress = 0.0f;

    }
}

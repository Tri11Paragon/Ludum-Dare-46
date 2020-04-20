using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireStates : MonoBehaviour {
    


    [Space]
    [Header("Sprites")]
    public SpriteRenderer spriteRenderer;
    public Sprite[] fireLogSprites;
    public Animator flameAnimator;

    [Space]
    [Header("Basic Fire Controls")]
    public int logs = 0;
    public int maxLogs = 5;
    public float decreaseSpeed = 0.1f; // How much fire strength decreases by per second
    [Range(0, 5)]
    public float fireStrength = 5; // 0 [off] -> 5 [full blast]

    [Space]
    [Header("Heating Effect Ranges")]
    [SerializeField]
    bool debugShowRanges = false;
    public float minRange = 2.0f;   // Within this distance, distances are considered equal
    public float heatRangeScale = 1.5f; // Range of heating scale factor from strength
    public AnimationCurve heatCurvePositive;
    private float heatRange;
    public float coolRange = 20.0f; // Range where cooling curve is affected
    public AnimationCurve heatCurveNegative;

    [Space]
    [Header("Heating Effect Modifiers")]
    public float heatEffectScale = 1.0f;
    public float coolEffectScale = 1.0f;

    [Space]
    [Header("Player Interactions")]
    public InGameKeyButton addLogsButton;
    public InGameKeyButton lightFireButton;

    //Other
    private PlayerController playerController;

    //**********************************************************************************

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update(){

        if(fireStrength > 0) {
            logs = Mathf.CeilToInt(fireStrength);
        }

        // Button inputs
        addLogsButton.active = logs < maxLogs && playerController.numLogs > 0;
        if (addLogsButton.complete) {
            // If the player has added a log, decrease their inventory count and add one to the fire
            playerController.GiveLog(-1);
            logs++;
            // Reset the button to be reused
            addLogsButton.Reset();
            // Increase fire strength if fire is lit
            if (fireStrength > 0) fireStrength++;
        }

        lightFireButton.active = fireStrength < 0.01 && logs > 0;
        if (lightFireButton.complete) {
            fireStrength = 1.5f * logs;
            lightFireButton.Reset();
        }

        fireStrength = Mathf.Clamp(fireStrength - decreaseSpeed * Time.deltaTime, 0, 5);

        heatRange = heatRangeScale * (fireStrength);

        // Update Sprites and Animations
        spriteRenderer.sprite = fireLogSprites[Mathf.Clamp(logs, 0, fireLogSprites.Length-1)];
        flameAnimator.SetFloat("FlameLevel", fireStrength / 5);
    }


    public float FireEffect(Vector3 playerPosition) {
        // Return the change to player's health per unit time
        // depending on their distance from the fire

        float curveValue = FireCurveValue(playerPosition);

        if(curveValue > 0) {
            // Scale value if positive
            return curveValue * fireStrength * heatEffectScale;
        } else {
            // Cooling is not scaled
            return curveValue * coolEffectScale;
        }
    }
    public float FireCurveValue(Vector3 playerPosition) { 

        // Calculate distance from player to fire
        float dist = (playerPosition - transform.position).magnitude;

        if(fireStrength <= 0) {
            // If fire is out, return lowest value of cooling
            return heatCurveNegative.Evaluate(1.0f);
        }
        if (dist < minRange) {
            // If player is within minimum range, return value of start of curve
            return heatCurvePositive.Evaluate(0.0f);
        }
        if (dist - minRange < heatRange) {
            // If player is in heatRange, evaluate curve at position in heat range
            return heatCurvePositive.Evaluate((dist - minRange) / heatRange);
        }
        // If player is out of heat range, evaluate curve at position in cooling range
        // Or at final value if past range
        return heatCurveNegative.Evaluate((dist - minRange - heatRange) / coolRange);
    }

    // Gizmos to draw heat ranges
    void OnDrawGizmosSelected() {
        // Draw debug gizmos
        if (debugShowRanges) {
            DrawHeatRanges();
        }
    }
    public void DrawHeatRanges() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minRange);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, minRange + heatRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, minRange + heatRange + coolRange);
    }
}

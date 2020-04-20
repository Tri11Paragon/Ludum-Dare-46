using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    void Awake() {
#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 45;
#endif
    }

    FireStates fire;
    public Animator animator;

    [Header("Health Settings")]
    public int maxHealth = 100;
    [SerializeField]
    public float health;

    [Header("Score / Stats")]
    public TMPro.TextMeshProUGUI logCounterDisplay;
    [SerializeField]
    public bool hasBackpack = false;

    public bool chopping;

    [Header("Items")]
    public int maxLogsWithoutBackpack = 2;
    public Sprite log;
    [SerializeField]
    public int numLogs = 0;

    [Header("UI")]
    public HealthBar healthbar;
    public CanvasManager canvasManager;
    public SoundEffects soundEffects;
    public GameObject handsFullError;


    void Start() {
        // Find fire object
        fire = GameObject.FindGameObjectWithTag("Fire").GetComponent<FireStates>();
        // Set health slider max value
        healthbar.setMax(maxHealth);
        animator.SetBool("HasBackpack", hasBackpack);
    }

    // Update is called once per frame
    void Update() {

        CalculateHealth();

        handsFullError.SetActive(chopping && numLogs >= maxLogsWithoutBackpack);

        animator.SetBool("Chopping", chopping);
        animator.SetBool("HasBackpack", hasBackpack);
        animator.SetBool("IsFrozen", health <= 0);
        if(health <= 0) {
            Die();
        }
        UpdateUI();
    }


    public void Die() {
        canvasManager.ChangeState(2);
    }
    public bool GiveLog(int num) {
        //chopping = false;
        if(num <= 0) {
            numLogs += num;
            return true;
        } else {
            if(numLogs < maxLogsWithoutBackpack || hasBackpack) {
                numLogs += num;
                soundEffects.PlayAxeChop();
                return true;
            } else {
                return false;
            }
        }
    }

    public float GetTemperatureValue()
    {
        return health;
    }

    void CalculateHealth() {
        health += fire.FireEffect(transform.position) * Time.deltaTime;
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    void UpdateUI() {
        // Update the thermometer reading
        healthbar.SetHealth((int)health);
        // Update log counter text
        logCounterDisplay.text = '×' + numLogs.ToString();
    }

    void updateSpriteOrder(GameObject obj, float yOffset = 0.0f) {

        // NO LONGER USED:
        // ProjectSettings -> Custom Transparency Sort Axis


        // If the player is behind a sprite, they should appear on a lower layer
        // Determined by comparing the y-positions of the sprites, including an offest value (defaulted to zero)
        if (transform.position.y < fire.transform.position.y + yOffset) {
            obj.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
        } else {
            obj.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
    }

}

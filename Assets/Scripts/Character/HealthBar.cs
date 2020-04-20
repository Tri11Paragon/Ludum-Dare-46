using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public Slider slider;

    public void SetHealth(int health) {
        // Adjust the health bar UI display
        slider.value = health;
    }
    public void setMax(int max) {
        slider.maxValue = max;
    }
}

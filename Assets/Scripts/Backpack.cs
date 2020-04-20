using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour {

    public InGameKeyButton pickUpButton;

    void Update() {
        if (pickUpButton.complete) {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().hasBackpack = true;
            Destroy(gameObject);
        }
    }
}

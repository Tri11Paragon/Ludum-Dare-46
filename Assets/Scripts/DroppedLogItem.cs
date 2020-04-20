using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedLogItem : MonoBehaviour
{
    public InGameKeyButton pickUpButton;
    public PlayerController player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update() {
        if (pickUpButton.playerInRange) {
            pickUpButton.active = player.numLogs < player.maxLogsWithoutBackpack;
        }
        if (pickUpButton.complete) {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().GiveLog(1);
            Destroy(gameObject);
        }
    }
}

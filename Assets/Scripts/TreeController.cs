using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour {

    public InGameKeyButton chopButton;
    public GameObject droppedLogPrefab;
    [Header("Chopping Setting")]
    public bool chopped = false;
    public Sprite stump;
    public BoxCollider2D colliderBox;


    // Game variables
    PlayerController playerController;

    void Start() {
        // Retreive player
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        ChangeCollider();

    }

    void Update() {
        if (!chopped) {
            if (chopButton.playerInRange) {
                playerController.chopping = chopButton.changing;
                
                    
            }
            chopped = chopButton.complete;
            if (chopped) {
                GetComponent<SpriteRenderer>().sprite = stump;
                ChangeCollider();
                playerController.chopping = false;
                if (!playerController.GiveLog(1)) {
                    Instantiate(droppedLogPrefab, transform.position+new Vector3(0, -1, 0), Quaternion.identity);
                }
                GameObject.FindGameObjectWithTag("WorldMaker").GetComponent<World>().TreeChopped(this.gameObject);
            }
        }
    }

    void ChangeCollider() {
        if (!chopped) {
            // Collider for whole tree is large
            colliderBox.offset = new Vector2(0, -0.75f);
            colliderBox.size = new Vector2(1.5f, 1.5f);
        } else {
            // Collider for stump is smaller
            colliderBox.offset = new Vector2(-0.03125f, -1.25f);
            colliderBox.size = new Vector2(0.5625f, 0.5f);
        }
    }

}

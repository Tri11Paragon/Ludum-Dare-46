using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5.0f;
    public Rigidbody2D rb;
    public Animator animator;
    public Transform triggerer;

    private PlayerController playerController;

    Vector2 movement;

    private void Start() {
        playerController = GetComponent<PlayerController>();
    }

    void Update() {
        // Handle input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Flip Player if facing right
        if (movement.x > 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        } else if (movement.x < 0) {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (playerController.chopping) {
            movement = Vector2.zero;
        }
        // Update Animator
        animator.SetBool("Moving", movement.sqrMagnitude > 0.01f);
    }

    void FixedUpdate() {
        // Handle movement
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        triggerer.position = rb.position;
    }
}

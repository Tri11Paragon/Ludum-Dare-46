using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowingAnimation : MonoBehaviour {
    public float speed = 5.0f;


    void Update() {
        this.transform.position += Vector3.down * speed * Time.deltaTime;
    }
}

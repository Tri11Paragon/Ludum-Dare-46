using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {
    public GameObject item;
    public float minRange;
    public float maxRange;
}
public class SpawnItems : MonoBehaviour {
    public Item[] itemsToSpawn;

    void Start() {
        for (int i = 0; i < itemsToSpawn.Length; i++) {
            // Instantiate the object in a random range around origin

            // Make a random vector with an angle and radius
            float angle = Random.Range(0, Mathf.PI);
            float length = Random.Range(itemsToSpawn[i].minRange, itemsToSpawn[i].maxRange);
            Vector3 position = new Vector3(Mathf.Cos(angle) * length, Mathf.Sin(angle) * length, 0f);

            // Instantiate the object
            Instantiate(itemsToSpawn[i].item, transform.position + position, Quaternion.identity);
        }
    }
}

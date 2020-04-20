using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeColiderTrigger : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        GameObject go = this.transform.gameObject;
        //go.SetActive(true);
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        TreeController tc = go.GetComponent<TreeController>();
        if (sr == null){
            sr = go.GetComponentInChildren<SpriteRenderer>();
        }
        sr.enabled = true;
        if (tc != null)
            tc.enabled = true;
    }

    public void OnTriggerExit2D(Collider2D other) {
        GameObject go = this.transform.gameObject;
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        TreeController tc = go.GetComponent<TreeController>();
        if (sr == null){
            sr = go.GetComponentInChildren<SpriteRenderer>();
        }
        sr.enabled = false;
        if (tc != null)
            tc.enabled = false;
        //go.SetActive(false);
    }

}

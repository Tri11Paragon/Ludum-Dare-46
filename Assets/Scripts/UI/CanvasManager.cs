using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    public GameObject[] loadingItems;
    public GameObject[] gameItems;
    public GameObject[] deathItems;
    public GameObject[] pausedItems;

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("WorldMaker") == null) {
            ChangeState(1);
        } else {
            ChangeState(0);
        }
    }

    public void ChangeState(int state) {
        // 0 - loading
        // 1 - gameplay
        // 2 - screen
        // 3 - paused

        // Turn on or off UI element sets
        for (int i = 0; i < loadingItems.Length; i++) {
            loadingItems[i].SetActive(state == 0);
        }

        for (int i = 0; i < gameItems.Length; i++) {
            gameItems[i].SetActive(state==1);
        }

        for (int i = 0; i < deathItems.Length; i++) {
            deathItems[i].SetActive(state==2);
        }

        for (int i = 0; i < pausedItems.Length; i++) {
            pausedItems[i].SetActive(state == 3);
        }
        switch (state) {
            case 0:
                // Dont run processes in loading scene
                Time.timeScale = 0.0f;
                break;
            case 1:
                // Run normally in gameplay
                Time.timeScale = 1.0f;
                break;
            case 2:
                // Slow camera follow in death scene
                Time.timeScale = 0.5f;
                break;
            case 3:
                // Stop timer if paused
                Time.timeScale = 0.0f;
                break;
        }
        // Only allow player to control when in play mode
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().enabled = state==1;
        player.GetComponent<PlayerController>().enabled = state==1;
    }

    public void PauseGame(bool pause) {
        // Pause game if true, otherwise unpause
        if (pause) {
            ChangeState(3);
        } else {
            ChangeState(1);
        }
    }

    public void ReturnToMainMenu() {
        string name = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<Score>().username;
        GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<Score>().endGame();
        Destroy(GameObject.FindGameObjectWithTag("WorldMaker"));
        Destroy(GameObject.FindGameObjectWithTag("ScoreKeeper"));
        SceneManager.LoadScene(0);
        StartCoroutine(reloadScoreName(name));
    }

    IEnumerator reloadScoreName(string name) {
        GameObject go = null;
        while (go == null){
            go = GameObject.FindGameObjectWithTag("ScoreKeeper");
            if (go != null)
                go.GetComponent<Score>().username = name;
            yield return new WaitForEndOfFrame();
        }
    }

}

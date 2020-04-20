using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public GameChanger worldGenerator;

    public GameObject mainMenuControls;
    public GameObject highscoresDisplay;
    public GameObject controlsDisplay;
    public GameObject creditsDisplay;
    public GameObject newGameMenu;

    [Header("Fox Animations")]
    public Animator FoxImage;
    public float avgChangeSpeed = 10.0f;
    private float progress = 0.0f;
    private float nextChangeTime;
    private bool foxWalking = false;

    private void Start() {
        // Hide all menus except main
        ReturnToMainMenu();
    }

    private void Update() {
        progress += Time.deltaTime;
        if (progress > nextChangeTime) {
            foxWalking = !foxWalking;
            FoxImage.SetBool("Walking", foxWalking);
            nextChangeTime = Random.Range(avgChangeSpeed / 2, 2 * avgChangeSpeed);
            progress = 0.0f;
        }
    }

    public void ShowNewGameMenu() {
        // Show new game menu
        newGameMenu.SetActive(true);
        mainMenuControls.SetActive(false);
    }
    public void LoadGame() {
        worldGenerator.loadGame();
    }

    public void ShowControls() {
        mainMenuControls.SetActive(false);
        controlsDisplay.SetActive(true);
    }

    public void ShowCredits() {
        mainMenuControls.SetActive(false);
        creditsDisplay.SetActive(true);
    }
    public void ShowHighscores() {
        mainMenuControls.SetActive(false);
        highscoresDisplay.SetActive(true);
    }

    public void QuitGame() {
        Debug.Log("Quitting");
        Application.Quit();
    }
    public void ReturnToMainMenu() {
        // Deactivate all other menus and return to main
        highscoresDisplay.SetActive(false);
        controlsDisplay.SetActive(false);
        creditsDisplay.SetActive(false);
        newGameMenu.SetActive(false);
        mainMenuControls.SetActive(true);
    }


}

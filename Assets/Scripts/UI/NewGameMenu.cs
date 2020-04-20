using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameMenu : MonoBehaviour
{

    public GameChanger worldGenerator;

    public TMPro.TMP_InputField usernameInput;
    public GameObject usernameError;
    [SerializeField]
    private bool seedValid = true;

    public TMPro.TMP_InputField seedInput;
    public GameObject seedError;
    [SerializeField]
    private bool usernameValid;

    public Button startButton;

    private int seed;
    private string username;

    private void Start() {
        ValidateName();
        ValidateSeed();
    }
    public void ValidateSeed() {
        seedValid = int.TryParse(seedInput.text, out seed) || seedInput.text.Equals("");
        UpdateStartButton();
    }
    public void ValidateName() {
        string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_";
        username = usernameInput.text;
        usernameValid = username.Length > 0 && chars.Contains(username.Substring(username.Length - 1));

        UpdateStartButton();
        
    }

    void UpdateStartButton() {
        // Update error messages
        usernameError.SetActive(!usernameValid);
        seedError.SetActive(!seedValid);
        // Update start button
        startButton.interactable = seedValid && usernameValid;
    }

    public void StartGame() {
        if (seedInput.text.Length == 0) {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }
        //worldGenerator.newGame();
        worldGenerator.newGame(username, seed);
    }

}

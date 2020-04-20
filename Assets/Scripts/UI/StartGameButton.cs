using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StartGameButton : MonoBehaviour
{

    public GameChanger worldGenerator;
    public Button startButton;

    public InputField usernameInput;
    public InputField seedInput;
    
    void Start()
    {
        print(gameObject.name);
        startButton = GetComponent<Button>();
        startButton.interactable = false;
    }

    void UpdateButton(bool validUsername, bool validSeed) {
        // Unlock button if both seed and username are valid
        startButton.interactable = validUsername && validSeed;
    }

}

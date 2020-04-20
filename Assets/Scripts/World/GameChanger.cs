using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameChanger : MonoBehaviour
{
    public GameObject scoreKeeper;
    public GameObject world;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad (scoreKeeper);
        DontDestroyOnLoad (world);
    }

    public void newGame(string name, int seed){
        DontDestroyOnLoad (scoreKeeper);
        DontDestroyOnLoad (world);
        SceneManager.LoadScene("tilemap1");
        if (seed == 0){
            seed = (int)(Random.value*1000);
        }
        world.GetComponent<World>().seed = seed;
        world.GetComponent<World>().Init();
        scoreKeeper.GetComponent<Score>().onLoadGame();
        if (name.Length < 3){
            name = "unnamed";
        }
        scoreKeeper.GetComponent<Score>().username = name;
    }

    public void loadGame(){
        DontDestroyOnLoad (scoreKeeper);
        DontDestroyOnLoad (world);
        SceneManager.LoadScene("tilemap1");
        world.GetComponent<World>().loadWorld();
        scoreKeeper.GetComponent<Score>().onLoadGame();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralMiniGameController : MonoBehaviour
{
    // Scene loader
    private SceneLoader sceneLoader;

    // Elevator
    public ElevatorController elevator;

    // minigame management variables
    private int score;
    private int timer; // seconds

    // minigame data
    private bool minigameSelected;
    private int minigameCode;
    private int difficulty;

    // number of minigames
    int n_minigames;


    // Start is called before the first frame update
    void Start()
    {
        score = 0; 
        timer = 0;
        minigameSelected = false;
        minigameCode = -1; // no minigame selected
        difficulty = -1; // difficulty not selected

        n_minigames = 0; // 1 is the lobby, as it is considered as a minigame
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* Loads minigame scenes / prefabs into the game scene */
    void LoadMinigames()
    {

    }
    
    // Initializes all necessary data when staring the game
    void InitializeGame()
    {

    }

    void SelectMinigame(int code)
    {
        minigameSelected = true;
        minigameCode = code;
        score = 0; 
        timer = 120; //minigame time
        difficulty = 1;

        elevator.OpenDoor();
    }

    void LoadScenes(){

    }
}

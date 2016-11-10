//  By Kasperi K
//
//  Handles the game starting and ening, points and UI elements

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Set all the texts in the editor
    public Text gameOverText;
    public Text scoreText;
    public Text welcomeText;
    public Text instructionsText;
    public Canvas creditsCanvas;
    public Canvas instructionsCanvas;

    // Grab the buttons and enemySpawn-script
    public EnemySpawn enemySpawn;
    public GameObject startButton;
    public GameObject quitButton;

    // Get the SteamVr controllers
    public SteamVR_TrackedObject rightHand;
    public SteamVR_TrackedObject leftHand;

    private int points;
    private bool isGameOver = false;

    private AudioSource[] allAudioSources;

    void Update()
    {
        // If the game is over, player can start again or quit using the touchpad
        if (isGameOver)
        {
            if(SteamVR_Controller.Input((int)rightHand.index).GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad) /*Input.GetKeyDown(KeyCode.A)*/)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(0);
            }
            if (SteamVR_Controller.Input((int)leftHand.index).GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad) /* Input.GetKeyDown(KeyCode.S)*/)
            {
                Debug.Log("Quit game");
                Application.Quit();
            }
        }
    }

    public void StartGame()
    {
        // Turn off the buttons
        startButton.SetActive(false);
        quitButton.SetActive(false);

        // Disable all texts
        creditsCanvas.enabled = false;
        instructionsCanvas.enabled = false;
        welcomeText.enabled = false;
        instructionsText.enabled = false;

        // Enable enemies to spawn
        enemySpawn.enabled = true;
    }

    public void GameOver()
    {
        isGameOver = true;

        // Stop time and all audio
        Time.timeScale = 0;
        StopAllAudio();

        // Change score text to show right points
        scoreText.text += points;

        // Show texts
        gameOverText.enabled = true;
        scoreText.enabled = true;
        creditsCanvas.enabled = true;
        instructionsCanvas.enabled = true;
    }

    public void AddPoints(int points)
    {
        this.points += points;
    }


    // Stops all audiosources in the scene
    private void StopAllAudio()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }
}

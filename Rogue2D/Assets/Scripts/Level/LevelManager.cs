using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;

    public float waitToLoad = 3f;

    public string nextLevel;

    public bool isPaused = false;
    private AudioManager audioPlayer;
    private PlayerController player;
    private UIController ui;

    private void Awake() 
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = AudioManager.instance;   
        player = PlayerController.instance;
        ui = UIController.instance;

        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    // coroutine
    public IEnumerator LevelEnd()
    {
        audioPlayer.PlayVictory();

        player.isMovable = false;

        ui.StartFadeIn();

        yield return new WaitForSeconds(waitToLoad);

        SceneManager.LoadScene(nextLevel);
    }

    public void PauseUnpause()
    {
        if (!isPaused)
        {
            ui.pauseMenu.SetActive(true);
            isPaused = true;

            // Stop game
            Time.timeScale = 0f;
        }
        else
        {
            ui.pauseMenu.SetActive(false);
            isPaused = false;

            // Start Game
            Time.timeScale = 1f;
        }
    }
}

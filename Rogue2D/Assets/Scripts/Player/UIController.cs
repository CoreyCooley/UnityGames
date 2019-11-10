using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Slider healthSlider;
    public Text healthText;
    public Text coinText;
    public GameObject deathScreen;
    public GameObject pauseMenu;
    public GameObject map;
    public GameObject miniMap;
    public string newGameScene;
    public string mainMenuScene;

    
    // Fading between levels
    public Image fadeScreen;
    public float fadeSpeed = 1;
    private bool fadeIn;
    private bool fadeOut;
    private LevelManager levelManager;

    private void Awake() 
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        fadeOut = true;
        fadeIn = false;
        levelManager = LevelManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(fadeOut)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a,0f, fadeSpeed * Time.deltaTime));
            
            if(fadeScreen.color.a == 0)
                fadeOut = false;
        }

        if(fadeIn)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 0)
                fadeIn = false;
        }
    }

    public void StartFadeOut()
    {
        fadeOut = true;
        fadeIn = false;
    }

    public void StartFadeIn()
    {
        fadeOut = false;
        fadeIn = true;
    }

    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(newGameScene);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Resume()
    {
        levelManager.PauseUnpause();
    }
}

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

    [Header("Weapon UI")]
    public Image primaryWeapon;
    public Text primaryWeaponName;
    public Image secondaryWeapon;
    public Text secondaryWeaponName;

    
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

    public void UpdateWeaponUI(Gun primary, Gun secondary, bool isPrimarySelected)
    {
        if(isPrimarySelected)
        {
            UpdatePrimaryWeaponUI(primary, true);
            UpdateSecondaryWeaponUI(secondary, false);
        }
        else
        {
            UpdatePrimaryWeaponUI(primary, false);
            UpdateSecondaryWeaponUI(secondary, true);
        }

    }

    private void UpdatePrimaryWeaponUI(Gun weapon, bool isSelected)
    {
        if(isSelected)
        {
            primaryWeapon.sprite = weapon.weaponUI;
            primaryWeaponName.text = weapon.weaponName;

            primaryWeapon.color = new Color(1, 1, 1, 1);
            primaryWeaponName.color = new Color(1, 1, 1, 1);
        }
        else
        {
            primaryWeapon.sprite = weapon.weaponUI;
            primaryWeaponName.text = weapon.weaponName;

            primaryWeapon.color = new Color(1, 1, 1, 0.5f);
            primaryWeaponName.color = new Color(1, 1, 1, 0.5f);
        }
    }

    private void UpdateSecondaryWeaponUI(Gun weapon, bool isSelected)
    {
        if (isSelected)
        {
            secondaryWeapon.sprite = weapon.weaponUI;
            secondaryWeaponName.text = weapon.weaponName;

            secondaryWeapon.color = new Color(1, 1, 1, 1);
            secondaryWeaponName.color = new Color(1, 1, 1, 1);
        }
        else
        {
            secondaryWeapon.sprite = weapon.weaponUI;
            secondaryWeaponName.text = weapon.weaponName;

            secondaryWeapon.color = new Color(1, 1, 1, 0.5f);
            secondaryWeaponName.color = new Color(1, 1, 1, 0.5f);
        }
    }
}

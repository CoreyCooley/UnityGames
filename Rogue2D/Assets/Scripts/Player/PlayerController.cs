using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance; // set for all versions of player controller scripts

    public float moveSpeed = 3;
    private float activeMoveSpeed;

    public Transform gunArm;

    public int health = 150;
    public GameObject[] deathSplatters;
    public GameObject hitEffect;

    public SpriteRenderer bodySR;
    public int dashSound = 8;

    private Animator anim;
    private Vector2 moveInput;
    private Rigidbody2D playerRb;
    private Camera playerCam;
    private PlayerHealthController playerHealth;
    private AudioManager audioPlayer;
    private LevelManager levelManager;
    private UIController uiController;
    
    // Dashing
    [Header("Dashing")]
    public float dashSpeed = 8f;
    public float dashLength = .5f;
    public float dashCooldown = 1f;
    public float dashInvincibility = 1.5f;    
    [HideInInspector]
    public float dashCounter;
    private float dashCoolCounter;

    [HideInInspector]
    public bool isMovable = true;

    [Header("Weapons")]
    //public List<Gun> weapons = new List<Gun>();
    public Gun primaryWeapon;
    public Gun secondaryWeapon;
    private int primarySlot = 0;
    private int secondarySlot = 1;
    private int currentWeapon;
    
    // Starts before start
    // This means you only have 1 player in a game
    private void Awake() 
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerCam = Camera.main;
        anim = GetComponent<Animator>();
        playerHealth = PlayerHealthController.instance;
        audioPlayer = AudioManager.instance;
        levelManager = LevelManager.instance;
        uiController = UIController.instance;

        activeMoveSpeed = moveSpeed;
        currentWeapon = 0;

        uiController.UpdateWeaponUI(primaryWeapon, secondaryWeapon, true);
    }

    // Update is called once per frame
    void Update()
    {
        if(isMovable && !levelManager.isPaused)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();

            playerRb.velocity = moveInput * activeMoveSpeed;

            Vector3 mousePos = Input.mousePosition;
            Vector3 playerPos = playerCam.WorldToScreenPoint(transform.localPosition);

            if(mousePos.x < playerPos.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                gunArm.localScale = new Vector3(-1f, -1f, 1f);
            }
            else
            {
                transform.localScale = Vector3.one;
                gunArm.localScale = Vector3.one;
            }

            // Rotate gun arm
            Vector2 offset = new Vector2(mousePos.x - playerPos.x, mousePos.y - playerPos.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            gunArm.rotation = Quaternion.Euler(0, 0, angle);

            // Switch to primary weapon
            if (Input.GetKeyDown(KeyCode.Alpha1) && currentWeapon != primarySlot)
            {
                SwitchWeapon(primarySlot);
            }
            // Switch to secondary weapon
            if (Input.GetKeyDown(KeyCode.Alpha2) && currentWeapon != secondarySlot)
            {
                SwitchWeapon(secondarySlot);
            }

            // Dash
            if(Input.GetKeyDown(KeyCode.Space) )
            {
                if(dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    playerHealth.MakeInvincible(dashInvincibility);
                    activeMoveSpeed = dashSpeed;                
                    dashCounter = dashLength;
                    anim.SetTrigger("dash");
                    audioPlayer.PlaySFX(dashSound);
                }            
            }
            if(dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if(dashCounter <= 0)
                {
                    activeMoveSpeed = moveSpeed;
                    dashCoolCounter = dashCooldown;
                }
            }
            if(dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }

            //Animations
            if(moveInput != Vector2.zero)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }
        else
        {
            playerRb.velocity = Vector2.zero;
            anim.SetBool("isMoving", false);
        }
    }

    public void SwitchWeapon(int slot)
    {
        currentWeapon = slot;

        if(currentWeapon == primarySlot)
        {
            uiController.UpdateWeaponUI(primaryWeapon, secondaryWeapon, true);
            primaryWeapon.gameObject.SetActive(true);
            secondaryWeapon.gameObject.SetActive(false);
        }
        else
        {
            uiController.UpdateWeaponUI(primaryWeapon, secondaryWeapon, false);
            primaryWeapon.gameObject.SetActive(false);
            secondaryWeapon.gameObject.SetActive(true);
        }
    }

    public void PickUpWeapon(Gun weapon)
    {

        Gun newWeapon = Instantiate(weapon);
        newWeapon.transform.parent = gunArm;
        newWeapon.transform.position = gunArm.position;
        newWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
        newWeapon.transform.localScale = Vector3.one;

        if(currentWeapon == primarySlot)
        {
            Destroy(primaryWeapon.gameObject);
            primaryWeapon = newWeapon;
        }
        else
        {
            Destroy(secondaryWeapon.gameObject);
            secondaryWeapon = newWeapon;
        }

        SwitchWeapon(currentWeapon);
    }
}

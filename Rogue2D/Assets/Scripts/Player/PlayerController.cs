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
    public List<Gun> weapons = new List<Gun>();
    private int pistolSlot = 0;
    private int shotgunSlot = 1;
    private int ar15Slot = 2;
    private int revolverSlot = 3;
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

        activeMoveSpeed = moveSpeed;
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

            // Gun Pick
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (weapons.Count > 0)
                {
                    currentWeapon++;
                    if(currentWeapon >= weapons.Count)
                    {
                        currentWeapon = 0;
                    }
                    SwitchWeapon(currentWeapon);
                }
                else
                {
                    Debug.Log("Player has no guns");
                }
            }

            // Gun switch pistol
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchWeapon(pistolSlot);
            }
            // Gun switch shotgun
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchWeapon(shotgunSlot);
            }
            // Gun switch AR-15
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchWeapon(ar15Slot);
            }
            // Gun switch revolver
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SwitchWeapon(revolverSlot);
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
        foreach(Gun gun in weapons)
        {
            gun.gameObject.SetActive(false);
        }
        weapons[slot].gameObject.SetActive(true);
    }
}

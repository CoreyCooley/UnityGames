using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance; // set for all versions of player controller scripts

    public float moveSpeed = 3;
    private float activeMoveSpeed;

    public Transform gunArm;
    public GameObject bulletType;
    public Transform firePoint;
    public int health = 150;
    public GameObject[] deathSplatters;
    public GameObject hitEffect;
    public SpriteRenderer bodySR;
    public int dashSound = 8;
    public int weaponSound = 12;

    private Animator anim;
    private Vector2 moveInput;
    private Rigidbody2D playerRb;
    private Camera playerCam;
    private PlayerHealthController playerHealth;
    private AudioManager audioPlayer;
    private LevelManager levelManager;

    private float fireRate = .25f;
    private float fireCounter;
    
    // Dashing
    public float dashSpeed = 8f;
    public float dashLenght = .5f;
    public float dashCooldown = 1f;
    public float dashInvincibility = 1.5f;
    [HideInInspector]
    public float dashCounter;
    private float dashCoolCounter;

    [HideInInspector]
    public bool isMovable = true;
    
    
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
        fireCounter = fireRate;
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

            // Fire Bullet
            // 0 - Left Mouse Button
            // 1 - Right Mouse Button
            // 2 - Center Mouse Button
            fireCounter -= Time.deltaTime;
            if(Input.GetMouseButtonDown(0))
            {
                if (fireCounter <= 0)
                {
                    audioPlayer.PlaySFX(weaponSound);
                    Instantiate(bulletType, firePoint.position, firePoint.rotation);
                    fireCounter = fireRate;
                }
            }
            if(Input.GetMouseButton(0))
            {
                if(fireCounter <= 0)
                {
                    audioPlayer.PlaySFX(weaponSound);
                    Instantiate(bulletType, firePoint.position, firePoint.rotation);

                    fireCounter = fireRate;
                }

            }

            // Dash
            if(Input.GetKeyDown(KeyCode.Space) )
            {
                if(dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    playerHealth.MakeInvincible(dashInvincibility);
                    activeMoveSpeed = dashSpeed;                
                    dashCounter = dashLenght;
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
}

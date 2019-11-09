using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D enemyRb;
    public int health = 150;

    // Enemy Movement
    [Header("Chase Player")]
    public float moveSpeed = 2.5f;    
    //Chase
    public bool isChasing;
    public float chaseDistance = 7f;
    //Run Away
    [Header("Run Away")]
    public bool shouldRunAway;
    public float runAwayDistance;
    //Wander
    [Header("Wandering")]
    public bool isWandering;    
    public float wanderLength;
    public float pauseLength;
    private Vector3 wanderDirection;
    private float wanderCounter;
    private float pauseCounter;
    private float minMultiple = 0.75f;
    private float maxMultiple = 1.25f;
    //Patrol
    [Header("Patrolling")]
    public bool isPatrolling;
    public Transform[] patrolPoints;
    private int currentPatrolPoint;

    // Effects
    [Header("Effects")]
    public GameObject[] deathSplatters;
    public GameObject hitEffect;

    // Audio
    [Header("Audio")]
    public int hitSound = 2;
    public int deathSound = 1;
    public int weaponSound = 13;

    // Combat System
    [Header("Combat System")]
    public bool isMelee;
    public GameObject bullet;
    public SpriteRenderer body;
    public Transform attackPoint;
    public float attackRate = 1f;
    public float attackRange = 3f;
    private float attackCounter;
    
    private Vector3 moveDirection;
    private GameObject target;
    private Animator anim;
    private PlayerController player;
    private AudioManager audioPlayer;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = PlayerController.instance;
        audioPlayer = AudioManager.instance;

        if(isWandering)
        {
            pauseCounter = Random.Range(pauseLength * minMultiple, pauseLength * maxMultiple);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(body.isVisible && player.gameObject.activeInHierarchy )
        {
            moveDirection = Vector3.zero;

            // Sets Chase direction
            if(Vector3.Distance(transform.position, player.transform.position) <= chaseDistance && isChasing)
            {
                moveDirection = player.transform.position - transform.position;
            }            
            else
            {
                // Wander
                if(isWandering)
                {
                    if(wanderCounter >= 0) //Activing wandering around
                    {
                        wanderCounter -= Time.deltaTime;

                        // Move the enemy
                        moveDirection = wanderDirection;

                        if(wanderCounter <= 0)
                        {
                            pauseCounter = Random.Range(pauseLength * minMultiple, pauseLength * maxMultiple);
                        }
                    }
                    
                    if(pauseCounter >= 0)
                    {
                        pauseCounter -= Time.deltaTime;

                        if(pauseCounter <= 0)
                        {
                            wanderCounter = Random.Range(wanderLength * minMultiple, wanderLength * maxMultiple);

                            wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                        }
                    }
                }

                // Patrol
                if(isPatrolling)
                {
                    moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;

                    if(Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < .2f)
                    {
                        //currentPatrolPoint = Random.Range(0,patrolPoints.Length);

                        currentPatrolPoint++;
                        if(currentPatrolPoint >= patrolPoints.Length)
                            currentPatrolPoint = 0;
                    }
                }
            }

            // Run Away
            if(shouldRunAway && Vector3.Distance(transform.position, player.transform.position) < runAwayDistance)
            {
                moveDirection = transform.position - player.transform.position;
            }

            moveDirection.Normalize();

            enemyRb.velocity = moveDirection * moveSpeed;

            // Ranged Attack
            if(!isMelee && Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                
                attackCounter -= Time.deltaTime;

                if (attackCounter <= 0)
                {
                    audioPlayer.PlaySFX(weaponSound);
                    attackCounter = attackRate;
                    Instantiate(bullet, attackPoint.transform.position, attackPoint.transform.rotation);
                }                
            }
        }
        else
        {
            enemyRb.velocity = Vector2.zero;
        }

        //Animations
        if (moveDirection != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

    }

    public void DamageEnemy(int damage)
    {
        health -= damage;
        
        Instantiate(hitEffect, transform.position, transform.rotation);

        if(health <= 0)
        {
            Destroy(gameObject);
            
            // Get Random Splatter
            int index = Random.Range(0, deathSplatters.Length);
            // Rotation 0, 90, 180, 270
            int rotation = Random.Range(0,4);
            Instantiate(deathSplatters[index], transform.position, Quaternion.Euler(0f,0f, rotation * 90f));
            audioPlayer.PlaySFX(deathSound);
        }
        else
        {
            audioPlayer.PlaySFX(hitSound);
        }
    }
}

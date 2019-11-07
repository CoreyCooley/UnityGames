using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D enemyRb;
    public float moveSpeed = 2.5f;
    public float chaseDistance = 7f;
    public int health = 150;
    public GameObject[] deathSplatters;
    public GameObject hitEffect;
    
    // Combat System
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

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = PlayerController.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(body.isVisible && player.gameObject.activeInHierarchy)
        {
            if(Vector3.Distance(transform.position, player.transform.position) <= chaseDistance)
            {
                moveDirection = player.transform.position - transform.position;
            }
            else
            {
                moveDirection = Vector3.zero;
            }
            moveDirection.Normalize();

            enemyRb.velocity = moveDirection * moveSpeed;

            if(!isMelee && Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                // Shot
                attackCounter -= Time.deltaTime;

                if (attackCounter <= 0)
                {
                    attackCounter = attackRate;
                    Instantiate(bullet, attackPoint.transform.position, attackPoint.transform.rotation);
                }                
            }
            else
            {
                // Melee
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
        }
    }
}

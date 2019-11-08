using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    public float speed = 7.5f;
    public int bulletDamage = 25;
    public GameObject impactEffect;
    public int impactSound = 4;
    
    private Vector3 direction;
    private PlayerController player;
    private PlayerHealthController playerHealth;
    private AudioManager audioPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.instance;
        playerHealth = PlayerHealthController.instance;
        audioPlayer = AudioManager.instance;

        direction = player.transform.position - transform.position;        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
        audioPlayer.PlaySFX(impactSound);

        if (other.tag == "Player")        
            playerHealth.DamagePlayer();
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    public float speed = 7.5f;
    public int bulletDamage = 50;
    public GameObject impactEffect;
    public int impactSound = 4;

    private Rigidbody2D rb2d;
    private AudioManager audioPlayer;    

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        audioPlayer = AudioManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
        audioPlayer.PlaySFX(impactSound);

        if(other.tag == "Enemy")
            other.GetComponent<EnemyController>().DamageEnemy(bulletDamage);
    }

    private void OnBecameInvisible() 
    {
        Destroy(gameObject);
    }
}

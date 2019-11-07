﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private PlayerHealthController playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = PlayerHealthController.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerHealth.DamagePlayer();
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            playerHealth.DamagePlayer();
        }    
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            playerHealth.DamagePlayer();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerHealth.DamagePlayer();
        }
    }
}

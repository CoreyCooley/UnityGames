using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public int currentHealth;
    public int maxHealth;

    public float invincibleLength = 1f;
    public float invincibleCounter;

    private PlayerController player;
    private UIController ui;

    private void Awake() 
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = PlayerController.instance;
        ui = UIController.instance;

        ui.healthSlider.maxValue = maxHealth;
        ui.healthSlider.value = currentHealth;
        ui.healthText.text = currentHealth + " / " + maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(invincibleCounter > 0)
        {
            invincibleCounter -= Time.deltaTime;
        }
        else
        {
            player.bodySR.color = new Color(player.bodySR.color.r, player.bodySR.color.g, player.bodySR.color.b, 1f);
        }
    }


    public void DamagePlayer()
    {
        if (invincibleCounter <= 0)
        {
            Instantiate(player.hitEffect, player.transform.position, player.transform.rotation);
            currentHealth--;
            ui.healthSlider.value = currentHealth;
            ui.healthText.text = currentHealth + " / " + maxHealth;
            invincibleCounter = invincibleLength;

            player.bodySR.color = new Color(player.bodySR.color.r,player.bodySR.color.g,player.bodySR.color.b,0.5f);
        }
        

        if (currentHealth <= 0)
        {
            player.gameObject.SetActive(false);

            ui.deathScreen.SetActive(true);

            // Get Random Splatter
            //int index = Random.Range(0, deathSplatters.Length);
            // Rotation 0, 90, 180, 270
           // int rotation = Random.Range(0, 4);
            //Instantiate(deathSplatters[index], transform.position, Quaternion.Euler(0f, 0f, rotation * 90f));
        }
        
    }

    public void MakeInvincible(float length)
    {
        invincibleCounter = length;

        player.bodySR.color = new Color(player.bodySR.color.r, player.bodySR.color.g, player.bodySR.color.b, 0.5f);
    }
}

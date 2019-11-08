using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{

    public int healAmount = 1;
    public int pickupSound = 7;
    private PlayerHealthController playerHealth;
    private AudioManager audioPlayer;

    public float waitToBeCollected = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = PlayerHealthController.instance;
        audioPlayer = AudioManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(waitToBeCollected > 0)
            waitToBeCollected -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player" && waitToBeCollected <= 0)
        {
            playerHealth.HealPlayer(healAmount);
            Destroy(gameObject);

            audioPlayer.PlaySFX(pickupSound);
        }    
    }
}

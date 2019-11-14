using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Gun weapon;
    
    public int pickupSound = 6;
    private AudioManager audioPlayer;
    private PlayerController player;

    public float waitToBeCollected = 0.25f;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.instance;
        audioPlayer = AudioManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitToBeCollected > 0)
            waitToBeCollected -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && waitToBeCollected <= 0)
        {
            player.PickUpWeapon(weapon);

            Destroy(gameObject);

            audioPlayer.PlaySFX(pickupSound);
        }
    }
}

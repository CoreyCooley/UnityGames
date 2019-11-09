using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1;
    public int pickupSound = 5;
    public float waitToBeCollected = 0.25f;

    private PlayerHealthController playerHealth;
    private AudioManager audioPlayer;
    private LevelManager levelManager;

    private void Awake() {

    }

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = PlayerHealthController.instance;
        audioPlayer = AudioManager.instance;
        levelManager = LevelManager.instance;
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
            levelManager.IncreaseCoins(coinValue);
            Destroy(gameObject);

            audioPlayer.PlaySFX(pickupSound);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{

    public GameObject purchaseMsg;
    public Text boardMsg;

    public int itemCost;
    public int healthUpgradeAmount;
    public string itemDescription;

    // Item Type
    public bool isHealthRestore;
    public bool isHealthUpgrade;
    public bool isWeapon;

    // Shop sounds
    public int buySound;
    public int failedSound;

    private bool inBuyZone;

    private LevelManager levelManager;
    private PlayerHealthController playerHealth;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = LevelManager.instance;
        playerHealth = PlayerHealthController.instance;
        audioManager = AudioManager.instance;

        boardMsg.text = itemDescription + System.Environment.NewLine + "- " + itemCost + " Gold - ";        
    }

    // Update is called once per frame
    void Update()
    {
        purchaseMsg.SetActive(inBuyZone);

        if (inBuyZone)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(levelManager.currentCoins >= itemCost)
                {
                    levelManager.SpendCoins(itemCost);

                    if(isHealthRestore)
                    {
                        playerHealth.HealPlayer(playerHealth.maxHealth);
                    }
                    if (isHealthUpgrade)
                    {
                        playerHealth.IncreaseMaxHealth(healthUpgradeAmount);
                    }
                    if (isWeapon)
                    {
                        // Add weapon to player
                    }

                    gameObject.SetActive(false);
                    inBuyZone = false;
                    audioManager.PlaySFX(buySound);
                }
                else
                {
                    audioManager.PlaySFX(failedSound);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            inBuyZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {            
            inBuyZone = false;
        }
    }
}

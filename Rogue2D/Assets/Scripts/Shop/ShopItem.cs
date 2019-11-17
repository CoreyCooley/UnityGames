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

    // Gun Item
    public Gun[] potentialWeapons;
    private Gun weapon;
    
    public SpriteRenderer itemSprite;

    private bool inBuyZone;

    private LevelManager levelManager;
    private PlayerHealthController playerHealth;
    private AudioManager audioManager;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = LevelManager.instance;
        playerHealth = PlayerHealthController.instance;
        audioManager = AudioManager.instance;
        player = PlayerController.instance;    

        if(isWeapon)
        {
            int selectedWeapon = Random.Range(0, potentialWeapons.Length);
            Debug.Log("Selected Shop Weapon " + selectedWeapon);
            weapon = potentialWeapons[selectedWeapon];

            itemSprite.sprite = weapon.weaponSprite;
            itemDescription = weapon.weaponName;
            itemCost = weapon.weaponCost;
        }

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
                        player.PickUpWeapon(weapon);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChest : MonoBehaviour
{
    public WeaponPickup[] weaponPickups;
    public SpriteRenderer sr;
    public Sprite chestOpen;

    public GameObject notification;
    public Transform spawnPoint;

    // Chest sounds
    public int openSound;
    public int failedSound;

    private bool inChestZone;
    private bool isOpen;
    private int scaleSpeed = 2;


    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (inChestZone && !isOpen)
        {
            
            notification.SetActive(inChestZone);

            if (Input.GetKeyDown(KeyCode.E))
            {
                int weaponSelected = Random.Range(0, weaponPickups.Length);
                Instantiate(weaponPickups[weaponSelected], spawnPoint.position, spawnPoint.rotation);

                sr.sprite = chestOpen;
                audioManager.PlaySFX(openSound);
                isOpen = true;

                transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
        }
        if(isOpen)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, Time.deltaTime * scaleSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inChestZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inChestZone = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletType;
    public Transform[] firePoints;
    public float fireRate = .75f;
    
    public int weaponSound = 12;

    public string weaponName;
    public Sprite weaponUI;

    private AudioManager audioPlayer;
    private PlayerController player;
    private LevelManager levelManager;

    private float fireCounter;

    // Start is called before the first frame update
    void Start()
    {
        fireCounter = fireRate;
        audioPlayer = AudioManager.instance;
        player = PlayerController.instance;
        levelManager = LevelManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isMovable && !levelManager.isPaused)
        {
            if (fireCounter > 0)
            {
                fireCounter -= Time.deltaTime;
            }
            else
            {                
                // Fire Bullet
                // 0 - Left Mouse Button
                // 1 - Right Mouse Button
                // 2 - Center Mouse Button
                // Click or hold down                
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
                {
                    audioPlayer.PlaySFX(weaponSound);    
                    foreach(Transform firePoint in firePoints)
                    {                        
                        Instantiate(bulletType, firePoint.position, firePoint.rotation);
                        fireCounter = fireRate;
                    }
                }
            }
        }
    }
}

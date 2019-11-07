using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    PlayerController player;

    public GameObject[] brokenPieces;
    public int maxPieces;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.tag == "Player Bullet")
        {
            Break();
        }
        
        if(other.tag == "Player")
        {
            if(player.dashCounter > 0)
            {
                Break();                
            }
        }
    }

    private void Break()
    {
        int peicesToDrop = Random.Range(1, maxPieces);

        for (int i = 0; i <= maxPieces; i++)
        {
            int randomPiece = Random.Range(0, brokenPieces.Length);

            Destroy(gameObject);
            Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public bool isLockedOnEnter;
    public bool isLockedOnClear;
    private bool isActiveRoom;

    public GameObject[] doors;

    public List<GameObject> enemies = new List<GameObject>();

    private CameraController gameCamera;

    // Start is called before the first frame update
    void Start()
    {
        gameCamera = CameraController.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (enemies.Count > 0 && isActiveRoom && !isLockedOnClear)
        {
            Debug.Log("Enemy Count: " + enemies.Count);

            for(int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    i--;
                }                    
            }

            if(enemies.Count == 0)
            {
                foreach (GameObject door in doors)
                {
                    Debug.Log("Enemy Count: " + enemies.Count);
                    door.SetActive(false);
                    isLockedOnEnter = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            gameCamera.ChangeTarget(transform);

            if(isLockedOnEnter)
            {
                foreach(GameObject door in doors)
                {
                    door.SetActive(true);
                }
            }

            isActiveRoom = true;
        }    
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player")
        {
            isActiveRoom = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
            isActiveRoom = false; 
    }
}

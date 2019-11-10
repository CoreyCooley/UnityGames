using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public bool isLockedOnEnter;
    private bool isActiveRoom;

    public GameObject[] doors;
    public GameObject mapHider;

    private CameraController gameCamera;

    private void Awake() 
    {
        gameCamera = CameraController.instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            mapHider.SetActive(false);
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

    public bool IsActiveRoom()
    {
        return isActiveRoom;
    }

    public void OpenDoors()
    {        
        foreach (GameObject door in doors)
        {
            door.SetActive(false);
            isLockedOnEnter = false;
        }
    }
}

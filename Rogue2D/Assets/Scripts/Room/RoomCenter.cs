using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{

    public bool isUnlockedOnClear;
    public List<GameObject> enemies = new List<GameObject>();
    public Room room;

    // Start is called before the first frame update
    void Start()
    {
        if(isUnlockedOnClear)
        {
            room.isLockedOnEnter = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(room.IsActiveRoom())
        {
            Debug.Log("In RoomCenter script Update");
            Debug.Log("Enemy Count: " + enemies.Count);
            Debug.Log("Is room active " + room.IsActiveRoom());
            Debug.Log("Is room unlocked on clear" + isUnlockedOnClear);
        }

        if (enemies.Count > 0 && room.IsActiveRoom() && isUnlockedOnClear)
        {
            Debug.Log("Enemy Count: " + enemies.Count);

            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }

            if (enemies.Count == 0)
            {
                room.OpenDoors();
            }
        }
    }
}

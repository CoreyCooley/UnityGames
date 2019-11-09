using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{

    public GameObject layoutRoom;
    public GameObject startRoom;
    public GameObject endRoom;
    public List<GameObject> levelRooms = new List<GameObject>();
    

    public Color startColor;
    public Color endColor;    
    public Transform generatorPoint;

    // Shop
    [Header("Shop")]
    public bool hasShop;
    public GameObject shopRoom;
    public RoomCenter centerShop;
    public Color shopColor;
    private int minDistanceToShop;
    private int maxDistanceToShop;    

    public int roomCount;
    public float xOffset = 18f;
    public float yOffset = 10f;
    public LayerMask roomLayer;

    public enum Direction {up, right, down, left};
    public Direction selectedDirection;

    public RoomPrefabs rooms;
    public List<GameObject> generatedOutlines = new List<GameObject>();

    public RoomCenter centerStart;    
    public RoomCenter[] centerRooms;
    public RoomCenter centerEnd;

    // Start is called before the first frame update
    void Start()
    {
        SpawnLevel();
        
    }

    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR        
        if(Input.GetKey(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
#endif            
    }

    public void SpawnLevel()
    {
        GenerateRooms();

        GenerateRoomOutlines();

        GenerateRoomCenters();
    }

    private void GenerateRooms()
    {
        // Start Room
        startRoom = GenerateRoom(startColor);

        // Basic Rooms
        for (int i = 0; i < roomCount; i++)
        {
            generatorPoint.position = MoveGenerationPoint(generatorPoint.position);
            levelRooms.Add(GenerateRoom(Color.white));
        }

        if(hasShop)
        {        
            // Shop Room
            int shopSelector = Mathf.RoundToInt(Random.Range(roomCount * 0.33f, roomCount * 0.66f));
            shopRoom = levelRooms[shopSelector];
            levelRooms.RemoveAt(shopSelector);
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
        }
        

        // End Room
        generatorPoint.position = MoveGenerationPoint(generatorPoint.position);
        endRoom = GenerateRoom(endColor);
    }

    private GameObject GenerateRoom(Color roomColor)
    {
        GameObject room = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);
        room.GetComponent<SpriteRenderer>().color = roomColor;

        return room;
    }

    private Vector3 MoveGenerationPoint(Vector3 point)
    {
        Vector3 newPoint = point;

        newPoint = NewPoint(newPoint);

        while (IsInvalidPoint(newPoint))
        {
            newPoint = MoveGenerationPoint(newPoint);
        }

        return newPoint;               
    }

    private Vector3 NewPoint(Vector3 point)
    {
        selectedDirection = (Direction)Random.Range(0, 4);

        switch (selectedDirection)
        {
            case Direction.up:
                point += new Vector3(0f, yOffset, 0f);
                break;
            case Direction.right:
                point += new Vector3(xOffset, 0f, 0f);
                break;
            case Direction.down:
                point += new Vector3(0f, -yOffset, 0f);
                break;
            case Direction.left:
                point += new Vector3(-xOffset, 0f, 0f);
                break;
        }

        return point;
    }

    private bool IsInvalidPoint(Vector3 point)
    {
        return Physics2D.OverlapCircle(point, 0.2f, roomLayer);
    }

    private void GenerateRoomOutlines()
    {
        //Start Room
        CreatRoomOutline(startRoom.transform.position);

        // General Rooms
        foreach(GameObject room in levelRooms)
        {
            CreatRoomOutline(room.transform.position);
        }

        // Shop Room
        if(hasShop)
            CreatRoomOutline(shopRoom.transform.position);

        // End Room
        CreatRoomOutline(endRoom.transform.position);
    }

    private void CreatRoomOutline(Vector3 roomPostion)
    {
        bool isRoomAbove = Physics2D.OverlapCircle(roomPostion + new Vector3(0f, yOffset, 0f), 0.2f, roomLayer);
        bool isRoomRight = Physics2D.OverlapCircle(roomPostion + new Vector3(xOffset, 0f, 0f), 0.2f, roomLayer);
        bool isRoomBelow = Physics2D.OverlapCircle(roomPostion + new Vector3(0f, -yOffset, 0f), 0.2f, roomLayer);
        bool isRoomLeft = Physics2D.OverlapCircle(roomPostion + new Vector3(-xOffset, 0f, 0f), 0.2f, roomLayer);

        int directionCount = 0;

        if(isRoomAbove)
            directionCount++;
        if (isRoomRight)
            directionCount++;
        if (isRoomBelow)
            directionCount++;
        if (isRoomLeft)
            directionCount++;

        switch(directionCount)
        {
            case 0:
                Debug.LogError("Found no room exists!!");
                break;
            case 1:
                if(isRoomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleUp, roomPostion, transform.rotation));
                }
                if (isRoomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleRight, roomPostion, transform.rotation));
                }
                if (isRoomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleDown, roomPostion, transform.rotation));
                }
                if (isRoomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPostion, transform.rotation));
                }
                break;
            case 2:
                if(isRoomAbove && isRoomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPostion, transform.rotation));
                }
                if (isRoomAbove && isRoomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPostion, transform.rotation));
                }
                if (isRoomAbove && isRoomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpLeft, roomPostion, transform.rotation));
                }
                if (isRoomRight && isRoomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleRightDown, roomPostion, transform.rotation));
                }
                if (isRoomRight && isRoomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleRightLeft, roomPostion, transform.rotation));
                }
                if (isRoomBelow && isRoomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleDownLeft, roomPostion, transform.rotation));
                }                
                break;
            case 3:
                if (isRoomAbove && isRoomRight && isRoomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpRightDown, roomPostion, transform.rotation));
                }
                if (isRoomAbove && isRoomRight && isRoomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpRightLeft, roomPostion, transform.rotation));
                }
                if (isRoomAbove && isRoomBelow && isRoomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpDownLeft, roomPostion, transform.rotation));
                }
                if (isRoomRight && isRoomBelow && isRoomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleRightDownLeft, roomPostion, transform.rotation));
                }
                break;
            case 4:
                if (isRoomAbove && isRoomRight && isRoomBelow && isRoomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.fourway, roomPostion, transform.rotation));
                }
                break;
        }

    }

    private void GenerateRoomCenters()
    {

        foreach(GameObject outline in generatedOutlines)
        {
            bool isGenerateCenter = true;

            // Start Room
            if(outline.transform.position == startRoom.transform.position)
            {
                Instantiate(centerStart, outline.transform.position, outline.transform.rotation).room = outline.GetComponent<Room>();
                isGenerateCenter = false;
            }

            // Start Room
            if (outline.transform.position == shopRoom.transform.position)
            {
                Instantiate(centerShop, outline.transform.position, outline.transform.rotation).room = outline.GetComponent<Room>();
                isGenerateCenter = false;
            }

            // End Room
            if (outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, outline.transform.rotation).room = outline.GetComponent<Room>();
                isGenerateCenter = false;
            }            

            if(isGenerateCenter)
            {
                int selectCenter = Random.Range(0, centerRooms.Length);

                Instantiate(centerRooms[selectCenter], outline.transform.position, outline.transform.rotation).room = outline.GetComponent<Room>();
            }
        }

    }

}

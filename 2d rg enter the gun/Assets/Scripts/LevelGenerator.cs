using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public GameObject layoutRoom; //room we use to layout generated level
    public Color startColor, endColor, shopColor, gunRoomColor; //rooms colors

    public int distanceToEnd; //how many rooms we have before exit room
    public bool includeShop;
    public int minDinstanceToShop, maxDistanceToShop;
    public bool includeGunRoom;
    public int minDistanceToGunRoom, maxDistanceToGunRoom;


    public Transform generatorPoint; //startPoint

    public enum Direction { up, right, down, left }; //jakby stworzenie nowego rodzaju zmiennej   0 1 2 3
    public Direction selectedDirection;

    private int xOffset = 76, yOffset = 40; //how much we want to move on x and y  //18 12
    private int XLxOffset = 27, XLyOffset = 15; //how much we want to move on x and y //27 15

    public LayerMask whatIsRoom;

    GameObject endRoom, shopRoom, gunRoom;

    List<GameObject> layoutRoomObjects = new List<GameObject>();

    public RoomPrefabs rooms;

    List<GameObject> generatedOutlines = new List<GameObject>();

    public RoomCenter centerStart, centerEnd, centerShop, centerGunRoom;
    //public RoomCenter[] potentialCenters;

    public bool generateRoom;


    public RoomCenter[] roomCenterSingleUp, roomCentersingleDown, roomCentersingleRight, roomCentersingleLeft,
        roomCenterdoubleUpDown, roomCenterdoubleLeftRight, roomCenterdoubleUpRight, roomCenterdoubleRightDown, roomCenterdoubleDownLeft, roomCenterdoubleLeftUp,
        roomCentertripleUpRightDown, roomCentertripleRightDownLeft, roomCentertripleDownLeftUp, roomCentertripleLeftUpRight,
        roomCenterfourway;//XL

    List<RoomCenter[]> roomCenterCurrent = new List<RoomCenter[]>();

    void Start()
    {

        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startColor; //ustawiam bialy pokoj start zmieniam kolor 

        selectedDirection = (Direction)Random.Range(0, 4); //castowanie na Direction



        MoveGenerationPoint();

        for (int i = 0; i < distanceToEnd; i++)
        {

            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

            layoutRoomObjects.Add(newRoom);

            if (i + 1 == distanceToEnd)//jezeli to ostatnia iteracja petli
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);

                endRoom = newRoom;
            }

            selectedDirection = (Direction)Random.Range(0, 4); //castowanie na Direction
            MoveGenerationPoint();

            while (Physics2D.OverlapCircle(generatorPoint.position, 0.2f, whatIsRoom)) //creates invisible circle 
            {
                MoveGenerationPoint();
            }



        }

        if (includeShop)
        {
            int shopSelector = Random.Range(minDinstanceToShop, maxDistanceToShop + 1); //+1 cause exclusive
            shopRoom = layoutRoomObjects[shopSelector];
            //Debug.LogWarning("Shop: " + shopSelector);
            layoutRoomObjects.RemoveAt(shopSelector);
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
        }

        if (includeGunRoom)
        {
            //earlier was error cause I was trying to acces layourRoomObjects at gunRoomSelector and if 
            //it was hgigher than layoutRoomObjects.Count error
            //can do same on shop
            int max = maxDistanceToGunRoom + 1 >= layoutRoomObjects.Count ? layoutRoomObjects.Count : maxDistanceToGunRoom + 1;
            int gunRoomSelector = UnityEngine.Random.Range(minDistanceToGunRoom, max);

            gunRoom = layoutRoomObjects[gunRoomSelector];
            layoutRoomObjects.RemoveAt(gunRoomSelector);
            gunRoom.GetComponent<SpriteRenderer>().color = gunRoomColor;
        }

        //create room outlines///////////////////////////////////////////////////

        CreateRoomOutline(Vector3.zero);


        foreach (GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }


        CreateRoomOutline(endRoom.transform.position);
        if (includeShop)
        {
            CreateRoomOutline(shopRoom.transform.position);
        }
        if (includeGunRoom)
        {
            CreateRoomOutline(gunRoom.transform.position);
        }
        int l = 0;
        foreach (GameObject outline in generatedOutlines)
        {
            if (outline.transform.position == Vector3.zero)
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
            else if (outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
            else if (includeShop && outline.transform.position == shopRoom.transform.position)
            {
                Instantiate(centerShop, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
            else if (includeGunRoom && outline.transform.position == gunRoom.transform.position)
            {
                Instantiate(centerGunRoom, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
            else
            {
                //list[1].array[2];
                int centerSelect = Random.Range(0, roomCenterCurrent[l].Length);
                                                                             //instantiate gdzie jest outline ze loopuje przez wszystkie to kazdemu zrobi
                Instantiate(roomCenterCurrent[l][centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                //Debug.LogWarning("XL room " + potentialCentersXL[centerSelect].gameObject.transform.localScale + " pos: " + outline.transform.position);
            }

           

            l++;
        }
    }

    void Update()
    {
        //for testing
#if UNITY_EDITOR //this will only run if we are in unity editor
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); //loads open scene
            Destroy(GameObject.FindGameObjectWithTag("Player"));
        }

#endif
    }

    public void MoveGenerationPoint()
    {
        switch (selectedDirection) //dziala jak 4 if'y
        {
            case Direction.up: //case 0
                {
                    generatorPoint.position += new Vector3(0f, yOffset, 0f);
                    break;
                }
            case Direction.down: //case 2
                {
                    generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                    break;
                }
            case Direction.right: //case 1
                {
                    generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                    break;
                }
            case Direction.left: //case 3
                {
                    generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                    break;
                }

        }
    }
    
    public void CreateRoomOutline(Vector3 roomPosition)
    {

        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), 0.2f, whatIsRoom);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), 0.2f, whatIsRoom);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), 0.2f, whatIsRoom);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), 0.2f, whatIsRoom);
        //XL

        int directionCount = 0;


        if (roomAbove)
        {
            directionCount++;
        }
        if (roomBelow)
        {
            directionCount++;
        }
        if (roomLeft)
        {
            directionCount++;
        }
        if (roomRight)
        {
            directionCount++;
        }


        switch (directionCount)
        {
            case 0:
                Debug.LogError("Found no room exists!");
                break;

            case 1:

                //XL
                if (roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleUpXL, roomPosition, transform.rotation));
                    roomCenterCurrent.Add(roomCenterSingleUp);

                }
                if (roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleDownXL, roomPosition, transform.rotation));
                    roomCenterCurrent.Add(roomCentersingleDown);
                }
                if (roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleLeftXL, roomPosition, transform.rotation));
                    roomCenterCurrent.Add(roomCentersingleLeft);
                }
                if (roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleRightXL, roomPosition, transform.rotation));
                    roomCenterCurrent.Add(roomCentersingleRight);
                }

                break;

            case 2:

                //XL

                if (roomAbove && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpDownXL, roomPosition, transform.rotation));
                    roomCenterCurrent.Add(roomCenterdoubleUpDown);

                }
                if (roomLeft && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftRightXL, roomPosition, transform.rotation));
                    roomCenterCurrent.Add(roomCenterdoubleLeftRight);

                }
                if (roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpRightXL, roomPosition, transform.rotation));
                    roomCenterCurrent.Add(roomCenterdoubleUpRight);

                }
                if (roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleRightDownXL, roomPosition, transform.rotation));
                    roomCenterCurrent.Add(roomCenterdoubleRightDown);

                }
                if (roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleDownLeftXL, roomPosition, transform.rotation));
                    roomCenterCurrent.Add(roomCenterdoubleDownLeft);

                }
                if (roomLeft && roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftUpXL, roomPosition, transform.rotation));
                    roomCenterCurrent.Add(roomCenterdoubleLeftUp);

                }


                break;


            case 3:
                //XL
                if (roomAbove && roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpRightDownXL, roomPosition, transform.rotation));
                    roomCenterCurrent.Add(roomCentertripleUpRightDown);

                }
                if (roomRight && roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleRightDownLeftXL, roomPosition, transform.rotation));
                    roomCenterCurrent.Add(roomCentertripleRightDownLeft);

                }
                if (roomBelow && roomLeft && roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleDownLeftUpXL, roomPosition, transform.rotation));
                    roomCenterCurrent.Add(roomCentertripleDownLeftUp);

                }
                if (roomLeft && roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleLeftUpRightXL, roomPosition, transform.rotation));
                    roomCenterCurrent.Add(roomCentertripleLeftUpRight);

                }

                break;

            case 4:

                //XL
                if (roomBelow && roomLeft && roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.fourwayXL, roomPosition, transform.rotation));
                    roomCenterCurrent.Add(roomCenterfourway);


                }

                break;
        }
    }
}

[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleUp, singleDown, singleRight, singleLeft,
        doubleUpDown, doubleLeftRight, doubleUpRight, doubleRightDown, doubleDownLeft, doubleLeftUp,
        tripleUpRightDown, tripleRightDownLeft, tripleDownLeftUp, tripleLeftUpRight,
        fourway,//XL
        singleUpXL, singleDownXL, singleRightXL, singleLeftXL,
        doubleUpDownXL, doubleLeftRightXL, doubleUpRightXL, doubleRightDownXL, doubleDownLeftXL, doubleLeftUpXL,
        tripleUpRightDownXL, tripleRightDownLeftXL, tripleDownLeftUpXL, tripleLeftUpRightXL,
        fourwayXL;
}
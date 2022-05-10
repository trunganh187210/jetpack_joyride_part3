using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour
{

    public GameObject[] availableRooms;
    public List<GameObject> currentRooms;

    private float screenWidthInPoints;

    public GameObject[] availaleObjects;
    public List<GameObject> objects;

    public float objectMaxDistance = 10.0f;
    public float objectMinDistance = 5.0f;

    public float objMaxY = 1.4f;
    public float objMinY = -1.4f;

    public float objectMinRotation = -45.0f;
    public float objectMaxRotation = 45.0f;

    // Start is called before the first frame update
    void Start()
    {
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;
        StartCoroutine(GenaratorCheck());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddRoom(float farthestRoomEndX)
    {
        //fatherestRoomEndX is the right most point of the level
        int randomRoomIndex = Random.Range(0, availableRooms.Length); //random [min;max)
        GameObject room = (GameObject)Instantiate(availableRooms[randomRoomIndex]);
        float roomWidth = room.transform.Find("Floor").localScale.x; //the floor's size = room's width
        float roomCenter = farthestRoomEndX + roomWidth * 0.5f; //to calculate where the center of the new room should be
        room.transform.position = new Vector3(roomCenter, 0, 0);
        currentRooms.Add(room);
    }

    void GenarateRoomIfRequired()
    {
        List<GameObject> roomToRemove = new List<GameObject>();
        bool addRoom = true;
        float playerX = transform.position.x;
        float removeRoomX = playerX - screenWidthInPoints; //the point where the room should be removed
        float addRoomX = playerX + screenWidthInPoints; // the point where the new room should be added
        float farthestRoomEndX = 0;
        foreach(var room in currentRooms)
        {
            float roomWidth = room.transform.Find("Floor").localScale.x;
            float roomStartX = room.transform.position.x - (roomWidth * 0.5f);
            float roomEndX = roomStartX + roomWidth;

            if(roomStartX > addRoomX)
            {
                addRoom = false;
            }

            if(roomEndX < removeRoomX)
            {
                roomToRemove.Add(room);
            }

            //find the newest rightest point
            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
        }

        //remove passed rooms
        foreach(var room in roomToRemove)
        {
            currentRooms.Remove(room);
            Destroy(room);
        }

        //add new rooms
        if (addRoom)
        {
            AddRoom(farthestRoomEndX);
        }

    }

    IEnumerator GenaratorCheck()
    {
        while (true)
        {
            GenarateRoomIfRequired();
            GenerateObjectIfRequired();
            yield return new WaitForSeconds(0.25f);
        }
    }

    void AddObject(float lastObjectX)
    {
        int randomIndex = Random.Range(0, availaleObjects.Length);
        GameObject obj = (GameObject)Instantiate(availaleObjects[randomIndex]);
        float objectPositionX = lastObjectX + Random.Range(objectMinDistance, objectMaxDistance); //x of the new object
        float objectPositionY = Random.Range(objMinY, objMaxY); //y of the new object
        obj.transform.position = new Vector3(objectPositionX, objectPositionY, 0);
        float rotation = Random.Range(objectMinRotation, objectMaxRotation);
        obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation); //random rotation
        objects.Add(obj);
    }

    void GenerateObjectIfRequired()
    {
        float playerX = transform.position.x;
        float removeObjectX = playerX - screenWidthInPoints;
        float addObjectX = playerX + screenWidthInPoints;
        float farthestObjectX = 0.0f;

        List<GameObject> objectsToRemove = new List<GameObject>();
        foreach(var obj in objects)
        {
            float objX = obj.transform.position.x;
            farthestObjectX = Mathf.Max(farthestObjectX, objX);

            if(objX < removeObjectX)
            {
                objectsToRemove.Add(obj);
            }
        }

        foreach(var obj in objectsToRemove)
        {
            objects.Remove(obj);
            Destroy(obj);
        }

        if(farthestObjectX < addObjectX)
        {
            AddObject(farthestObjectX);
        }
    }
}

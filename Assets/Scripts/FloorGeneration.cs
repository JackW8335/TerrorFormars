using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGeneration : MonoBehaviour {

    public int minNumOfRooms;
    public int maxNumOfRooms;
    public int roomsToBeMade = 0;
    public int currentNumOfRooms = 0;
    public GameObject[] rooms;
    public GameObject room;
	// Use this for initialization
	void Start () {
        Random.seed = Random.Range(0, 1000);
        roomsToBeMade = Random.Range(minNumOfRooms, maxNumOfRooms);     //each floor will have a random amount of rooms
        rooms = new GameObject[roomsToBeMade];  //this will assign the cave or tunnel prefabs based on conditions
        for (int i = 0; i < roomsToBeMade; i++)
        {
            AddRoomToFloor();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void AddRoomToFloor()
    {
        Instantiate(room, this.transform);  //position of room will have to be calculated for transform
    }
}

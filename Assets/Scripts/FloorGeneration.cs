using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGeneration : MonoBehaviour {

    public int maxNumOfRooms;
    public int minNumOfRooms;
    public int currentNumOfRooms = 0;
    public GameObject[] rooms;
	// Use this for initialization
	void Start () {
        currentNumOfRooms = 3;  //just for testing
        rooms = new GameObject[currentNumOfRooms];  //this will assign the cave or tunnel prefabs based on conditions
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGeneration : MonoBehaviour {

    public GameObject roomPrefab;
    public bool isNextRoomDeadEnd = false;
	// Use this for initialization
	void Start () {
        //Decide first how many exits this room will have, then based on answer, set what roomPrefab should be
        Instantiate(roomPrefab, this.transform);    //test - This will need to know the correct prefab to assign and the correct position
        //Next set chance of the next room being a dead end
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

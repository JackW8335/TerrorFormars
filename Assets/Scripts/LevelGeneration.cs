using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {

    public int floorCount;
    public GameObject floorObj;
	// Use this for initialization
	void Start () {
		for (int i = 0; i < floorCount; i++)
        {
            AddFloorToMap();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void AddFloorToMap()
    {
        Instantiate(floorObj, this.transform);  //position of level will have to be calculated here
    }
}

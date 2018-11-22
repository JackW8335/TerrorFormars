using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{


    public int numFloors = 3;
    public int MaxObj = 11;
    int floorObjCount = 0;

    public GameObject[] caves;
    public GameObject[] startObjects;

    public GameObject[] tunnels;
    List<GameObject> currentRoute;


    private void Start()
    {
        currentRoute = new List<GameObject>();
        
        MainGeneration();
    }

    void MainGeneration()
    {
        for (int i = 0; i < numFloors; i++)
        {
            GenerateFloor(i);
        }
    }

    void GenerateFloor(int FloorNum)
    {
        GameObject FloorStart = PlaceObject(FloorNum, startObjects[FloorNum], true);
        currentRoute.Add(FloorStart);
        //gerate all tunnels off first object

        for (int i = 0; i < FloorStart.transform.childCount; i++)
        {
            Transform caveChild = FloorStart.transform.GetChild(i);
            if (caveChild.tag == "Door")
            {
                if (FloorNum != 0 && i == 0)
                {
                    continue;
                }
                Vector3 startPos = new Vector3(0, 0, 0);
                    Vector3 nextRot = new Vector3(0, 0, 0);

                    GameObject tunnel = tunnels[0];
                    Vector3 currentStartPoint = tunnel.transform.GetChild(0).position;
                    Vector3 currentEndPoint = currentEndPoint = tunnel.transform.GetChild(1).position;

                    float R = Vector3.Distance(currentStartPoint, currentEndPoint) / 2;
                    float spawnPos = caveChild.position.x + R;

                    switch (i)
                    {
                        case 0:
                            nextRot = new Vector3(0, 90, 0);
                            spawnPos = caveChild.position.x - R;
                            startPos = new Vector3(spawnPos, caveChild.position.y, FloorStart.transform.position.z);
                            break;
                        case 1:
                            nextRot = new Vector3(0, 0, 0);
                            spawnPos = caveChild.position.z - R;
                            startPos = new Vector3(FloorStart.transform.position.x, caveChild.position.y, spawnPos);
                            break;
                        case 2:
                            nextRot = new Vector3(0, 0, 0);
                            spawnPos = caveChild.position.z + R;
                            startPos = new Vector3(FloorStart.transform.position.x, caveChild.position.y, spawnPos);
                            break;
                        case 3:
                            spawnPos = caveChild.position.x + R;
                            nextRot = new Vector3(0, 90, 0);
                            startPos = new Vector3(spawnPos, caveChild.position.y, FloorStart.transform.position.z);
                            break;
                    }

                    GameObject nextObj = Instantiate(tunnel, startPos, Quaternion.identity);
                    nextObj.transform.parent = transform;
                    nextObj.transform.eulerAngles = nextRot;
            }
        }
    }

     GameObject PlaceObject(int nextObjID, GameObject nextObjPrefab, bool firstCave)
     {
         Vector3 startPos = new Vector3(0, 0, 0);
         Vector3 LastEnd = new Vector3(0, 0, 0);
         Vector3 nextRot = new Vector3(0, 90, 0);
         Vector3 currentStartPoint = new Vector3(0, 0, 0);
         Vector3 currentEndPoint = new Vector3(0, 0, 0);

        if (currentRoute.Count -1 >= 0)
        {
            currentStartPoint = nextObjPrefab.transform.GetChild(0).position;

            GameObject lastPrefab = currentRoute[currentRoute.Count - 1];
            if (firstCave)
            {
                LastEnd = lastPrefab.transform.GetChild(3).GetChild(1).position;
                currentEndPoint = nextObjPrefab.transform.GetChild(3).GetChild(0).position;               
            }
            else 
            {
                LastEnd = lastPrefab.transform.GetChild(0).position;
                currentEndPoint = nextObjPrefab.transform.GetChild(3).position;
            }

            nextRot = lastPrefab.transform.eulerAngles;
            //Edge of last spawned piece
            float lastPrefabEdge = LastEnd.x;

        
            // calculaion to get spawn postion

            float R = Vector3.Distance(currentStartPoint, currentEndPoint) / 2;
            float spawnPos = LastEnd.x + R;
            startPos = new Vector3(spawnPos, LastEnd.y, 0);

            
        }
         GameObject nextObj = Instantiate(nextObjPrefab, startPos, Quaternion.identity);
         nextObj.transform.parent = transform;
         nextObj.transform.eulerAngles = nextRot;
         return nextObj;
     }
}

/*public int floorCount;
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
        Instantiate(floorObj, this.transform);
    }

    */

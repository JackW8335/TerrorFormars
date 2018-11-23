using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{


    public int numFloors = 3;
    public int MaxObj = 11;

     int branchMax = 10;
     int branchMin = 0;

    public GameObject[] caves;
    public GameObject[] startObjects;

    public GameObject[] tunnels;
    List<GameObject> fullCaveRoute;
    List<GameObject> currentFloor;
    private readonly float ModelRadius = 4.9965f;


    private void Start()
    {
        fullCaveRoute = new List<GameObject>();
        currentFloor = new List<GameObject>();


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
        GameObject FloorStart = PlaceStartRoom(FloorNum, startObjects[FloorNum], true);
        currentFloor.Clear();
        fullCaveRoute.Add(FloorStart);
        currentFloor.Add(FloorStart);

        //gerate all tunnels off first object
        branchOffCave(FloorNum, FloorStart);
    }

    void branchOffCave(int FloorNum,GameObject Cave)
    {

        for (int i = 0; i < Cave.transform.childCount; i++)
        {
            Transform caveDoor = Cave.transform.GetChild(i);
            if (caveDoor.tag == "openDoor")
            {
                if (FloorNum != 0 && i == 0)
                {
                    continue;
                }
                int branchLength = Random.Range(branchMin, branchMax);               

                GenerateBranch(branchLength, i, tunnels[0], caveDoor);
                caveDoor.tag = "closedDoor";
            }
        }
    }

    void GenerateBranch(int branchLength, int doorNum, GameObject StartObject, Transform StartDoor)
    {
        GameObject nextObject = StartObject;
        Transform Door = StartDoor;
        for (int i = 0; i < branchLength; i++)
        {
            Vector3 Spawn = new Vector3(0, 0, 0);
            Spawn = placeObject(doorNum,Door);

            if (canPlace(Spawn))
            {
                GameObject newObject = Instantiate(nextObject, Spawn, Quaternion.identity);
                newObject.transform.parent = transform;
                newObject.transform.eulerAngles = RotateObject(doorNum);

                fullCaveRoute.Add(newObject);
                currentFloor.Add(newObject);

                //if (currentFloor[currentFloor.Count - 1].tag == "cave")
                //{
                Door = newObject.transform.GetChild(1);// get exit
                nextObject = tunnels[0];
               // }
                //else
                //{
                //    nextObject = pickObjectToMake();
                //}
            }
        }
    }

    GameObject pickObjectToMake()
    {
        float chance = Random.Range(0, 1);
        if (chance > 0.3)
        {
            return tunnels[0];
        }
        return caves[0];
    }


    Vector3 placeObject(int doorNum,Transform Door)
    {
        Vector3 Spawn = new Vector3(0, 0, 0);
        float spawnOffset = 0.0f;

        switch (doorNum)
        {
            //Bottom
            case 0:
                spawnOffset = Door.position.x - ModelRadius;
                Spawn = new Vector3(spawnOffset, Door.position.y, Door.position.z);
                break;
            //Right
            case 1:
                spawnOffset = Door.position.z - ModelRadius;
                Spawn = new Vector3(Door.position.x, Door.position.y, spawnOffset);
                break;
            //Left
            case 2:
                spawnOffset = Door.position.z + ModelRadius;
                Spawn = new Vector3(Door.position.x, Door.position.y, spawnOffset);
                break;
            //Top
            case 3:
                spawnOffset = Door.position.x + ModelRadius;
                Spawn = new Vector3(spawnOffset, Door.position.y, Door.position.z);
                break;
        }
        return Spawn;
    }
    Vector3 RotateObject(int doorNum)
    {
        Vector3 nextRot = new Vector3(0, 0, 0);
        switch (doorNum)
        {
            //Bottom
            case 0:
                nextRot = new Vector3(0, 270, 0);
                break;
            //Right
            case 1:
                nextRot = new Vector3(0, 180, 0);
                break;
            //Left
            case 2:
                nextRot = new Vector3(0, 0, 0);
               break;
            //Top
            case 3:
                nextRot = new Vector3(0, 90, 0);
               break;
        }
        return nextRot;
    }



    bool canPlace(Vector3 nwObj)
    {
        foreach(GameObject obj in currentFloor)
        {
            if(obj.transform.position == nwObj)
            {
                return false;
            }
        }
        return true;
    }

     GameObject PlaceStartRoom(int nextObjID, GameObject nextObjPrefab, bool firstCave)
     {
         Vector3 startPos = new Vector3(0, 0, 0);
         Vector3 LastEnd = new Vector3(0, 0, 0);
         Vector3 nextRot = new Vector3(0, 90, 0);
         Vector3 currentStartPoint = new Vector3(0, 0, 0);
         Vector3 currentEndPoint = new Vector3(0, 0, 0);

        if (fullCaveRoute.Count -1 >= 0)
        {
            GameObject lastPrefab;
            if (firstCave)
            {
                lastPrefab = fullCaveRoute[fullCaveRoute.Count - currentFloor.Count];
                LastEnd = lastPrefab.transform.GetChild(3).GetChild(1).position;
            }
            else 
            {
                lastPrefab = fullCaveRoute[fullCaveRoute.Count - 1];
                LastEnd = lastPrefab.transform.GetChild(0).position;
            }

            nextRot = lastPrefab.transform.eulerAngles;
            //Edge of last spawned piece
            float lastPrefabEdge = LastEnd.x;

            float spawnPos = LastEnd.x + ModelRadius;
            startPos = new Vector3(spawnPos, LastEnd.y, 0);     
        }
         GameObject nextObj = Instantiate(nextObjPrefab, startPos, Quaternion.identity);
         nextObj.transform.parent = transform;
         nextObj.transform.eulerAngles = nextRot;
         return nextObj;
     }


}
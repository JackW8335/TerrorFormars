﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{


    public int numFloors = 3;
    public int MaxObj = 11;

     int branchMax = 4;
     int branchMin = 0;

    public GameObject[] caves;
    public GameObject[] startObjects;

    public GameObject[] tunnels;
    public GameObject deadEnd;
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
        StartCoroutine(branchOffCave(FloorNum, FloorStart));
    }

    IEnumerator branchOffCave(int FloorNum, GameObject Cave)
    {

        for (int i = 0; i < Cave.transform.childCount; i++)
        {
            Transform caveDoor = Cave.transform.GetChild(i);
            if (caveDoor.tag == "openDoor")
            {
                if (FloorNum != 0 && i == 0)// this wont work any more
                {
                    continue;
                }
                int branchLength = Random.Range(branchMin, branchMax);               

                GenerateBranch(FloorNum,branchLength, i, tunnels[0], caveDoor);
            }
        }
        yield return null;
    }

    void GenerateBranch(int FloorNum,int branchLength, int doorNum, GameObject StartObject, Transform StartDoor)
    {
        GameObject nextObject = StartObject;
        Transform Door = StartDoor;
        Door.tag = "closedDoor";
        for (int i = 0; i <= branchLength; i++)
        {
            Vector3 Spawn = new Vector3(0, 0, 0);
            Spawn = placeObject(doorNum,Door);

            if (canPlace(Spawn))//&& currentFloor.Count < MaxObj
            {
                GameObject newObject = Instantiate(nextObject, Spawn, Quaternion.identity);
                newObject.transform.parent = transform;
                newObject.transform.eulerAngles = Door.eulerAngles;
                newObject.transform.GetChild(0).tag = "closedDoor";

                fullCaveRoute.Add(newObject);
                currentFloor.Add(newObject);


                if (newObject.tag == "cave")
                {
                    Door = newObject.transform.GetChild(3);
                    Door.tag = "closedDoor";
                    nextObject = tunnels[0];
                    StartCoroutine(branchOffCave(FloorNum, newObject));
                }
                else
                {
                    Door = newObject.transform.GetChild(1);// get exit
                    Door.tag = "closedDoor";
                    nextObject = pickObjectToMake();
                }
            }
            if(i == branchLength)
            {
                GameObject DeadEnd = Instantiate(deadEnd, Door.position, Quaternion.identity);
                DeadEnd.transform.parent = transform;
                DeadEnd.transform.eulerAngles = Door.eulerAngles;
            }
        }

    }

    GameObject pickObjectToMake()
    {
        float chance = Random.Range(0.0f,1.0f);
        if (chance > 0.3)
        {
            return tunnels[0];
        }
        return caves[0];
    }



    Vector3 placeObject(int doorNum, Transform Door)
    {

        //needs to know which way they want to be placed and from that give back the right direction
        Vector3 Spawn = new Vector3(0, 0, 0);
        float spawnOffset = 0.0f;
        switch ((int)Door.eulerAngles.y)
        {
            //Bottom
            case 90:
                spawnOffset = Door.position.x + ModelRadius;
                Spawn = new Vector3(spawnOffset, Door.parent.position.y, Door.parent.position.z);
                break;
            //Right
            case 180:
                spawnOffset = Door.position.z - ModelRadius;
                Spawn = new Vector3(Door.parent.position.x, Door.parent.position.y, spawnOffset);
                break;
            //Left
            case 270:
                spawnOffset = Door.position.x - ModelRadius;
                Spawn = new Vector3(spawnOffset, Door.parent.position.y, Door.parent.position.z);
                break;
            //Top
            case 0:
                spawnOffset = Door.position.z + ModelRadius;
                Spawn = new Vector3(Door.parent.position.x, Door.parent.position.y, spawnOffset);
                break;
        }
        
        return Spawn;
    }
    Vector3 RotateObject(int doorNum,Transform lastBranchTransform)
    {
        Vector3 nextRot = lastBranchTransform.eulerAngles;

        switch (doorNum)
        {
            //Bottom
            case 0:
                nextRot.y = lastBranchTransform.eulerAngles.y + 180 ;
                break;
            //Right
            case 1:
                nextRot.y = lastBranchTransform.eulerAngles.y + 90;
                break;
            //Left
            case 2:
                nextRot.y = lastBranchTransform.eulerAngles.y + 270;
                break;
            //Top
            case 3:
                nextRot.y = lastBranchTransform.eulerAngles.y;
                break;
        }
        return nextRot;
       /*
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
        */
        
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
         Vector3 nextRot = new Vector3(0, 0, 0);

        if (fullCaveRoute.Count -1 >= 0)
        {
            GameObject lastPrefab;
            
            lastPrefab = fullCaveRoute[fullCaveRoute.Count - currentFloor.Count];
            LastEnd = lastPrefab.transform.GetChild(3).GetChild(1).position;
            
            

            nextRot = lastPrefab.transform.eulerAngles;
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
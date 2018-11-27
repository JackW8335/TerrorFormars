﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcdeuralGeneration : MonoBehaviour
{
    public GameObject[] caves;

    public GameObject[] tunnels;

    public GameObject[] transitionObjects;

    List<GameObject> currentRoute;

    private float spawnPos = 0;

    private int tunelGap = 3;

    public int generationSize = 15;
    public int floorCount = 1;

    bool generateTunel = false;


    // Use this for initialization
    void Start ()
    {
        currentRoute = new List<GameObject>();
       
        MainGeneration();
	}
	
	void MainGeneration()
    {
        // First loop through number of floors
        //then for each floor, run the generateRooms function
        //for (int i = 0; i < floorCount; i++)
        //{
        //    generateRooms();
        //}

        int tunelChance = 0;

        for (int i = 0; i < generationSize; i++)
        {
            //Reset tunelchance to 0 if a tunel is spawned
            if (TunelChance(tunelChance))
            {
                tunelChance = 0;
            }
            else
            {
                tunelChance++;
            }

            //Generate cave or tunel
            if (!generateTunel)
            {
                currentRoute.Add(PlaceObject(i, caves[Random.Range(0, caves.Length)]));
            }
            else
            {
                currentRoute.Add(PlaceObject(i, tunnels[Random.Range(0, tunnels.Length)]));

            }
        }

    }

    GameObject PlaceObject(int nextObjID, GameObject nextObjPrefab)
    {
        Vector3 nextPrefabPos = new Vector3(0, 0, 0);
        Quaternion nextRot = new Quaternion(0, 0, 0, 0); 
        spawnPos = 0;

        if (nextObjID > 0)
        {
            GameObject lastPrefab = currentRoute[nextObjID - 1];

            Vector3 LastEnd = lastPrefab.transform.GetChild(1).transform.position;
            nextRot = lastPrefab.transform.rotation;

            //Edge of last spawned piece
            float lastPrefabEdge = LastEnd.x;

            Vector3 currentStartPoint = nextObjPrefab.transform.GetChild(0).transform.position;
            Vector3 currentEndPoint = nextObjPrefab.transform.GetChild(1).transform.position;

            // calculaion to get spawn postion

            float R = Vector3.Distance(currentStartPoint, currentEndPoint)/2;
            spawnPos = LastEnd.x + R;
        }

        GameObject nextObj = Instantiate(nextObjPrefab, new Vector3(spawnPos, 0, 0), Quaternion.identity);
        nextObj.transform.parent = transform;
        nextObj.transform.eulerAngles = new Vector3(0, 90, 0);
        generateTunel = false;
        return nextObj;
    }


    bool TunelChance(int chance)
    {
        //Uses a percentage chance essentially to choose whether to generate a tunel
        //Gets higher chance after each generation 

        Random.seed = Random.Range(0, 1000);
        float randValue = Random.Range(0, 100);
     
        //No chance
        if (chance == 0)
        {
            return false;
        }
        //10% chance
        else if(chance == 1)
        {
            if(randValue < 30)
            {
                generateTunel = true;
                return true;
            }
        }
        //40% chance
        else if (chance == 2)
        {
            if (randValue < 40)
            {
                generateTunel = true;
                return true;
            }
        }
        //Guaranteed chance
        else if(chance == 3)
        {
            generateTunel = true;
            return true;
        }


        return false;
    }

}

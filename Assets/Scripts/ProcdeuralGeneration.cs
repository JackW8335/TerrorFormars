using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcdeuralGeneration : MonoBehaviour
{
    public GameObject[] caves;

    public GameObject[] tunels;

    List<GameObject> currentRoute;

    public Vector3 currentPrefabPos;
    private float spawnPos = 0;

    private int tunelGap = 3;

    public int generationSize = 15;

    bool generateTunel = false;
    //private Vector3 nextPrefabPos;


    // Use this for initialization
    void Start ()
    {
        currentPrefabPos = new Vector3(0, 0, 0);
        currentRoute = new List<GameObject>();
        // nextPrefabPos = new Vector3(0, 0, 0);
       
        MainGeneration();
	}
	
	void MainGeneration()
    {

        Random.seed = Random.Range(0, 1000);
        int tunelChance = 0;

        for (int i = 0; i < generationSize; i++)
        {
            //Reset tunelchance to 0 if a tunel is spawned
            if(TunelChance(tunelChance))
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
                currentRoute.Add(GenerateCave(i, Random.Range(0, 3)));
            }
            else
            {
                currentRoute.Add(GenerateCave(i, Random.Range(0, 2)));
                
            }
        }

    }

    GameObject GenerateCave(int routePos, int size)
    {
       
        Vector3 nextPrefabPos = new Vector3(0, 0, 0);

        //BoxCollider prefabColl = caves[size].GetComponent<BoxCollider>();
        //nextPrefabPos.x += (prefabColl.size.x * caves[size].transform.localScale.x);

        if (routePos > 0)
        {
            BoxCollider prefabColl;

            GameObject lastPrefab = currentRoute[routePos-1];

            BoxCollider lastPrefabColl = lastPrefab.GetComponent<BoxCollider>();

            //Check between tunel or cave piece
            if (!generateTunel)
            {
                prefabColl = caves[size].GetComponent<BoxCollider>();
            }
            else
            {
                prefabColl = tunels[size].GetComponent<BoxCollider>();
            }

            //Edge of last spawned piece
            float lastPrefabEdge = lastPrefab.transform.position.x + ((lastPrefabColl.size.x * lastPrefab.transform.localScale.x) / 2);

            //If cave get cave size, else get tunel size for spawnpos
            if (!generateTunel)
            {
                spawnPos = lastPrefabEdge + ((prefabColl.size.x * caves[size].transform.localScale.x) / 2);
            }
            else
            {
                spawnPos = lastPrefabEdge + ((prefabColl.size.x * tunels[size].transform.localScale.x) / 2);
            }

            Debug.Log("fuck you alex");
        }




        if (!generateTunel)
        {
            GameObject cave = Instantiate(caves[size], new Vector3(spawnPos, 0, 0), Quaternion.identity);
            cave.transform.parent = transform;
            return cave;
        }
        else
        {
            GameObject tunel = Instantiate(tunels[size], new Vector3(spawnPos, 0, 0), Quaternion.identity);
            tunel.transform.parent = transform;
            generateTunel = false;
            return tunel;
        }

       
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
            if(randValue < 10)
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

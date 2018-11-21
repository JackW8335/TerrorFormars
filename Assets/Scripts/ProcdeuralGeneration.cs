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
            //if(TunelChance(tunelChance))
            //{
            //    tunelChance = 0;
            //}
            //else
            //{
            //    tunelChance++;
            //}

            //Generate cave or tunel
            if (!generateTunel)
            {
                currentRoute.Add(GenerateCave(i, 0 /*Random.Range(0, caves.Length)*/));
            }
            else
            {
                currentRoute.Add(GenerateCave(i, 0 /*Random.Range(0, 1)*/));
                
            }
        }

    }

    GameObject GenerateCave(int routePos, int size)
    {
       
        Vector3 nextPrefabPos = new Vector3(0, 0, 0);

       
        Quaternion nextRot = new Quaternion(0, 0, 0, 0);

        Vector3 startTrans = new Vector3(0, 0, 0);
        Vector3 endTrans = new Vector3(0,0,0);
        spawnPos = 0;

        //BoxCollider prefabColl = caves[size].GetComponent<BoxCollider>();
        //nextPrefabPos.x += (prefabColl.size.x * caves[size].transform.localScale.x);

       


        if (routePos > 0)
        {
            GameObject lastPrefab = currentRoute[routePos - 1];

            // BoxCollider prefabColl;
            foreach (Transform child in lastPrefab.transform)
            {
                if (child.CompareTag("StartPos"))
                {
                    startTrans = child.transform.position;
                    Debug.Log("StartTrans ="+ startTrans);
                }
                if (child.CompareTag("EndPos"))
                {
                    endTrans = child.transform.position;
                    Debug.Log("EndTrans =" + endTrans);
                }
            }

            //BoxCollider lastPrefabColl = lastPrefab.GetComponent<BoxCollider>();

            nextRot = lastPrefab.transform.rotation;

            

            //Edge of last spawned piece
            //float lastPrefabEdge = lastPrefab.transform.position.x + ((lastPrefabColl.size.x * lastPrefab.transform.localScale.x) / 2);

            float lastPrefabEdge = endTrans.x;
            Debug.Log(routePos);
            Debug.Log(lastPrefabEdge);
            float currentEndPoint = 0;
            float currentStartPoint = 0;

            //If cave get cave size, else get tunel size for spawnpos
            if (!generateTunel)
            {
                //spawnPos = lastPrefabEdge + ((prefabColl.size.x * caves[size].transform.localScale.x) / 2);
                
                foreach (Transform child in caves[size].transform)
                {
                    if (child.CompareTag("StartPos"))
                    {
                        currentStartPoint = child.transform.position.x;
                        //Debug.Log("EndTrans =" + endTrans);
                    }
                    if (child.CompareTag("EndPos"))
                    {
                        currentEndPoint = child.transform.position.x;
                        //Debug.Log("EndTrans =" + endTrans);
                    }
                }
            }


            else
            {
                
                foreach (Transform child in tunels[size].transform)
                {
                    if (child.CompareTag("StartPos"))
                    {
                        currentStartPoint = child.transform.position.x;
                        //Debug.Log("EndTrans =" + endTrans);
                    }
                    if (child.CompareTag("EndPos"))
                    {
                    currentEndPoint = child.transform.position.x;
                    //Debug.Log("EndTrans =" + endTrans);
                    }
                }           
            }
            //shitty magic number
            float math = ((currentStartPoint * 1.8f) + (currentEndPoint * 1.8f)) /2;
           // math *= 2;
            //spawnPos = currentStartPoint + math;
            spawnPos = lastPrefabEdge + math;
            
            //Debug.Log("fuck you alex");
        }


        Debug.Log("Spawn Pos" + routePos + " " + spawnPos);

        if (!generateTunel)
        {
            GameObject cave = Instantiate(caves[size], new Vector3(spawnPos, 0, 0), Quaternion.identity);
            cave.transform.parent = transform;
            cave.transform.eulerAngles = new Vector3(0, 90, 0);

            return cave;
        }
        else
        {
            GameObject tunel = Instantiate(tunels[size], new Vector3(spawnPos, 0, 0), Quaternion.identity);
            tunel.transform.parent = transform;
            tunel.transform.eulerAngles = new Vector3(0, 90, 0);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcdeuralGeneration : MonoBehaviour
{
    public GameObject[] caves;

    public GameObject[] tunels;

    List<GameObject> currantRoute;

    public Vector3 currentPrefabPos;
    float prefabEdge = 0;
    //private Vector3 nextPrefabPos;


    // Use this for initialization
    void Start ()
    {
        currentPrefabPos = new Vector3(0, 0, 0);
        currantRoute = new List<GameObject>();
       // nextPrefabPos = new Vector3(0, 0, 0);
        MainGeneration();
	}
	
	void MainGeneration()
    {
        for(int i = 0; i < 15; i++)
        {

            currantRoute.Add(GenerateCave(i,Random.Range(0, 2)));

        }

    }

    GameObject GenerateCave(int routePos,int size)
    {
       
        Vector3 nextPrefabPos = new Vector3(0, 0, 0);

        //BoxCollider prefabColl = caves[size].GetComponent<BoxCollider>();
        //nextPrefabPos.x += (prefabColl.size.x * caves[size].transform.localScale.x);

        if (routePos > 0)
        {
            GameObject lastPrefab = currantRoute[routePos-1];

            BoxCollider lastPrefabColl = lastPrefab.GetComponent<BoxCollider>();

            BoxCollider prefabColl = caves[size].GetComponent<BoxCollider>();

            float lastPrefabEdge = lastPrefab.transform.position.x + ((lastPrefabColl.size.x * lastPrefab.transform.localScale.x) / 2);//(lastPrefab.transform.position.x * lastPrefab.transform.localScale.x) + (prefabColl.size.x * caves[size].transform.localScale.x);

            prefabEdge = lastPrefabEdge + ((prefabColl.size.x * caves[size].transform.localScale.x)/2);//x + current cube width / 2;


            Debug.Log("fuck you alex");
        }


        GameObject cave = Instantiate(caves[size], new Vector3(prefabEdge,0,0), Quaternion.identity);

        return cave;
    }

    void GenerateTunel(int size, Vector3 position)
    {
        GameObject tunel = Instantiate(tunels[size], position, Quaternion.identity);
        //nextPrefabPos.x += tunel.GetComponent<Collider>().bounds.size.x;
    }

}

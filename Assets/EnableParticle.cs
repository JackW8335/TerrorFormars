using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableParticle : MonoBehaviour
{
    public GameObject particles;
    public GameObject throwObj;
    private GameObject player;
    private Transform location;
    private bool able;
    // Use this for initialization

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        location = GameObject.FindWithTag("Hand").transform;
        able = player.GetComponent<Player>().canCarry;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && able)
        {
            player.GetComponent<Player>().canCarry = false;
            GameObject tank = Instantiate(throwObj, location);
            tank.transform.parent = location;

            GameObject effect = Instantiate(particles, gameObject.transform.position, Quaternion.identity);


            findClosetTankToPlayer().gameObject.GetComponent<RespawnTank>().Deactive();

        }
    }

    Transform findClosetTankToPlayer()
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        for(int i =0; i < transform.childCount;i++)
        {
            Transform t = transform.GetChild(i);
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }
}

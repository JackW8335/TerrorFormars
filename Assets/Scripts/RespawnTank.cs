using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTank : MonoBehaviour {
    Vector3 TanksSpawnPostion;
    bool deactiveated;
	// Use this for initialization
	void Start () {
        TanksSpawnPostion = this.transform.position;
        deactiveated = false;
    }
	
	// Update is called once per frame
	void Update () {
		if(deactiveated)
        {
            deactiveated = false;
            StartCoroutine(WaitAndRespwan(50.0f));
        }
	}
    
    public void Deactive()
    {
        deactiveated = true;
        this.GetComponent<CapsuleCollider>().enabled = false;
        for(int i =0; i < transform.childCount;i++)
        {
            transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
        }
    }

    IEnumerator WaitAndRespwan(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        reActive();
    }

    void reActive()
    {
        this.transform.position = TanksSpawnPostion;
        this.GetComponent<CapsuleCollider>().enabled = true;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
        }
    }
}

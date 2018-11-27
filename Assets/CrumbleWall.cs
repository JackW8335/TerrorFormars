using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumbleWall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(WaitAndDestroy(5.0f));
	}

    IEnumerator WaitAndDestroy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(this);
    }
}

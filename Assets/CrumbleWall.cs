using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumbleWall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(this.transform.position.y < 0.0f)
        {
            Destroy(this);
        }
	}
}

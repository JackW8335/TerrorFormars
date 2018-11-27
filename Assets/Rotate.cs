using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

	
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.rotation = new Quaternion(0, 300, 0,1);
	}
}

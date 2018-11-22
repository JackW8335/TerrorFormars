using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenThrow : MonoBehaviour
{
    Rigidbody rb;
	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            Destroy(this);
        }
    }
    
}

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
        rb.AddRelativeForce(new Vector3(0,0,5));
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            Destroy(this);
        }
    }
    
}

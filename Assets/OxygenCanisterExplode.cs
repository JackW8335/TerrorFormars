using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenCanisterExplode : MonoBehaviour {

    public GameObject explosion;
    private GameObject player;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider thing)
    {
        if (this.GetComponent<Rigidbody>() != null)
        {
            if (thing != GameObject.FindGameObjectWithTag("Player"))
            {
                Debug.Log(thing.name);
                Instantiate(explosion, this.transform).transform.parent = null;
                Destroy(this.gameObject);
            }
        }
    }
}

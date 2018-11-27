using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenCanisterExplode : MonoBehaviour {

    public GameObject explosion;
    private GameObject player;
    public GameObject newWall;

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
            if (thing.tag == "Blockable" || thing.tag == "cave" || thing.tag == "tunnel" || thing.tag == "OutSideBlocker")//(thing != GameObject.FindGameObjectWithTag("Player"))
            {
                Debug.Log(thing.name);
                Instantiate(explosion, this.transform).transform.parent = null;
                Destroy(this.gameObject);
            }
            if (thing.tag == "Blockable")
            {
                thing.gameObject.SetActive(false);
                GameObject crumbleWall = Instantiate(newWall, thing.transform.position, Quaternion.identity);
                crumbleWall.transform.eulerAngles = thing.transform.eulerAngles;
                Destroy(thing);
            }
        }

    }
}

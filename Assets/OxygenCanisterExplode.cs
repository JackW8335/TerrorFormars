using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenCanisterExplode : MonoBehaviour {

    public GameObject explosion;
    private GameObject player;
    public GameObject newWall;
    public GUITexture guiTexture;
	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindWithTag("Player");
        guiTexture = GetComponent<GUITexture>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnTriggerStay(Collider thing)
    {
        if (this.GetComponent<Rigidbody>() != null)
        {
            if (thing.tag == "Blockable" || thing.tag == "cave" || thing.tag == "tunnel")//(thing != GameObject.FindGameObjectWithTag("Player"))
            {
                Debug.Log(thing.name);
                Instantiate(explosion, this.transform).transform.parent = null;
                //thing.gameObject.GetComponent<RespawnTank>().Deactive();
                Destroy(this.gameObject);
            }
            if (thing.tag == "Blockable")
            {
                //other.gameObject.SetActive(false);
                thing.gameObject.SetActive(false);
                GameObject crumbleWall = Instantiate(newWall, thing.transform.position, Quaternion.identity);
                crumbleWall.transform.eulerAngles = thing.transform.eulerAngles;
                Destroy(thing);
                StartCoroutine(Fade(0, 1f, .1f));
            }
        }
    }
    IEnumerator Fade(float start, float end, float length)
    {
        Color col = guiTexture.color;
        for (float i = 0f; i < 10.0f; i += Time.deltaTime*(1/length))
        {
            Debug.Log("Start fade");
            col.a = Mathf.Lerp(start, end, i);
            //guiTexture.color = col;
        }
        yield return null;
        //white screen fade
        //change to win scene
    }
}

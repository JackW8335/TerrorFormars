using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oxygen : MonoBehaviour {

    public Sprite[] sprites;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        float oxygen = GameObject.Find("Special Big Sister").GetComponent<Player>().GetOxygen();

        if (oxygen > 80)
        {
           GetComponent<Image>().sprite = sprites[0];
        }
        else if (oxygen > 60)
        {
            GetComponent<Image>().sprite = sprites[1];
        }
        else if (oxygen > 40)
        {
            GetComponent<Image>().sprite = sprites[2];
        }
        else if (oxygen > 20)
        {
            GetComponent<Image>().sprite = sprites[3];
        }
        else if (oxygen > 0)
        {
            GetComponent<Image>().sprite = sprites[4];
        }
        else if (oxygen == 0)
        {
            GetComponent<Image>().sprite = sprites[5];
        }
    }
}

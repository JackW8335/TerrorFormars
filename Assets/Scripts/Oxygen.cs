using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oxygen : MonoBehaviour {

    private Material m;

    public void Start()
    {
        m = GetComponent<MeshRenderer>().material; //Getting the Material
    }



    // Update is called once per frame
    void Update ()
    {
        float oxygen = GameObject.Find("Special Big Sister").GetComponent<Player>().GetOxygen();

        if (oxygen > 80)
        {
            m.SetFloat("_Threshold", 1.0f); //Setting _Threshold in the shader to healthValue
        }
        else if (oxygen > 60)
        {
            m.SetFloat("_Threshold", 0.8f);
        }
        else if (oxygen > 40)
        {
            m.SetFloat("_Threshold", 0.6f);
        }
        else if (oxygen > 20)
        {
            m.SetFloat("_Threshold", 0.4f);
        }
        else if (oxygen > 0)
        {
            m.SetFloat("_Threshold", 0.2f);
        }
        else if (oxygen == 0)
        {
            m.SetFloat("_Threshold", 0.0f);
        }
    }
}

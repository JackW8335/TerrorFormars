using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class Oxygen : MonoBehaviour {

    private Material m;
    private VignetteModel.Settings vigenetteSettings;
    public PostProcessingProfile profile;
    public void Start()
    {
        m = GetComponent<MeshRenderer>().material; //Getting the Material

        vigenetteSettings = profile.vignette.settings;

        vigenetteSettings.intensity = 0.0f;
    }

    // Update is called once per frame
    void Update ()
    {
        float oxygen = GameObject.Find("Special Big Sister").GetComponent<Player>().GetOxygen();
       

        if (oxygen > 80)
        {
            if(vigenetteSettings.intensity < 0.0)
                vigenetteSettings.intensity += 0.0025f;


            m.SetFloat("_Threshold", 1.0f); //Setting _Threshold in the shader to healthValue
        }
        else if (oxygen > 60)
        {
            if (vigenetteSettings.intensity < 0.2)
                vigenetteSettings.intensity += 0.0025f;

            m.SetFloat("_Threshold", 0.8f);
        }
        else if (oxygen > 40)
        {
            if (vigenetteSettings.intensity < 0.4)
                vigenetteSettings.intensity += 0.0025f;

            m.SetFloat("_Threshold", 0.6f);
        }
        else if (oxygen > 20)
        {
            if (vigenetteSettings.intensity < 0.6)
                vigenetteSettings.intensity += 0.0025f;

            m.SetFloat("_Threshold", 0.4f);
        }
        else if (oxygen > 0)
        {
            if (vigenetteSettings.intensity < 0.8)
                vigenetteSettings.intensity += 0.0025f;

            m.SetFloat("_Threshold", 0.2f);
        }
        else if (oxygen == 0)
        {
            if (vigenetteSettings.intensity < 1.0)
                vigenetteSettings.intensity += 0.0025f;

            m.SetFloat("_Threshold", 0.0f);
        }

        profile.vignette.settings = vigenetteSettings;
    }
}

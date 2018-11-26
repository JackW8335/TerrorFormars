using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class Oxygen : MonoBehaviour {

    private Material m;
    private VignetteModel.Settings vignetteSettings;
    public PostProcessingProfile profile;
    public void Start()
    {
        m = GetComponent<MeshRenderer>().material; //Getting the Material

        vignetteSettings = profile.vignette.settings;

        vignetteSettings.intensity = 0.0f;
    }

    // Update is called once per frame
    void Update ()
    {
        decreaseOxygenBar();
    }
    void decreaseOxygenBar()
    {
        float oxygen = GameObject.Find("Player").GetComponent<Player>().oxygen;
        //if (oxygen > 80)
        //{
        //    if (vignetteSettings.intensity < 0.0)
        //        vignetteSettings.intensity += 0.0025f;

        //    m.SetFloat("_Threshold", 1.0f); //Setting _Threshold in the shader to healthValue
        //}
        //else if (oxygen > 60)
        //{
        //    //if (vignetteSettings.intensity < 0.2)
        //    //    vignetteSettings.intensity += 0.0025f;

        //    m.SetFloat("_Threshold", 0.8f);
        //}
        //else if (oxygen > 40)
        //{
        //    //if (vignetteSettings.intensity < 0.4)
        //    //    vignetteSettings.intensity += 0.0025f;

        //    m.SetFloat("_Threshold", 0.6f);
        //}
        //else if (oxygen > 20)
        //{
        //    //if (vignetteSettings.intensity < 0.6)
        //    //    vignetteSettings.intensity += 0.0025f;

        //    m.SetFloat("_Threshold", 0.4f);
        //}
        //else if (oxygen > 0)
        //{
        //    //if (vignetteSettings.intensity < 0.8)
        //    //    vignetteSettings.intensity += 0.0025f;

        //    m.SetFloat("_Threshold", 0.2f);
        //}
        //else if (oxygen == 0)
        //{
        //    //if (vignetteSettings.intensity < 1.0)
        //    //    vignetteSettings.intensity += 0.0025f;

        //    m.SetFloat("_Threshold", 0.0f);
        //}
        m.SetFloat("_Threshold", oxygen/100);
        vignetteSettings.intensity = (100.0f - oxygen) / 100;
        profile.vignette.settings = vignetteSettings;
    }

}

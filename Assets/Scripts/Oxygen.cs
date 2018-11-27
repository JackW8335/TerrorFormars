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

        m.SetFloat("_Threshold", oxygen/100);
        vignetteSettings.intensity = (100.0f - oxygen) / 100;
        profile.vignette.settings = vignetteSettings;
    }

}

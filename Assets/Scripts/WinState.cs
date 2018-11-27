using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinState : MonoBehaviour {

    public enum states{ PLAYING,VICTORY, DEFEAT};
    private float fadeCounter = 0;
    private GameObject panel;

    public states state;
    void Start()
    {
        state= states.PLAYING;
        panel = GameObject.Find("Panel");
    }
    void Update()
    {
        if(fadeCounter <= 1.0f)
        {
            FadeOut();
        }
        else if(state == states.VICTORY)
        {
            SceneManager.LoadScene("Victory");
        }
        else if(state == states.DEFEAT)
        {
            SceneManager.LoadScene("GameOver");
        }

    }

    void FadeOut()
    {
        if (state != states.PLAYING)
        {
            fadeCounter += 0.01f;
        }
    }
}

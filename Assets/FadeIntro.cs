using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FadeIntro : MonoBehaviour {

    public Text text1;
    public Text text2;
    public float time = 1.0f;
    private int turn = 0;

    // Use this for initialization
    void Start ()
    {
        text1.color = new Color(text1.color.r, text1.color.g, text1.color.b, 0);
        text2.color = new Color(text2.color.r, text2.color.g, text2.color.b, 0);

        StartCoroutine("cutscene");
    }
	
	// Update is called once per frame
	void Update ()
    {
	}

    IEnumerator fadeTextIn(Text text)
    {
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / time));
            yield return null;
        }
        turn++;
        StartCoroutine("cutscene");
    }

    IEnumerator fadeTextOut(Text text)
    {
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / time));
            yield return null;
        }
        turn++;
        StartCoroutine("cutscene");
    }

    IEnumerator cutscene()
    {
        switch (turn)
        {
            case 0:
                {
                    StartCoroutine("fadeTextIn", text1);
                    break;
                }
            case 1:
                {
                    StartCoroutine("fadeTextOut", text1);
                    break;
                }
            case 2:
                {
                    StartCoroutine("fadeTextIn", text2);
                    break;
                }
            case 3:
                {
                    StartCoroutine("fadeTextOut", text2);
                    break;
                }
            case 4:
                {
                    SceneManager.LoadScene("WaterTest");
                    break;
                }
        }
        yield return null;
    }
}

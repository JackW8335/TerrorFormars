using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FateCredits : MonoBehaviour {

    public Text t;
    public float time = 1.0f;
    private int turn = 0;

    // Use this for initialization
    void Start ()
    {
        t.color = new Color(t.color.r, t.color.g, t.color.b, 0);

        StartCoroutine("fadeTextIn", t);
        // StartCoroutine("cutscene");
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Taunt"))
        {
            SceneManager.LoadScene("MainMenu");
        }

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
                    StartCoroutine("fadeTextIn", t);
                    break;
                }
            case 1:
                {
                    StartCoroutine("fadeTextOut", t);
                    break;
                }
            case 2:
                {
                    SceneManager.LoadScene("MainMenu");
                    break;
                }
        }
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEndGame : MonoBehaviour
{
    private WinState win;
    private void Start()
    {
        win = gameObject.GetComponent<WinState>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Throwable")
        {
            win.state = WinState.states.VICTORY;
        }
    }
}

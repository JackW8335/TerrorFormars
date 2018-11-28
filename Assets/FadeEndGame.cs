using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEndGame : MonoBehaviour
{
    private WinState win;
    public GameObject EndExplosion;

    private void Start()
    {
        win = gameObject.GetComponent<WinState>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Throwable")
        {
            Instantiate(EndExplosion, collision.transform);
            win.state = WinState.states.VICTORY;
        }
    }
}

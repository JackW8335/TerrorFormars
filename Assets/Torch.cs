using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public float flicker = 0.1f;
    public float flickerSpeed = 1.0f;
    private float waitTime = 0;
    private Light torchLight;
    private Player player;

    // Use this for initialization
    private void Start()
    {
        torchLight = GetComponent<Light>();
        player = GetComponentInParent<Player>();
    }


    // Update is called once per frame
    void FixedUpdate ()
    {
        //Torch will start to flash when oxygen is low
        if (player.oxygen < 40)
        {

            waitTime += Time.deltaTime;

            if (waitTime >= Random.Range(15, 25))
            {
                StartCoroutine(FlickeringOn());
                waitTime = 0;
            }

        }

    }

    private IEnumerator FlickeringOn()
    {
        torchLight.enabled = false;
        yield return new WaitForSeconds(Random.Range(flicker, flickerSpeed));
        torchLight.enabled = true;
    }

}

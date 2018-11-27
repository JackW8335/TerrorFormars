using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public float flicker = 0.1f;
    public float flickerSpeed = 1.0f;
    private float waitTime = 0;
    private Light torchLight;

    // Use this for initialization
    private void Start()
    {
        torchLight = GetComponent<Light>();
    }


    // Update is called once per frame
    void FixedUpdate ()
    {
        waitTime += Time.deltaTime;

        if (waitTime >= Random.Range(15, 30))
        {
            StartCoroutine(FlickeringOn());
            waitTime = 0;
        }

        

    }

    private IEnumerator FlickeringOn()
    {
        torchLight.enabled = false;
        yield return new WaitForSeconds(Random.Range(flicker, flickerSpeed));
        torchLight.enabled = true;
    }

}

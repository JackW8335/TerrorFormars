using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableParticle : MonoBehaviour
{
    public GameObject particles;
    // Use this for initialization

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameObject effect = Instantiate(particles, gameObject.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}

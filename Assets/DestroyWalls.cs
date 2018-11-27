using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWalls : MonoBehaviour
{

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Throwable")
        {
            Destroy(this.gameObject);
        }
    }
}

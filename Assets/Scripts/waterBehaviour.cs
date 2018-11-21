using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterBehaviour : MonoBehaviour {
    public float waterRiseingRate = 0.5f;
    public GameObject playerObj;
    private Player Player;
    float max;
    public float HeightTolearnce;
    // Use this for initialization
    void Start () {
        Player = playerObj.GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {

        RaiseWaterLevel();
    }
    void RaiseWaterLevel()
    {
        max = Player.getYPos() + HeightTolearnce;

        Mathf.Clamp(transform.position.y, 0.0f, max);
        if (transform.position.y < max)
        {
            this.transform.Translate(new Vector3(0.0f, waterRiseingRate * Time.deltaTime, 0.0f));
        }
    }
}

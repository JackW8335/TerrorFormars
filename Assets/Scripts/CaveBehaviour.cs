using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveBehaviour : MonoBehaviour
{
    public enum HazardType { NONE = 0, WATER = 1, LAVA = 2, GAS = 3, SAND = 4};
    //Type of hazard for room
    public HazardType  currentHazard = HazardType.NONE;

    //Oxygen pickup in room?
    private bool oxygenRoom = false;
    //Will the room have hazards?

    //Hazard already in room or will it spawn in after some time?
    private bool instantHazard = false;

    // Use this for initialization
    void Start ()
    {
        GenerateBehaviour();
	}
	
	void GenerateBehaviour()
    {
        HazardCheck();
        CaveType();
    }

    void HazardCheck()
    {
        float randValue = Random.Range(0, 100);

        if(randValue < 25)
        {
            HazardCreation();
        }
        else
        {
            return;
        }
    }

    void HazardCreation()
    {
        Random.seed = Random.Range(0, 1000);
        float randValue = Random.Range(0, 100);

        /* 25% chance to be one of the hazards, need to check somehow what the 
        last hazard was so the same one doesnt keep coming up
        Would maybe need to use instances of caves and have a HazardType return 
        function to get the type from the last cave Instance*/
        if (randValue <= 25)
        {
            currentHazard = HazardType.WATER;
        }
        else if(randValue <=50 && randValue > 25)
        {
            currentHazard = HazardType.LAVA;
        }
        else if (randValue <= 75 && randValue > 50)
        {
            currentHazard = HazardType.GAS;
        }
        else if (randValue <= 100 && randValue > 75)
        {
            currentHazard = HazardType.SAND;
        }

        //Spawn in hazardtype here, not sure how yet


    }

    void CaveType()
    {
        //Logic for whether oxygen should be in the room
        //Not sure how to decide on this yet
    }
}

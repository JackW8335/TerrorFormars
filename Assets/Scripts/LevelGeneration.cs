using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{

    public int Max;
    public int Min;

    public int branchMax = 3;
    public int branchMin = 1;

    public float tunnelChance = 0.5f;
    public GameObject cave;
    public GameObject tunnel;
    public GameObject deadEnd;
    const float ModelRadius = 4.9965f;

    public int maxBreakAble;
    public float BreakAbleChance = 0.6f;
    int currentBreakAble;
    public GameObject breakAbleWall;

    public int maxTanks;
    public float tankChance = 0.6f;
    int currentTanks;
    public GameObject oxygenTank;

    public int maxPlacementToExit;
    public int minPlacementToExit;
    public float exitChance = 0.6f; // how quickly to place a tunnel 
    bool exitPlaced;
    public GameObject exit;//blueprint
    GameObject ExitObject;//actual

    List<GameObject> CaveSystem;


    private void Start()
    {
        CaveSystem = new List<GameObject>();
        InilizeGenereation();
    }

    public void InilizeGenereation()
    {
        exitPlaced = false;
        CaveSystem.Clear();
        currentTanks = 0;
        currentBreakAble = 0;
        MainGeneration();
       // spawnAllTanks();
        EndAllPaths();
        if (CaveSystem.Count < Min)
        {
            InilizeGenereation();
        }

        while(!exitPlaced)
        {
            // make sure there is always an exit 
            makeSureISExit();
        }
        //Debug.Log("had exit not placed first time arund");
       
    }

    void makeSureISExit()
    {
        int j = 0;
        foreach (GameObject obj in CaveSystem)
        {
            if (!exitPlaced)
            {
                j++;
               
                if (obj.tag == "OutSideBlocker")
                {
                    float chance = Random.Range(0.0f, 1.0f);
                    if (j >= minPlacementToExit && j <= maxPlacementToExit && exitChance > chance)
                    {
                        createExit(obj.transform);
                    }
                }
            }
            else
            {
                return;
            }
        }
    }

#region generate floor
    void MainGeneration()
    {

        GameObject FloorStart = Instantiate(cave, new Vector3(0, 0, 0), Quaternion.identity);
        FloorStart.transform.parent = transform;
        FloorStart.transform.eulerAngles = new Vector3(0, 0, 0);

        currentTanks = 0;
        CaveSystem.Add(FloorStart);
        spawnTank(FloorStart.transform, true);
        //gerate all tunnels off first object
        StartCoroutine(branchOffCave(FloorStart));
    }
#endregion

    #region genarete branchs 
    IEnumerator branchOffCave(GameObject Cave)
    {
        for (int i = 0; i < Cave.transform.childCount; i++)
        {
            Transform caveDoor = Cave.transform.GetChild(i);

            if (caveDoor.tag == "openDoor" && CaveSystem.Count < Max)     //don't include the slope exit
            {
                int branchLength = Random.Range(branchMin, branchMax);
                caveDoor.tag = "closedDoor";

                GenerateBranch(branchLength, i, tunnel, caveDoor);
            }
        }
        yield return null ;
    }

    void GenerateBranch(int branchLength, int doorNum, GameObject StartObject, Transform StartDoor)
    {
        GameObject nextObject = StartObject;
        Transform Door = StartDoor;
        Door.tag = "closedDoor";

        for (int i = 0; i <= branchLength; i++)
        {
            Vector3 Spawn = new Vector3(0, 0, 0);
            Spawn = placeObject(doorNum, Door);


            GameObject newObject = Instantiate(nextObject, Spawn, Quaternion.identity);
            newObject.transform.parent = transform;
            newObject.transform.eulerAngles = Door.eulerAngles;
            newObject.transform.GetChild(0).tag = "closedDoor";

            if (!canPlace(newObject))
            {
                Destroy(newObject);
                Door.tag = "openDoor";
                continue;
            }

            CaveSystem.Add(newObject);
            placeBreakAbleWall(Door);
            if (newObject.tag == "cave")
            {
                Door = newObject.transform.GetChild(3);
                if (i < branchLength)
                {
                    Door.tag = "closedDoor";
                }
                nextObject = tunnel;
                StartCoroutine(branchOffCave(newObject));
            }
            else
            {
                Door = newObject.transform.GetChild(1);// get exit
                if (i < branchLength)
                {
                    Door.tag = "closedDoor";
                }
                nextObject = pickObjectToMake();
            }

        }
    }
    GameObject pickObjectToMake()
    {
        float chance = Random.Range(0.0f, 1.0f);
        if (tunnelChance > chance)
        {
            return tunnel;
        }
        return cave;
    }

    void placeBreakAbleWall(Transform Door)
    {
        // this hole system should be linked to oxygen 
        float chance = Random.Range(0.0f, 1.0f);
        if (BreakAbleChance > chance)
        {
            if(currentBreakAble < maxBreakAble)// this should never be higher then the number of oxygen tanks 
            {
                GameObject nwBreakAble = Instantiate(breakAbleWall, Door.position, Quaternion.identity);
                currentBreakAble++;
            }
        }
    }
    #region find new objects transform
    Vector3 placeObject(int doorNum, Transform Door)
    {

        //needs to know which way they want to be placed and from that give back the right direction
        Vector3 Spawn = new Vector3(0, 0, 0);
        float spawnOffset = 0.0f;
        switch ((int)Door.eulerAngles.y)
        {
            //Bottom
            case 90:
                spawnOffset = Door.position.x + ModelRadius;
                Spawn = new Vector3(spawnOffset, Door.parent.position.y, Door.parent.position.z);
                break;
            //Right
            case 180:
                spawnOffset = Door.position.z - ModelRadius;
                Spawn = new Vector3(Door.parent.position.x, Door.parent.position.y, spawnOffset);
                break;
            //Left
            case 270:
                spawnOffset = Door.position.x - ModelRadius;
                Spawn = new Vector3(spawnOffset, Door.parent.position.y, Door.parent.position.z);
                break;
            //Top
            case 0:
                spawnOffset = Door.position.z + ModelRadius;
                Spawn = new Vector3(Door.parent.position.x, Door.parent.position.y, spawnOffset);
                break;
        }

        return Spawn;
    }
    Vector3 RotateObject(int doorNum, Transform lastBranchTransform)
    {
        Vector3 nextRot = lastBranchTransform.eulerAngles;

        switch (doorNum)
        {
            //Bottom
            case 0:
                nextRot.y = lastBranchTransform.eulerAngles.y + 180;
                break;
            //Right
            case 1:
                nextRot.y = lastBranchTransform.eulerAngles.y + 90;
                break;
            //Left
            case 2:
                nextRot.y = lastBranchTransform.eulerAngles.y + 270;
                break;
            //Top
            case 3:
                nextRot.y = lastBranchTransform.eulerAngles.y;
                break;
        }
        return nextRot;
    }

    bool canPlace(GameObject nwObject)
    {
        Vector3 objec1Pos = nwObject.transform.position;
        Vector3 object1Scale = nwObject.transform.localScale;
        foreach (GameObject obj in CaveSystem)
        {
            Vector3 object2Pos = obj.transform.position;
            Vector3 object2Scale = obj.transform.localScale;

            if (objec1Pos.x < object2Pos.x + object2Scale.x &&
                objec1Pos.x + object1Scale.x > object2Pos.x &&
                objec1Pos.y < object2Pos.y + object2Scale.y &&
                objec1Pos.y + object1Scale.y > object2Pos.y &&
                objec1Pos.z < object2Pos.z + object2Scale.z &&
                objec1Pos.z + object1Scale.z > object2Pos.z)
            {
                return false;
            }

            if (obj.name == "StartRoom(Clone)")
            {
                object2Pos = obj.transform.GetChild(3).position;
                object2Scale = obj.transform.GetChild(3).localScale;
                if (objec1Pos.x < object2Pos.x + object2Scale.x &&
               objec1Pos.x + object1Scale.x > object2Pos.x &&
               objec1Pos.y < object2Pos.y + object2Scale.y &&
               objec1Pos.y + object1Scale.y > object2Pos.y &&
               objec1Pos.z < object2Pos.z + object2Scale.z &&
               objec1Pos.z + object1Scale.z > object2Pos.z)
                {
                    return false;
                }
            }
            //if(obj.transform.position == nwObj)
            //{
            //return false;
            //}

        }
        return true;
    }
        #endregion
    #endregion

    #region spawn tank
    void spawnTank(Transform cave,bool firstCave)
    {
        if(!firstCave && cave.transform.position == new Vector3(0,0,0))
        {
            return;
        }
        float chance = Random.Range(0.0f, 1.0f);
        if (currentTanks < maxTanks)
        {
            if (tankChance > chance || firstCave)
            {
                GameObject nwOxygen = Instantiate(oxygenTank, cave.position, Quaternion.identity);
                currentTanks++;
            }
        }
    }

    void spawnAllTanks()
    {
        foreach (GameObject obj in CaveSystem)
        {
            if (obj.tag == "cave")
            {
                spawnTank(obj.transform,false);
            }
        }
    }
    #endregion

    #region cap paths
    void EndPath(Transform Door)
    {
        GameObject DeadEnd = Instantiate(deadEnd, new Vector3(Door.position.x, Door.position.y + 1.1f, Door.position.z), Quaternion.identity);
        DeadEnd.transform.parent = Door;
        DeadEnd.transform.eulerAngles = Door.eulerAngles;
        CaveSystem.Add(DeadEnd);
    }

    void EndAllPaths()
    {
        for (int j = 0; j < CaveSystem.Count; j++)
        {
            GameObject obj = CaveSystem[j];

            for (int i = 0; i < obj.transform.childCount; i++)
            {
                GameObject door = obj.transform.GetChild(i).gameObject;
                if (door.tag == "openDoor")
                {

                    float chance = Random.Range(0.0f, 1.0f);
                    if (j >= minPlacementToExit && j <= maxPlacementToExit && exitChance > chance && !exitPlaced)
                    {
                        createExit(door.transform);
                    }
                    else
                    {
                        EndPath(door.transform);
                    }
                    door.tag = "closedDoor";
                }
            }
        }
    }

        #region spawn exit
    void createExit(Transform Door)
    {
        ExitObject = Instantiate(exit, new Vector3(Door.position.x, Door.position.y, Door.position.z), Quaternion.identity);
        ExitObject.transform.parent = Door;
        ExitObject.transform.eulerAngles = new Vector3(Door.eulerAngles.x, Door.eulerAngles.y, Door.eulerAngles.z);
        CaveSystem.Add(ExitObject);
        exitPlaced = true;
    }

        #endregion
    #endregion

}
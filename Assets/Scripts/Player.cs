﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class Player : MonoBehaviour
{
    Animator anim;

    [Header("Movement Stuff")]

    public int walkSpeed;
    public int runSpeed;
    private int speed;
    private float tempo;
    public float boostSpeedModifier = 2f;

    public int rotateSpeed;

    public bool MovingForward = false;
    public bool MovingBackwards = false;

    private Rigidbody rb;
    private Vector3 move;

    
    private Camera mainCam;
    private Vector3 camF;
    private Vector3 camR;
    private Vector3 desiredMoveDirection;

    private float joystick_deadzone = 0.3f;
    private bool running = false;

    [Header("Swimming")]
    public float oxygen = 100;
    public float oxygenDecrease = 5;
    public bool IsInWater = false;
    private FogMode fogMode;
    private float fogDensity;
    private Color fogColour;
    private bool fogEnabled;
    private float waterSurfacePosY = 0.0f;
    public float aboveWaterTolerance = 0.5f;

    public Transform head;

    [Range(0.5f, 3.0f)]
    public float UpDownSpeed = 1.0f;
    public Color fogColurWater;

    public PostProcessingProfile PPP_Land;
    public PostProcessingProfile PPP_Underwater;

    [Header("Other")]
    public GameObject oxygenTank;
    public Image deathFade;
    private float fadeCounter = 0;

    [Header("Audio Stuff")]
    private GameObject audioManager;
    public AudioClip[] clips;

    // Use this for initialization
    void Start()
    {
        aboveWaterTolerance = 0.5f;
        fogColurWater = new Color(0.2f, 0.65f, 0.75f, 0.5f);
        fogMode = RenderSettings.fogMode;
        fogDensity = RenderSettings.fogDensity;
        fogColour = RenderSettings.fogColor;
        fogEnabled = RenderSettings.fog;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;
        audioManager = GameObject.Find("Audio Source");
    }

    // Update is called once per frame
    void Update()
    {

        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        anim.SetFloat("Vertical", v);
        anim.SetFloat("Horizontal", h);

        //Testing oxygen
        if (Input.GetKeyDown(KeyCode.P))
        {
            IncreaseOxygen(20.0f);
        }

        if (IsInWater)
        {
            if (isUnderWater())
            {
                rb.drag = 3.0f;
                Dive(v, h);
                setRenderDive();
                LowerOxygen(oxygenDecrease);
            }
            else
            {
                setRenderDefault();
                Movement(h, v);
                setDirection(h, v);
            }
        }
        else
        {
            Movement(h, v);
            setDirection(h, v);
        }
        if (Input.GetButtonDown("Taunt"))
        {
            Taunt();
        }

        if (oxygen <= 0)
        {
            Dying();
        }
        else
        {
            deathFade.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        }
        //GetOxygen();
    }

    private void LowerOxygen(float amount)
    {
        oxygen -= amount * Time.deltaTime;
        if (oxygen < 0)
        {
            oxygen = 0;
        }
    }
    private void IncreaseOxygen(float amount)
    {
        oxygen += amount;
        if (oxygen > 100)
        {
            oxygen = 100;
        }
    }
    public float GetOxygen()
    {

        return oxygen;
    }
    //Does the movement
    void Movement(float h, float v)
    {
        if (Input.GetButton("Run"))
        {
            speed = runSpeed;
            anim.SetBool("Running", true);
            running = true;
            tempo = 1.5f;
            if (anim.GetBool("Swimming"))
            {
                anim.speed = 3;
            }
        }
        else
        {
            speed = walkSpeed;
            anim.SetBool("Running", false);
            running = false;
            tempo = 0.9f;
            if (anim.GetBool("Swimming"))
            {
                anim.speed = 1;
            }
        }

        if (v > joystick_deadzone || v < -joystick_deadzone || h > joystick_deadzone || h < -joystick_deadzone)
        {

            desiredMoveDirection = Vector3.RotateTowards(desiredMoveDirection, desiredMoveDirection, 10 * Time.deltaTime, 1000);
            desiredMoveDirection = desiredMoveDirection.normalized;
            transform.rotation = Quaternion.LookRotation(desiredMoveDirection);

            Vector3 move = desiredMoveDirection * speed;

            //move.y = rb.velocity.y;
            rb.velocity = move;

            transform.position += move * Time.deltaTime;
            footSteps(tempo, 0.01f);

        }


    }

    void footSteps(float speed, float volume)
    {
        if (!audioManager.GetComponent<AudioScript>().audioSource.isPlaying)
        {
            audioManager.GetComponent<AudioScript>().audioSource.pitch = speed;
            audioManager.GetComponent<AudioScript>().audioSource.volume = volume;
            audioManager.GetComponent<AudioScript>().setAudio(clips[Random.Range(0, 2)]);
            audioManager.GetComponent<AudioScript>().audioSource.time = 0.0f;
            audioManager.GetComponent<AudioScript>().audioSource.Play();
        }

    }
    void setDirection(float h, float v)
    {
        Transform cameraTransform = mainCam.transform;
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);

        forward.y = 0;
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        right.y = 0;

        desiredMoveDirection = (forward * v + right * h);
    }

    void Dive(float v, float h)
    {
        Vector3 move = new Vector3(0, 0, 0);

        //also maybe set player rotation to face up or down when animation is implemented
        if (head.position.y < (waterSurfacePosY + aboveWaterTolerance))
        {
            if (Input.GetButton("Surface"))//Surface
            {
                move.y = +UpDownSpeed;
            }
            else if (Input.GetButton("Dive"))//Dive
            {

                move.y = -UpDownSpeed;
            }

        }
        rb.velocity = move;

        transform.position += move * Time.deltaTime;

        SwimMovement(v, h);
        setDirection(h, v);
    }

    void SwimMovement(float v, float h)
    {
        float swimSpeed = 3.5f;

        if (v > joystick_deadzone || v < -joystick_deadzone || h > joystick_deadzone || h < -joystick_deadzone)
        {

            desiredMoveDirection = Vector3.RotateTowards(desiredMoveDirection, desiredMoveDirection, 10 * Time.deltaTime, 1000);
            desiredMoveDirection = desiredMoveDirection.normalized;
            transform.rotation = Quaternion.LookRotation(desiredMoveDirection);

            Vector3 move = desiredMoveDirection * swimSpeed;

            rb.velocity = move;
            transform.position += move * Time.deltaTime;
        }
    }

    void Taunt()
    {
        if (oxygen > 25)
        {
            GameObject OT = Instantiate(oxygenTank, head.transform.position, this.transform.rotation);
            //OT.transform.eulerAngles = new Vector3(90, this.transform.rotation.y, this.transform.rotation.z);
            OT.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * 5);
            OT.transform.eulerAngles = new Vector3(90, this.transform.rotation.y, this.transform.rotation.z);
            oxygen -= 25;
        }

    }

    private void Dying()
    {
        if (fadeCounter <= 0.8f)
        {
            fadeCounter += 0.005f;
        }
        deathFade.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, fadeCounter);

        StartCoroutine(Death());
    }
    private IEnumerator Death()
    {
        yield return new WaitForSeconds(10);
        if (oxygen <= 0)
        {
            //set death bool anim to true
        }
    }
    bool isUnderWater()
    {
        return head.position.y < (waterSurfacePosY);
    }

    void setRenderDive()
    {

        RenderSettings.fog = true;
        RenderSettings.fogColor = fogColurWater;
        RenderSettings.fogDensity = 0.1f;
        RenderSettings.fogMode = FogMode.Exponential;

        mainCam.GetComponent<PostProcessingBehaviour>().enabled = true;//.profile = PPP_Underwater;
    }

    void setRenderDefault()
    {
        RenderSettings.fog = fogEnabled;
        RenderSettings.fogColor = fogColour;
        RenderSettings.fogMode = fogMode;
        RenderSettings.fogDensity = fogDensity;
        mainCam.GetComponent<PostProcessingBehaviour>().enabled = false;//.profile = PPP_Land;
    }

    public float getYPos()
    {
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        float height = capsule.height;
        return transform.position.y + height;
    }

    private void OnTriggerStay(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Water")//water
        {
            IsInWater = true;
            waterSurfacePosY = other.gameObject.transform.position.y;//* other.bounds.size.y
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Water")//water
        {
            IsInWater = false;
            waterSurfacePosY = 0.0f;

        }
    }
}
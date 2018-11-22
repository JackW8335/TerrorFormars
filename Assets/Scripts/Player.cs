using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class Player : MonoBehaviour
{
    Animator anim;

    [Header("Movement Stuff")]
    public float oxygen = 100;
    public int walkSpeed;
    public int runSpeed;
    private int speed;
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

    //Swim
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


        if (IsInWater)
        {
            if (isUnderWater())
            {
                rb.drag = 3.0f;
                Dive(v, h);
                setRenderDive();
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

        GetOxygen();
    }

    public float GetOxygen()
    {
        oxygen -= 5 * Time.deltaTime;
        if(oxygen < 0)
            oxygen = 0;

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
            footSteps(1.5f, 0.01f);
        }
        else
        {
            speed = walkSpeed;
            anim.SetBool("Running", false);
            running = false;
            
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
            footSteps(1.0f,0.01f);

        }

        
    }

   void footSteps(float speed, float volume)
    {
        if (!audioManager.GetComponent<AudioScript>().audioSource.isPlaying)
        {
            audioManager.GetComponent<AudioScript>().audioSource.pitch = speed;
            audioManager.GetComponent<AudioScript>().audioSource.volume = volume;
            audioManager.GetComponent<AudioScript>().setAudio(clips[Random.Range(1,3)]);
            audioManager.GetComponent<AudioScript>().audioSource.time = 0.0f;
            audioManager.GetComponent<AudioScript>().audioSource.PlayDelayed(-2.0f);
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

        if (head.position.y < (waterSurfacePosY + aboveWaterTolerance))
        {
            if (Input.GetKey(KeyCode.P))//Surface
            {
                move.y = +UpDownSpeed;
            }else if (Input.GetKey(KeyCode.I))//Dive
            {
                move.y = -UpDownSpeed;
            }

        }
        rb.velocity = move;

        transform.position += move * Time.deltaTime;

        SwimMovement(v,h);
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
        if(LayerMask.LayerToName(other.gameObject.layer) == "Water")//water
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
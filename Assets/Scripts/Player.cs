using System.Collections;
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

    private AudioSource audioSource;
    public bool submerged;
    public bool emerged;
    
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
    float rotSpeed = 50;
    private float swimmingAngle = 0;

    public Transform body;
    public Transform head;

    [Range(0.5f, 3.0f)]
    public float UpDownSpeed = 1.0f;
    public Color fogColurWater;

    public PostProcessingProfile PPP_Land;
    public PostProcessingProfile PPP_Underwater;

    [Header("Other")]
    public GameObject oxygenTank;
    public Image deathFade;
    public bool canCarry = true;
    private float fadeCounter = 0;
    private bool alive = true;

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
        audioSource = audioManager.GetComponent<AudioScript>().audioSource;
}

    // Update is called once per frame
    void Update()
    {

        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        anim.SetFloat("Vertical", v);
        anim.SetFloat("Horizontal", h);

        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Swimming.swimming"))
        {
            anim.SetBool("StartedSwim", true);
        }

        if (anim.GetBool("Dead"))
        {
            anim.SetBool("InDead", true);
        }

        if (IsInWater)
        {
            if (isUnderWater())
            {
                if (!submerged)
                {
                    Submerge();
                    submerged = true;
                    emerged = false;
                }
                else if(submerged)
                {
                    WaterAmbient();
                }
                anim.SetBool("Swimming", true);
                rb.drag = 3.0f;
                Dive(v, h);
                setRenderDive();
                LowerOxygen(oxygenDecrease);

                if(Input.GetButton("Dive"))
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                                  Quaternion.Euler(60, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), rotSpeed * Time.deltaTime);
                    swimmingAngle = transform.rotation.x;
                }
                else if(Input.GetButton("Surface"))
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                                  Quaternion.Euler(-60, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), rotSpeed * Time.deltaTime);
                    swimmingAngle = transform.rotation.x;
                }
                else
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                                  Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), rotSpeed * Time.deltaTime);
                    swimmingAngle = transform.rotation.x;
                }
            }
            else if(ExitingWater())
            {
                rb.drag = 3.0f;
                Dive(v, h);
            }
            else
            {
                anim.SetBool("Swimming", false);
                anim.SetBool("StartedSwim", false);
                setRenderDefault();
                Movement(h, v);
                setDirection(h, v);
                
                setRenderDefault();

                if (submerged)
                {
                    //Play Emerge sound effect if above water and currently submerged and not emerged
                    Emerge();
                    emerged = true;
                    submerged = false;
                }
                if (Input.GetButtonDown("Taunt"))
                {
                    if (!canCarry)
                    {
                        anim.SetBool("Throw", true);
                        StartCoroutine("launchAirCanister");
                    }
                }
                else
                {
                    anim.SetBool("Throw", false);
                }
            }
        }
        else
        {
            setRenderDefault();
            anim.SetBool("Swimming", false);
            Movement(h, v);
            setDirection(h, v);
            anim.SetBool("Swimming", false);
            anim.SetBool("StartedSwim", false);

            if (Input.GetButtonDown("Taunt"))
            {
                if (!canCarry)
                {
                    anim.SetBool("Throw", true);
                StartCoroutine("launchAirCanister");
                }
            }
            else
            {
                anim.SetBool("Throw", false);
            }
        }
        

        if (oxygen <= 0)
        {
            Dying();
        }
        else
        {
           // deathFade.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
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
        }
        else
        {
            speed = walkSpeed;
            anim.SetBool("Running", false);
            running = false;
            tempo = 1.2f;
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
            footSteps(tempo, 0.1f);

        }



    }

    void footSteps(float speed, float volume)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.pitch = speed;
            audioSource.volume = volume;
            audioManager.GetComponent<AudioScript>().setAudio(clips[Random.Range(0, 2)]);
            audioSource.time = 0.0f;
            audioSource.Play();
        }

    }
    void setDirection(float h, float v)
    {
        Transform cameraTransform = mainCam.transform;
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);

        Vector3 up = cameraTransform.TransformDirection(Vector3.up);

        up.y = 0;
        up = up.normalized;

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
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                //              Quaternion.Euler(-60, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), rotSpeed * Time.deltaTime);

                anim.SetBool("Moving", true);


            }
            else if (Input.GetButton("Dive"))//Dive
            {
                move.y = -UpDownSpeed;
                anim.SetBool("Moving", true);
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

            anim.SetBool("Moving", true);
        }
        else
        {
            if (!Input.GetButton("Dive") && !Input.GetButton("Surface"))
            {
                anim.SetBool("Moving", false);
            }
        }

        if (Input.GetButton("Run"))
        {
            speed = runSpeed;
            anim.SetBool("Running", true);
            running = true;
        }
        else
        {
            speed = walkSpeed;
            anim.SetBool("Running", false);
            running = false;
        }

    }

    void Taunt()
    {
        anim.SetBool("Throw", true);
        //if (oxygen > 25)
        //{
        //    GameObject OT = Instantiate(oxygenTank, head.transform.position, this.transform.rotation);
        //    //OT.transform.eulerAngles = new Vector3(90, this.transform.rotation.y, this.transform.rotation.z);

        //    OT.transform.eulerAngles = new Vector3(90, this.transform.rotation.y, this.transform.rotation.z);
        //    oxygen -= 25;
        //}

    }

    private void Dying()
    {

        if (oxygen <= 0 && alive)
        {
            anim.SetBool("Dead", true);
            audioSource.Stop();
            PlayerAudio(1.0f, 1.0f, 6, 0.0f);
            alive = false;

            GetComponentInParent<WinState>().state = WinState.states.DEFEAT;
        }
        rb.AddForce(Physics.gravity * 300);
    }

    bool isUnderWater()
    {
        return head.position.y <= (waterSurfacePosY);
    }

    bool ExitingWater()
    {
        return body.position.y < (waterSurfacePosY);
    }

    void setRenderDive()
    {

        RenderSettings.fog = true;
        RenderSettings.fogColor = fogColurWater;
        RenderSettings.fogDensity = 0.1f;
        RenderSettings.fogMode = FogMode.Exponential;

    }

    void setRenderDefault()
    {
        RenderSettings.fog = false;

        //.profile = PPP_Land;
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

    void PlayerAudio(float pitch, float volume, int clip, float delay)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.pitch = pitch;
            audioSource.volume = volume;
            audioManager.GetComponent<AudioScript>().setAudio(clips[clip]);
            audioSource.PlayDelayed(delay);
        }
    }

    void Submerge()
    {
        audioSource.Stop();
        PlayerAudio(1.0f, 1.0f, 3, 0.0f);
    }

    void WaterAmbient()
    {
        PlayerAudio(1.0f, 1.0f, 4, 0.0f);
    }

    void Emerge()
    {
        audioSource.Stop();
        PlayerAudio(1.0f, 1.0f, 5, 0.0f);
    }


    private void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Water")//water
        {
            IsInWater = false;
            waterSurfacePosY = 0.0f;

        }
    }

    private IEnumerator launchAirCanister()
    {
        
            yield return new WaitForSeconds(1.5f);
            canCarry = true;
            GameObject obj = GameObject.FindGameObjectWithTag("Throwable");

            obj.transform.parent = null;
            obj.AddComponent<Rigidbody>();
            obj.GetComponent<Rigidbody>().AddForce(this.transform.forward * 10, ForceMode.Impulse);
        
    }
}
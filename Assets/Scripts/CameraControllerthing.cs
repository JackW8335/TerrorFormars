using UnityEngine;
using System.Collections;

public class CameraControllerthing : MonoBehaviour
{
    public GameObject cameraTarget;
    private GameObject cameraTargetStart;
    public float rotateSpeed;
    float rotate;
    public float offsetDistance;
    public float offsetHeight;
    public float smoothing;
    public bool rotation90 = false;
    public bool rotated = false;
    Vector3 offset;
    bool following = true;
    Vector3 lastPosition;

    private bool left = false;
    private bool right = false;
    public int cameraRotation = 0;

    float joystick_deadzone = 0.3f;

    void Start()
    {
        lastPosition = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y + offsetHeight, cameraTarget.transform.position.z - offsetDistance);
        offset = lastPosition;

        cameraTargetStart = cameraTarget;
    }

    void Update()
    {
        if (!rotation90)
        {
            normalCamera();
        }
        else if (rotation90)
        {
            if (Input.GetKey(KeyCode.Q) || Input.GetAxisRaw("Rotation") < -joystick_deadzone)
            {
                right = true;
            }
            else if (Input.GetKey(KeyCode.E) || Input.GetAxisRaw("Rotation") > joystick_deadzone)
            {
                left = true;
            }
        }

        offset = Quaternion.AngleAxis(rotate * rotateSpeed, Vector3.up) * offset;
        transform.position = offset;
        transform.position = new Vector3(Mathf.Lerp(lastPosition.x, cameraTarget.transform.position.x + offset.x, smoothing * Time.deltaTime),
            Mathf.Lerp(lastPosition.y, cameraTarget.transform.position.y + offset.y, smoothing * Time.deltaTime),
            Mathf.Lerp(lastPosition.z, cameraTarget.transform.position.z + offset.z, smoothing * Time.deltaTime));
        transform.LookAt(cameraTarget.transform.position);

        rotating90();
    }

	void LateUpdate()
	{
		lastPosition = transform.position;
        linecastCheck();
	}

    void linecastCheck()
    {
        RaycastHit hitinfo;

        if (Physics.Linecast(cameraTarget.transform.position, transform.position, out hitinfo))
        {
            transform.position = hitinfo.point;
        }
    }

    void normalCamera()
    {
        if (Input.GetKey(KeyCode.Q) || Input.GetAxisRaw("Rotation") < -joystick_deadzone)
        {
            if (!Input.GetKey(KeyCode.Q))
            {
                rotate = Input.GetAxisRaw("Rotation");
            }
            else
            {
                rotate = -1;
            }
        }

        else if (Input.GetKey(KeyCode.E) || Input.GetAxisRaw("Rotation") > joystick_deadzone)
        {

            if (!Input.GetKey(KeyCode.E))
            {
                rotate = Input.GetAxisRaw("Rotation");
            }
            else
            {
                rotate = 1;
            }
        }
        else
        {
            rotate = 0;
        }
    }

    void rotating90()
    {
        if (rotation90 && !rotated)
        {
            offset = new Vector3(0, offset.y, offset.z);
            rotated = true;
        }

        if (cameraRotation == 90 / rotateSpeed || cameraRotation == -90 / rotateSpeed)
        {
            left = false;
            right = false;
            cameraRotation = 0;
        }

        if (left == true)
        {
            rotate = 1;
            cameraRotation++;
        }
        else if (right == true)
        {
            rotate = -1;
            cameraRotation++;
        }
        else
        {
            rotate = 0;
        }
    }
}
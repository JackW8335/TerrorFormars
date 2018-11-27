using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineWalker : MonoBehaviour {

    public IEnumerator PausePlatform()
    {
        yield return new WaitForSeconds(2f);
    }

    private void Start()
    {
        Debug.Log("Spline length = " + spline.GetLength());
    }

    public enum SplineWalkerMode
    {
        Once,
        Loop,
        PingPong
    }

    public BezierSpline spline;     //spline object

    public float duration;  //duration of movement

    public SplineWalkerMode mode;   //adjust the type of walk
    private bool goingForward = true;
    [HideInInspector]
    public float progress;
    public bool lookForward;

    private void Update()
    {
        if (goingForward)
        {
            progress += Time.deltaTime / duration;
            if (progress > 1f)
            {
                if (mode == SplineWalkerMode.Once)
                {
                    progress = 1f;
                }
                else if (mode == SplineWalkerMode.Loop)
                {
                    progress -= 1f;
                }
                else
                {
                    progress = 2f - progress;
                    goingForward = false;
                }
            }
        }
        else
        {
            progress -= Time.deltaTime / duration;
            if (progress < 0f)
            {
                progress = -progress;
                goingForward = true;
            }
        }
        //Debug.Log("Progress = " + progress);
        Vector3 position = spline.GetPoint(progress);
        transform.localPosition = position;    //gradually move across spline
        if (lookForward)
        {
            transform.LookAt(position + spline.GetDirection(progress));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Log.cs
 * ------------
 * Responsible for activating the walker tile if the user
 * looks at an angle downwards.
 * It also checks conditions such as if the walker tile 
 * will intersect with another object. 
 *
 */

public class Log : MonoBehaviour {

    // time variables; used so that the code block under Update runs per second and not per frame
    int interval = 1;
    float nextTime = 0;
    // target: the walker activator
    public Transform target;
    // walker tile
    public GameObject walker;
    Vector3 upwardVector, targetDir;
    // added to Johnny Walker's position for evacuation scene para sumakto sa reticle
    Vector3 extraNudge;
    public GvrReticlePointer reticle;
    float angle, prevAngle;
    RaycastHit hit;
    float upperThreshold, lowerThreshold;

    int layerMask;
    int layerMask2;

    // Use this for initialization
    void Start () {

        if (target == null || walker == null)
        {
            target = GameObject.Find("Johnny Walker").transform;
            walker = GameObject.Find("Johnny Walker");

        }
        // set upward vector 
        upwardVector = new Vector3(0.0f, 1.0f, 0.0f);
        prevAngle = Vector3.Angle(upwardVector, upwardVector);
        // hide walker object
        walker.SetActive(false);
        layerMask = 1 << LayerMask.NameToLayer("CanActivate");
        layerMask2 = 1 << LayerMask.NameToLayer("CannotActivate");
        lowerThreshold = 120.0f;
        upperThreshold = 180.0f;
    }
	
	// Update is called once per frame
	void Update () {

        if (target == null || walker == null)
        {
            target = GameObject.Find("Johnny Walker").transform;
            walker = GameObject.Find("Johnny Walker");

        }
        angle = Vector3.Angle(upwardVector, transform.forward);
        extraNudge = SceneManager.GetActiveScene().name != "evacuation" ? Vector3.zero : new Vector3(-0.05f, 0f, 0f);
        lowerThreshold = SceneManager.GetActiveScene().name != "evacuation" ? 120.0f : 125.0f;
        upperThreshold = SceneManager.GetActiveScene().name != "evacuation" ? 180.0f : 190.0f;

        if (Time.time >= nextTime) {
            // if user is looking at an angle between the lower and upper threshold
            if (angle >= lowerThreshold && angle < upperThreshold) { 
                // render the walker thing
                // check if the ray intersects any objects excluding the player layer
                if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMask) & !(Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMask2)))
                {
                    // active walker tile
                    walker.SetActive(true);

                    // change walker position if reticle has moved
                    if (!angle.Equals(prevAngle)) target.transform.position = reticle.CurrentRaycastResult.worldPosition + extraNudge;

                    prevAngle = angle;
                } 
            }

            else {
                walker.SetActive(false);
            }
            nextTime += interval;

        }
    }
}

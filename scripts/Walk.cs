using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Walk.cs
 * ------------
 * This script is responsible for making the user
 * walk around the environment.
 * 
 * Attached to Player object.
 * 
 */

public class Walk : MonoBehaviour {

    // player speed
    public float playerSpeed;
    public bool isWalking;
    const int INCORRECT = -1;
    const int INACTIVE = 0;

    // Use this for initialization
    void Start () {
        isWalking = false;
        if (!isWalking && !gameObject.GetComponent<Adaptivity>().isTicking) gameObject.GetComponent<Adaptivity>().RunTimer();
    }
	
	// Update is called once per frame
    void Update () {
        // if the walking toggle is "on", update user's position to simulate walking
        if (isWalking == true) {
            transform.position = transform.position + Camera.main.transform.forward * playerSpeed * Time.deltaTime;
        }
    }

    // called from the Johnny Walker object every time walker tile is clicked.
    public void WalkAround() {
        isWalking = !(isWalking);

        // if the user isn't walking, start inactivity timer
        if (!isWalking) gameObject.GetComponent<Adaptivity>().RunTimer();
        else if (isWalking && gameObject.GetComponent<Adaptivity>().isTicking) gameObject.GetComponent<Adaptivity>().ResetTimer();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Fall_Script.cs
 * ------------
 * This script is responsible for the scene where the user can
 * fall after attempting to walk towards the still water without grabbing the stick.
 * 
 */

public class Fall_Script : MonoBehaviour
{
    bool enter = true; // rid public
    bool stay = true;

    AudioSource audio;
    public GameObject resPoint;


    void Start()
    {
        audio = GetComponent<AudioSource>();
        resPoint = GameObject.Find("Checkpoints/Fall");
    }

    // respawn user to checkpoint before hazard 
    void Respawn() {
        Debug.Log("Respawn");
        GameObject.Find("Player").GetComponent<Walk>().isWalking = false;        
        GameObject.Find("Player").transform.position = resPoint.transform.position;

        GameObject.Find("Player").GetComponent<Adaptivity>().eventRunning = "Fall";
        GameObject.Find("Player").GetComponent<Adaptivity>().WrongMove();    
    }

    // handle event when user enters the hazard collider
    private void OnTriggerEnter(Collider other)
    {
        if (enter && other.gameObject.name == "Player")
        {
            GameObject.Find("Player").GetComponent<Walk>().isWalking = false;            
            audio.Play();
        }
    }

    // sends user back to checkpoint after hazard interacts with user
    private float stayCount = 0.0f;
    private void OnTriggerStay(Collider other)
    {
        if (stay)
        {
            if (stayCount > 3.0f)
            {
                stayCount = 0.0f;
                Respawn();
            }
            
            stayCount = stayCount + Time.deltaTime;
        }
    }
}
